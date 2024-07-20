using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField] private string fileName = "";

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistences;
    private List<IResetable> _resetables;

    private FileDataHandler _dataHandler;

    public TextAsset LocalizationFile { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        CreateFileDataHandler();
        FindAllDataPersistenceObjects();
        FindAllResetableObjects();
    }

    public void FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        _dataPersistences = new List<IDataPersistence>(dataPersistences);
    }

    public void FindAllResetableObjects()
    {
        IEnumerable<IResetable> resetables = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>();
        _resetables = new List<IResetable>(resetables);

    }

    private void CreateFileDataHandler()
    {
        _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    public void CreateNewGameData()
    {
        _gameData = new GameData();
        SaveGameData();
    }

    public void SaveGameData()
    {
        foreach (IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.SaveData(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    public void LoadGameData()
    {
        _gameData = _dataHandler.Load();

        if (_gameData == null)
        {
            CreateNewGameData();
        }

        foreach (IResetable resetableData in _resetables)
        {
            resetableData.Reset();
        }

        foreach (IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.LoadData(_gameData);
        }
    }
    
    public void SetLocalizationFile(TextAsset file)
    {
        LocalizationFile = file;
    }
}
