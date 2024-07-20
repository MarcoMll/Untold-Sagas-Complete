using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneComponentsHolder : MonoBehaviour, IDataPersistence
{
    [Header("Scene")]
    [SerializeField] private DynamicImage background;
    [SerializeField] private TextVisualizer title;
    [SerializeField] private DialogueCharacterSetup fullHeightDialogueCharacter;
    [SerializeField] private DialogueCharacterSetup simplifiedDialogueCharacter;
    [Header("Audio")] 
    [SerializeField] private AudioController musicAudioController;
    [SerializeField] private AudioController ambienceAudioController;
    [SerializeField] private AudioController sfxAudioController;

    private EventsManager _eventsManager;

    public static SceneComponentsHolder Instance { get; private set; }
    
    public TextVisualizer TitleField => title;

    private void Awake()
    {
        _eventsManager = GetComponent<EventsManager>();
        Instance = this;
    }

    public void LoadData(GameData data)
    {
        if (data.MusicClip != null)
        {
            SetSong(data.MusicClip);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.MusicClip = musicAudioController.Clip;
    }
    
    private void SetupEventTitle(Chapter chapter, int currentEventID)
    {
        string key = LocalizationKeysHolder.GetEventTitleKey(chapter, currentEventID);
        string localizedTitle = LocalizationManager.Instance.GetTranslation(key);

        TitleField.WriteText(localizedTitle);
    }

    public void SetupScene(int currentEventID)
    {
        Chapter currentChapter = _eventsManager.GetCurrentChapter();
        GameEvent currentEvent = currentChapter.GetEvent(currentEventID);

        SetupDialogueCharacter(currentEvent.DialogueCharacter, currentEvent.Dialogue, currentEvent.CharacterEmotion, currentEvent.SpriteColor, false);
        SetBackground(currentEvent.Background);

        if (currentEvent.ChangeMusicStyle)
            musicAudioController.PlayAudioClip(currentEvent.MusicClip);

        SetupEventTitle(currentChapter, currentEventID);
        SetAmbience(currentEvent.Ambience);
    }

    public void SetBackground(Sprite image)
    {
        background.SmoothlyChangeImage(image, Color.white);
    }

    public void SetupDialogueCharacter(Character character, bool dialogue, Emotion characterEmotion, Color spriteColor , bool simplified = true)
    {
        if (dialogue == false)
        {
            fullHeightDialogueCharacter.ToggleDialogueVisibility(false);
            simplifiedDialogueCharacter.ToggleDialogueVisibility(false);
            return;
        }

        character.Familiar = true;

        if (simplified == true)
        {
            simplifiedDialogueCharacter.SetupCharacter(character, default, Color.white);
            simplifiedDialogueCharacter.ToggleDialogueVisibility(true);
            fullHeightDialogueCharacter.ToggleDialogueVisibility(false);
        }
        else
        {
            fullHeightDialogueCharacter.SetupCharacter(character, characterEmotion, spriteColor);
            fullHeightDialogueCharacter.ToggleDialogueVisibility(true);
            simplifiedDialogueCharacter.ToggleDialogueVisibility(false);
        }
    }

    public void SetSong(AudioClip songClip)
    {
        musicAudioController.PlayAudioClip(songClip);
    }
    
    public void SetAmbience(AudioClip ambienceClip)
    {
        if (ambienceAudioController.Clip == ambienceClip) return;
        ambienceAudioController.PlayAudioClip(ambienceClip);
    }

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        sfxAudioController.OverlapAudioClip(soundEffect);
    }
}