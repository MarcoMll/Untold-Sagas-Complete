using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomInspector;

[RequireComponent(typeof(CardTransformHandler))]
public class DecisionCard : MonoBehaviour
{
    [HorizontalLine("Setup")]
    [Header("Decision title")]
    [SerializeField] private TMP_Text titleTextField;
    [Header("Decision description")]
    [SerializeField] private TMP_Text descriptionTextField;
    [SerializeField] private KeywordsHighlighter keywordsHighlighter;
    [SerializeField] private KeywordsLinker keywordsLinker;
    [Header("Decision visuals")]
    [SerializeField] private Image cardImage;
    [SerializeField] private UIInteractionDetector uiInteractionDetector;
    [HorizontalLine("Conditions GUI")]
    [SerializeField] private ImageVisualizer requiredCharacteristic;
    [SerializeField] private ImageVisualizer requiredItemIcon;
    [HorizontalLine("Sound effects")]
    [SerializeField] private AudioClip defaultClickSound;
    [SerializeField] private AudioClip characteristicSound;
    
    private Decision _decision;
    private AudioClip _clickSound;

    private EventsManager _eventsManager;
    private PlayerStats _playerStats;
    private SecondaryEventsVisualizer _secondaryEventsVisualizer;

    public CardTransformHandler CardTransformHandler { get; private set; }

    private void Awake()
    {
        CardTransformHandler = GetComponent<CardTransformHandler>();
        
        uiInteractionDetector.onLeftClick += SelectCard;
        uiInteractionDetector.onMouseEnter += CardTransformHandler.ScaleUp;
        uiInteractionDetector.onMouseExit += CardTransformHandler.ScaleDown;
    }
    
    private void ToggleCardComponent(GameObject targetObject, bool value, bool disableParent = false)
    {
        targetObject.SetActive(value);
        
        if (disableParent == true)
        {
            targetObject.transform.parent.gameObject.SetActive(value);
        }
    }
    
    private void SelectCard()
    {
        if (CardTransformHandler.Selected == false)
        {
            CardTransformHandler.SelectCard();
            ApplyInteractions();

            _eventsManager.CardController.HideAllCardsOnTheScene(this);
        }
        else
        {
            CardTransformHandler.HideCard(true);
            SetClickSound(defaultClickSound);
        }

        PlaySound(_clickSound);
    }

    private void PlaySound(AudioClip clip)
    {
        SceneComponentsHolder.Instance.PlaySoundEffect(clip);
    }

    private void ApplyInteractions()
    {
        foreach (var currentInteraction in _decision.Interactions)
        {
            switch (currentInteraction)
            {
                case Decision.Interaction.AddHeart:
                    _playerStats.ChangeHeartsAmount(+1);
                    break;
                case Decision.Interaction.RemoveHeart:
                    _playerStats.ChangeHeartsAmount(-1);
                    break;
                case Decision.Interaction.AddCharacteristic:
                    AddCharacteristic();
                    break;
                case Decision.Interaction.ChangeRelationship:
                    ChangeRelationship();
                    break;
                case Decision.Interaction.AddItem:
                    _playerStats.ItemHandler.AddItem(_decision.AddableItem);
                    break;
                case Decision.Interaction.RemoveItem:
                    _playerStats.ItemHandler.RemoveItem(_decision.RemovableItem);
                    break;
                case Decision.Interaction.PlayChapter:
                    EditChapter();
                    break;
                case Decision.Interaction.PlayCutscene:
                    _eventsManager.CutsceneController.SetCutscene(_decision.Cutscene);
                    break;
                case Decision.Interaction.LaunchMinigame:  
                    LaunchMinigame();
                    break;
                case Decision.Interaction.SaveGameData:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void LaunchMinigame()
    {
        MinigameController.Instance.SetupMinigame(_decision.Minigame, _decision.EventIdOnPass, _decision.EventIdOnLoss);
    }
    
    private void AddCharacteristic()
    {
        if (_decision.Unique)
        {
            if (_decision.AddableUniqueCharacteristic.RequirementsMet())
            {
                VisualizeUniqueCharacteristic(_decision.AddableUniqueCharacteristic);
                _playerStats.AddCharacteristic(_decision.AddableUniqueCharacteristic.RewardCharacteristic);

            }
            
            return;       
        }
        
        _playerStats.AddCharacteristic(_decision.AddableCharacteristic);
        SecondaryEventsVisualizer.Instance.VisualizeAddableCharacteristic(_decision.AddableCharacteristic);
    }

    private void VisualizeUniqueCharacteristic(UniqueCharacteristicData characteristicData)
    {
        UniqueCharacteristicTemplate prefab = _secondaryEventsVisualizer.UniqueCharacteristicTemplate;
        SecondaryEvent secondaryEvent = _secondaryEventsVisualizer.InstantiateEvent(prefab);
        UniqueCharacteristicTemplate characteristicInteraction = secondaryEvent as UniqueCharacteristicTemplate;

        characteristicInteraction.Setup(characteristicData);
        _secondaryEventsVisualizer.VisualizeInteraction(secondaryEvent);
    }
    
    private void ChangeRelationship()
    {
        if (_decision.CharactersRelationshipModifier.RelationshipModifiers.Length <= 0) return;

        foreach (var modifier in _decision.CharactersRelationshipModifier.RelationshipModifiers)
        {
            Character targetCharacter = modifier.Character;
            int amount = modifier.Relationship;

            _playerStats.ChangeRelationship(targetCharacter, amount);
            _secondaryEventsVisualizer.VisualizeCharacterRelationshipInteraction(targetCharacter, amount);
        }
    }

    private void ResetTargetID()
    {
        _decision.TargetEventID = 0;
    }

    private void EditChapter()
    {
        ResetTargetID();
        _eventsManager.SetChapter(_decision.TargetChapter);
    }

    private void SetEventsManager(EventsManager eventsManager)
    {
        _eventsManager = eventsManager;
        _playerStats = PlayerStats.Instance;
        _secondaryEventsVisualizer = _eventsManager.SecondaryEventsVisualizer;
    }

    private void SetImage(Sprite image)
    {
        if (image != null)
        {
            cardImage.sprite = image;
        }
        else
        {
            cardImage.gameObject.SetActive(false);
        }
    }

    private void SetClickSound(AudioClip clip)
    {
        _clickSound = clip;
    }

    private void CheckForRequirements()
    {
        if (_decision.CharacteristicRequirement() == true)
        {
            ToggleCardComponent(requiredCharacteristic.gameObject, true, false);
            requiredCharacteristic.SetupImage(_decision.RequiredCharacteristic.Icon);
            requiredCharacteristic.Hide();
        }
        else
        {
            ToggleCardComponent(requiredCharacteristic.gameObject, false, false);
        }

        if (_decision.ItemRequirement() == true)
        {
            ToggleCardComponent(requiredItemIcon.gameObject, true, false); 
            requiredItemIcon.SetupImage(_decision.RequiredItem.Icon);
            requiredItemIcon.Show();
        }
        else
        {
            ToggleCardComponent(requiredItemIcon.gameObject, false, false); 
        }
        
        var hasRequirements = _decision.CharacteristicRequirement() || _decision.ItemRequirement();
        if (hasRequirements) _eventsManager.SceneComponents.PlaySoundEffect(characteristicSound);
    }

    private void HighlightDescriptionTextKeywords()
    {
        var keywordsList = new List<string>();

        foreach (var tooltipData in TooltipsHolder.Instance.Tooltips)
        {
            foreach (var keyword in tooltipData.Keywords)
            {
                keywordsList.Add(keyword);
            }
        }
        
        keywordsHighlighter.HighlightWords(keywordsList.ToArray());
        keywordsLinker.LinkKeywords(keywordsList.ToArray());
    }
    
    public void InitializeCard(Decision decision, EventsManager eventsManager)
    {
        _decision = decision;
        SetEventsManager(eventsManager);
        CheckForRequirements();
        
        CardTransformHandler.ShowCard();

        string titleKey = LocalizationKeysHolder.GetDecisionTitleKey(_eventsManager.GetCurrentChapter(), _eventsManager.GetCurrentEventID(), decision.DecisionIndex);
        titleTextField.text = LocalizationManager.Instance.GetTranslation(titleKey);
        
        string descriptionKey = LocalizationKeysHolder.GetDecisionDescriptionKey(_eventsManager.GetCurrentChapter(), _eventsManager.GetCurrentEventID(), decision.DecisionIndex);
        descriptionTextField.text = LocalizationManager.Instance.GetTranslation(descriptionKey);
        HighlightDescriptionTextKeywords();

        SetImage(decision.Image);

        AudioClip clickSound = decision.ClickSound == null ? defaultClickSound : decision.ClickSound;
        SetClickSound(clickSound);
    }
    
    public void PlayEvent()
    {
        _eventsManager.SetEventID(_decision.TargetEventID);
        _eventsManager.PlayEvent();
    }
}
