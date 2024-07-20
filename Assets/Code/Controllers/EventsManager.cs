using System;
using UnityEngine;

[RequireComponent(typeof(SceneComponentsHolder))]
[RequireComponent(typeof(CardController))]
[RequireComponent(typeof(SecondaryEventsVisualizer))]
[RequireComponent(typeof(CutsceneController))]
public class EventsManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Chapter initialChapter;

    private int _currentEventID = 0;
    private Chapter _currentChapter;
    private ISkippable skippable;
    
    public SceneComponentsHolder SceneComponents { get; private set; }
    public CardController CardController { get; private set; }
    public SecondaryEventsVisualizer SecondaryEventsVisualizer { get; private set; }
    public DataPersistenceManager DataPersistenceManager { get; private set; }
    public CutsceneController CutsceneController { get; private set; }

    public static EventsManager Instance { get; private set; }

    private void Awake()
    {
        SetChapter(initialChapter);

        Instance = this;

        DataPersistenceManager = GameObject.FindAnyObjectByType<DataPersistenceManager>();

        if (DataPersistenceManager != null)
        {
            DataPersistenceManager.FindAllDataPersistenceObjects();
            DataPersistenceManager.FindAllResetableObjects();
            DataPersistenceManager.LoadGameData();
        }
    }

    private void Start()
    {
        SceneComponents = GetComponent<SceneComponentsHolder>();
        CardController = GetComponent<CardController>();
        SecondaryEventsVisualizer = GetComponent<SecondaryEventsVisualizer>();
        CutsceneController = GetComponent<CutsceneController>();

        PlayEvent();
    }

    private void Update()
    {
        if (skippable == null) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skippable.OnSkipButtonPressed();
        }
    }

    public void LoadData(GameData data)
    {
        if (data.CurrentChapter != null)
        {
            _currentChapter = data.CurrentChapter;
        }

        _currentEventID = data.CurrentEventID;
    }

    public void SaveData(ref GameData data)
    {
        data.CurrentChapter = _currentChapter;
        data.CurrentEventID = _currentEventID;
    }

    private bool AllowedToPlayEvent()
    {
        if (CutsceneController.HasCutsceneToVisualize())
        {
            CutsceneController.PlayCutsceneEvent();
            skippable = CutsceneController;
            return false;
        } 
        
        if (MinigameController.Instance.HasMinigameToVisualize())
        {
            SceneComponents.SetupDialogueCharacter(null, false, default, default);
            SceneComponents.TitleField.WriteText(String.Empty);
            
            MinigameController.Instance.Launch();
            skippable = MinigameController.Instance;
            return false;
        }
        
        if (SecondaryEventsVisualizer.HasAvailableContentToVisualize())
        {
            SecondaryEventsVisualizer.VisualizeAll();
            skippable = SecondaryEventsVisualizer;
            return false;
        }

        return true;
    }

    public void SetSkippable(ISkippable newSkippable)
    {
        skippable = newSkippable;
    }
    
    public void PlayEvent()
    {
        CardController.ClearCards();

        if (!AllowedToPlayEvent()) return;

        CardController.SpawnCards(_currentEventID);
        SceneComponents.SetupScene(_currentEventID);

        if (DataPersistenceManager != null)
            DataPersistenceManager.SaveGameData();
    }

    public void SetEventID(int id)
    {
        _currentEventID = id;
    }

    public int GetCurrentEventID()
    {
        return _currentEventID;
    }
    
    public void SetChapter(Chapter chapter)
    {
        _currentChapter = chapter;
    }

    public Chapter GetCurrentChapter()
    {
        return _currentChapter;
    }
}