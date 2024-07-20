using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour, ISkippable
{
    [SerializeField] private Transform minigameSection;
    [SerializeField] private MinigameNotification minigameNotificationPrefab;
    [SerializeField] private Color passIconColor, lossIconColor;
    [SerializeField] private Color passTextColor, lossTextColor;
    
    private Minigame _currentMinigame;
    private Minigame _minigamePrefab;
    private MinigameNotification _endgameNotification;
    
    private int _eventIdOnLoss, _eventIdOnPass;
    private int _targetEventID = 0;
    
    private Coroutine _resumeGameCoroutine;
    
    public static MinigameController Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnSkipButtonPressed()
    {
        if (_endgameNotification == null) return;
        ReturnToMainGame();
    }
    
    private IEnumerator ContinueGameAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        ReturnToMainGame();
    }
    
    private void ReturnToMainGame()
    {
        UserInterfaceController.Instance.ShowAll();
        EventsManager.Instance.SetEventID(_targetEventID);
        
        Clear();
        EventsManager.Instance.PlayEvent();
    }
    
    private void Clear()
    {
        if (_endgameNotification != null)
        {
            Destroy(_endgameNotification.gameObject);
            _endgameNotification = null;
        }

        if (_currentMinigame != null)
        {
            Destroy(_currentMinigame.gameObject);
            _currentMinigame = null;
        }

        if (_resumeGameCoroutine != null)
        {
            StopCoroutine(_resumeGameCoroutine);
            _resumeGameCoroutine = null;
        }

        _minigamePrefab = null;
        _targetEventID = 0;
        _eventIdOnPass = 0;
        _eventIdOnLoss = 0;
    }

    private void ShowTutorialIfNeeded()
    {
        if (_currentMinigame == null)
        {
            Debug.LogError("Can't launch the minigame since the passed minigame is null!");
            return;
        }

        switch (_currentMinigame)
        {
            case FightingMinigame:
                if (TutorialsManager.Instance.FightingMinigameTutorialShown == true)
                {
                    _currentMinigame.StartGame();
                }
                else
                {
                    TutorialsManager.Instance.ShowFightingTutorial(_currentMinigame.StartGame);
                }
                break;
            
            case CookingMinigame:
                _currentMinigame.StartGame();
                break;
        }
    }
    
    public void SetupMinigame(Minigame minigame, int passEventID, int loseEventID)
    {
        Clear();

        _minigamePrefab = minigame;
        _eventIdOnPass = passEventID;
        _eventIdOnLoss = loseEventID;
    }
    
    public void Launch()
    {
        _currentMinigame = Instantiate(_minigamePrefab, minigameSection);
        ShowTutorialIfNeeded();
        
        UserInterfaceController.Instance.HideAll();
    }

    public void CloseMinigame(bool passed, bool hasReward = false)
    {
        if (_endgameNotification != null)
        {
            Destroy(_endgameNotification.gameObject);
        }
        _endgameNotification = Instantiate(minigameNotificationPrefab, minigameSection);
        
        if (passed == true)
        {
            if (hasReward == true)
            {
                _endgameNotification.Setup("Критический успех!", passTextColor, passIconColor);
                _endgameNotification.SetCharacteristicReward(_currentMinigame.RewardCharacteristic.Name,
                    _currentMinigame.RewardCharacteristic.Icon);
            }
            else
            {
                _endgameNotification.Setup("Успех!", passTextColor, passIconColor);
            }
        }
        else
        {
            _endgameNotification.Setup("Провал!", lossTextColor, lossIconColor);
        }
        
        if (_currentMinigame != null)
        {
            Destroy(_currentMinigame.gameObject);
            _currentMinigame = null;
        }
        
        _targetEventID = passed ? _eventIdOnPass : _eventIdOnLoss;
        
        _resumeGameCoroutine = StartCoroutine(ContinueGameAfterDelay());
        _endgameNotification.Initialize();
    }
    
    public bool HasMinigameToVisualize()
    {
        return _minigamePrefab != null;
    }
}