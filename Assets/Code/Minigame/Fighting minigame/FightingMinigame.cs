using System.Linq;
using FightingMinigameLogic;
using UnityEngine;
using CustomUtilities;
using MinigameUtilities;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(FightController))]
[RequireComponent(typeof(MinigameModifierController))]
public class FightingMinigame : Minigame
{
    [SerializeField] private FightersList fightersList;
    [SerializeField] private EnemyDefeatWindow enemyDefeatWindow;
    [SerializeField] private StatsVisualizer playerStatsVisualizer;
    [SerializeField] private StatsVisualizer enemyStatsVisualizer;
    [SerializeField] private Image enemyCharacterImage;
    [SerializeField] private TMP_Text enemyCharacterNameField;
    [SerializeField] private Fighter[] enemyFighters;
    [SerializeField] private bool lethalFight;
    
    private int _currentEnemyIndex = 0;
    
    private Fighter _player;
    private Fighter _enemy;

    private FightController _fightController;
    private MinigameModifierController _minigameModifierController;
    
    private void Awake()
    {
        _fightController = GetComponent<FightController>();
        _minigameModifierController = GetComponent<MinigameModifierController>();
    }

    public override void StartGame()
    {
        if (_minigameModifierController.AllBuffsApplied == false)
        {
            _minigameModifierController.StartApplyingBuffs(this);
            return;
        }
        
        _fightController.onEnemyDefeated += EndFight;
        _fightController.onPlayerDefeated += EndGame;
        enemyDefeatWindow.onWindowHidden += StartFight;
        
        fightersList.VisualizeFighters(enemyFighters.ToList());
        fightersList.SelectSlot(_currentEnemyIndex);
        
        InitializePlayer();
        InitializeEnemy(GetEnemyFighterByIndex(_currentEnemyIndex));
        StartFight();
    }

    public override void EndGame()
    {
        MinigameController.Instance.CloseMinigame(_player.HealthPoints > 0, Reward(_player.HealthPoints, 1));
        
        if (lethalFight == true) return;
        
        var initialHeartsAmount = PlayerStats.Instance.Hearts;
        
        if (_player.HealthPoints < 0)
            PlayerStats.Instance.ChangeHeartsAmount(-initialHeartsAmount + 1);
    }

    private void InitializePlayer()
    {
        var playerHealth = PlayerStats.Instance.Hearts;
        var playerSkills = 2;
        
        _player = new Fighter(null, playerHealth, playerSkills);
        _player.Initialize();
        
        playerStatsVisualizer.Initialize(_player);
    }

    private void InitializeEnemy(Fighter enemyFighter)
    {
        _enemy = enemyFighter;
        
        enemyFighter.Initialize();
        enemyStatsVisualizer.Initialize(enemyFighter);
        
        enemyCharacterImage.sprite = enemyFighter.Character.Idle;
        enemyCharacterNameField.text = enemyFighter.Character.Name;
    }

    private void StartFight()
    {
        if (_player == null || _enemy == null)
        {
            TimeUtility.CallWithDelay(3f, EndGame);
            return;
        }
        
        _fightController.StartFight(_player, _enemy);
    }

    private void EndFight()
    {
        enemyDefeatWindow.ShowWindow();

        _currentEnemyIndex++;
        Fighter newEnemy = GetEnemyFighterByIndex(_currentEnemyIndex);

        if (newEnemy == null)
        {
            TimeUtility.CallWithDelay(3f, EndGame);
            return;
        }
        InitializeEnemy(newEnemy);
    }
    
    private Fighter GetEnemyFighterByIndex(int index)
    {
        if (index >= enemyFighters.Length)
        {
            Debug.Log("The passed index exceeds the number of available fighters in the list! An empty fighter has been returned.");
            return null;
        }
        
        return enemyFighters[index];
    }
}