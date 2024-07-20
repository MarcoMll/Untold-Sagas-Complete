using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryEventsVisualizer : MonoBehaviour, ISkippable
{
    [SerializeField] private Transform grid;
    [Header("Setups")]
    [SerializeField] private RelationshipInteractionSetup relationshipInteractionPrefab;
    [SerializeField] private RelationshipInteractionSetup fullSizeCharacterInteractionPrefab;
    [SerializeField] private CharacteristicInteractionSetup characteristicInteractionPrefab;
    [SerializeField] private CharacteristicStatsSetup characteristicStatsSetup;
    [SerializeField] private UniqueCharacteristicTemplate uniqueCharacteristicTemplate;

    private Coroutine _clearCoroutine;
    private List<Transform> _interactions = new List<Transform>();
    private List<List<Transform>> _interactionGroups = new List<List<Transform>>();
    private int _currentGroupIndex = 0;
    private EventsManager _eventsManager;

    public static SecondaryEventsVisualizer Instance { private set; get; }
    
    public RelationshipInteractionSetup RelationshipInteractionPrefab { get => relationshipInteractionPrefab; }
    public CharacteristicInteractionSetup CharacteristicInteractionPrefab { get => characteristicInteractionPrefab; }
    public CharacteristicStatsSetup CharacteristicStatsSetup { get => characteristicStatsSetup; }
    public UniqueCharacteristicTemplate UniqueCharacteristicTemplate { get => uniqueCharacteristicTemplate; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _eventsManager = GetComponent<EventsManager>();
        GenerateInteractionGroups();
    }
    
    public void OnSkipButtonPressed()
    {
        ShowNextGroup();
    }
    
    private void GenerateInteractionGroups()
    {
        List<Transform> currentGroup = new List<Transform>();
        foreach (Transform interaction in _interactions)
        {
            SecondaryEvent secEvent = interaction.GetComponent<SecondaryEvent>();

            if (secEvent.Separate)
            {
                if (currentGroup.Count > 0)
                {
                    _interactionGroups.Add(new List<Transform>(currentGroup));
                    currentGroup.Clear();
                }
                _interactionGroups.Add(new List<Transform> { interaction });
            }
            else
            {
                if (currentGroup.Count < 3)
                {
                    currentGroup.Add(interaction);
                }
                else
                {
                    _interactionGroups.Add(new List<Transform>(currentGroup));
                    currentGroup.Clear();
                    currentGroup.Add(interaction);
                }
            }
        }

        if (currentGroup.Count > 0)
        {
            _interactionGroups.Add(currentGroup);
        }
    }

    private void ShowNextGroup()
    {
        if (_currentGroupIndex < _interactionGroups.Count)
        {
            if (_currentGroupIndex > 0)
            {
                DisableGroup(_interactionGroups[_currentGroupIndex - 1]);
            }

            VisualizeGroup(_interactionGroups[_currentGroupIndex]);
            _currentGroupIndex++;
        }
        else
        {
            if (_interactionGroups.Count == 0)
            {
                return;
            }
            
            Debug.Log("Group skipped.");
            Clear();
            PlayEvent();
        }
    }

    private void VisualizeGroup(List<Transform> group)
    {
        float animationDelay = 0.5f;

        foreach (Transform interaction in group)
        {
            SecondaryEvent secondaryEvent = interaction.gameObject.GetComponent<SecondaryEvent>();

            interaction.gameObject.SetActive(true);

            if (secondaryEvent is IAnimatable animatableSecondaryEvent)
            {
                animatableSecondaryEvent.ExecuteAnimation(animationDelay);
                animationDelay += 0.5f;
            }
        }

        if (_clearCoroutine == null)
        {
            _clearCoroutine = StartCoroutine(SwitchGroupAfterDelay(animationDelay + 2f));
        }
        else
        {
            StopCoroutine(_clearCoroutine);
            _clearCoroutine = null;
            _clearCoroutine = StartCoroutine(SwitchGroupAfterDelay(animationDelay + 2f));
        }
    }

    private void DisableGroup(List<Transform> group)
    {
        foreach (Transform interaction in group)
        {
            interaction.gameObject.SetActive(false);
        }
    }

    private IEnumerator SwitchGroupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextGroup();
    }

    private void Clear()
    {
        foreach (Transform interaction in _interactions)
        {
            Destroy(interaction.gameObject);
        }

        if (_clearCoroutine != null)
        {
            StopCoroutine(_clearCoroutine);
            _clearCoroutine = null;
        }

        _interactions.Clear();
        _interactionGroups.Clear();
        _currentGroupIndex = 0;
    }

    private void PlayEvent()
    {
        _eventsManager.PlayEvent();
    }

    public void VisualizeAll()
    {
        GenerateInteractionGroups();
        ShowNextGroup();
    }

    public void VisualizeInteraction(SecondaryEvent secondaryEvent)
    {
        secondaryEvent.Initialize(_interactions);
    }

    public void VisualizeAddableCharacteristic(Characteristic characteristic)
    {
        CharacteristicInteractionSetup prefab = CharacteristicInteractionPrefab;
        SecondaryEvent secondaryEvent = InstantiateEvent(prefab);
        CharacteristicInteractionSetup characteristicInteraction = secondaryEvent as CharacteristicInteractionSetup;

        characteristicInteraction.SetCharacteristic(characteristic);
        VisualizeInteraction(secondaryEvent);
    }

    public void VisualizeCharacterRelationshipInteraction(Character targetCharacter, int modificationAmount)
    {
        RelationshipInteractionSetup prefab = RelationshipInteractionPrefab;
        SecondaryEvent secondaryEvent = InstantiateEvent(prefab);
        RelationshipInteractionSetup relationshipInteraction = secondaryEvent as RelationshipInteractionSetup;

        relationshipInteraction.SetCharacter(targetCharacter);
        relationshipInteraction.SetTargetRelationshipValue(targetCharacter.Relationship + modificationAmount);
        VisualizeInteraction(secondaryEvent);
    }
    
    public SecondaryEvent InstantiateEvent(SecondaryEvent prefab)
    {
        SecondaryEvent secondaryEvent = Instantiate(prefab, grid);
        return secondaryEvent;
    }

    public bool HasAvailableContentToVisualize()
    {
        return _interactions.Count > 0;
    }
}