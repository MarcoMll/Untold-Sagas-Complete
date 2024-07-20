using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace FightingMinigameLogic
{
    public class FightController : MonoBehaviour
    {
        [SerializeField] private ActionButtonsHolder actionButtonsHolder;
        [SerializeField] private DynamicButton finishMoveButton;
        [SerializeField] private SkillsVisualizationController skillsVisualizationController;
        [SerializeField] private PlayerInventoryController playerInventory;

        private bool _playerMove;
        private Fighter _player;
        private Fighter _enemy;
        
        public Fighter Player => _player;

        public Action onPlayerDefeated;
        public Action onEnemyDefeated;        

        
        private void Start()
        {
            finishMoveButton.Button.onClick.AddListener(StartEnemyMove);
        }
        
        private void ToggleFinishMoveButton(int skillPoints)
        {
            if (skillPoints <= 0 && _playerMove == true)
            {
                finishMoveButton.Show(0.3f);
            } 
            else
            {
                finishMoveButton.Hide(0.3f);
            }
        }
        
        private void ToggleActionButtons(bool value)
        {
            actionButtonsHolder.ToggleInteractability(value);
            playerInventory.ToggleInteractabilityToAllSlots(value);
        }
        
        private void DistributeEnemySkills()
        {
            var skillPoints = _enemy.SkillPoints;
            var availableItemsAmount = _enemy.Inventory.Count;

            if (skillPoints > _enemy.CharacterStats.LastSkillPointsAmount)
            {
                _enemy.CharacterStats.LastSkillPointsAmount = skillPoints;
            }

            Random rand = new Random();
            var itemPoints = 0;
            if (availableItemsAmount > 0)
            {
                itemPoints = rand.Next(availableItemsAmount + 1);
            }

            var remainingPoints = skillPoints - itemPoints;
    
            int attackPoints, defendPoints, restPoints;
    
            if (remainingPoints < 3) {
                attackPoints = 1;
                defendPoints = 1;
                restPoints = remainingPoints - 2;
            } else {
                var firstSplit = rand.Next(1, remainingPoints);
                int secondSplit;
                do {
                    secondSplit = rand.Next(1, remainingPoints);
                } while (secondSplit == firstSplit);

                var lowerSplit = Math.Min(firstSplit, secondSplit);
                var higherSplit = Math.Max(firstSplit, secondSplit);

                attackPoints = lowerSplit;
                defendPoints = higherSplit - lowerSplit;
                restPoints = remainingPoints - higherSplit;
            }

            attackPoints = Math.Max(0, attackPoints);
            defendPoints = Math.Max(0, defendPoints);
            restPoints = Math.Max(0, restPoints);

            _enemy.CharacterStats.AttackSkillPoints = attackPoints;
            _enemy.CharacterStats.DefenceSkillPoints = defendPoints;
            _enemy.CharacterStats.RestSkillPoints = restPoints;
    
            _enemy.SelectRandomItemsFromInventory(itemPoints);
            _enemy.ChangeSkillPoints(-skillPoints);
        }
        
        private void MemorizePlayerSelections()
        {
            _player.CharacterStats.AttackSkillPoints = actionButtonsHolder.AttackButton.PointsCount;
            _player.CharacterStats.RestSkillPoints = actionButtonsHolder.RestButton.PointsCount;
            _player.CharacterStats.DefenceSkillPoints = actionButtonsHolder.DefenceButton.PointsCount;
            
            playerInventory.UpdatePlayerInventory();
        }

        private void TrackEnemyHealthPoints(int healthPoints)
        {
            if (healthPoints > 0) return;
            skillsVisualizationController.InterruptAnimations();
            onEnemyDefeated?.Invoke();
        }

        private void TrackPlayerHealthPoints(int healthPoints)
        {
            if (healthPoints > 0) return;
            skillsVisualizationController.InterruptAnimations();
            onPlayerDefeated?.Invoke();
        }
        
        private void StartEnemyMove()
        {
            _playerMove = false;
            finishMoveButton.Hide(0.3f);

            ToggleActionButtons(false);
            MemorizePlayerSelections();
            DistributeEnemySkills();
            CompareSkillPoints();
        }

        private void CompareSkillPoints()
        {
            Debug.Log($"Player attack: {_player.CharacterStats.AttackSkillPoints} || Enemy defence: {_enemy.CharacterStats.DefenceSkillPoints}" +
                      $"\nPlayer defence: {_player.CharacterStats.DefenceSkillPoints} || Enemy attack: {_enemy.CharacterStats.AttackSkillPoints}" +
                      $"\nPlayer rest: {_player.CharacterStats.RestSkillPoints} || Enemy rest: {_enemy.CharacterStats.RestSkillPoints}" +
                      $"\nOverall Player points: {_player.CharacterStats.LastSkillPointsAmount} || Overall Enemy points: {_enemy.CharacterStats.LastSkillPointsAmount}");
            
            skillsVisualizationController.VisualizeSkills(_player, _enemy);
        }

        private void UpdateFighterSkillPoints(Fighter targetFighter)
        {
            targetFighter.ChangeSkillPoints(targetFighter.CharacterStats.LastSkillPointsAmount + targetFighter.CharacterStats.RestSkillPoints);
            targetFighter.CharacterStats.ResetValues();
        }

        private void InitializeEnemy(Fighter newEnemy)
        {
            if (_enemy != null)
            {
                _enemy.onHealthAmountChanged -= TrackEnemyHealthPoints;
                _enemy = null;
            }

            _enemy = newEnemy;
            _enemy.onHealthAmountChanged += TrackEnemyHealthPoints;
        }

        private void InitializePlayer(Fighter newPlayer)
        {
            if (_player != null) return;
            
            _player = newPlayer;
            _player.onSkillsAmountChanged += ToggleFinishMoveButton;
            _player.onHealthAmountChanged += TrackPlayerHealthPoints;
            
            playerInventory.InitializeInventory(PlayerStats.Instance.ItemHandler.QuickSlots);
        }
        
        public void StartNewRound()
        {
            _playerMove = true;
            playerInventory.DeselectAllSlots();
            
            _player.CharacterStats.ResetValues();
            _enemy.CharacterStats.ResetValues();
            
            UpdateFighterSkillPoints(_player);
            UpdateFighterSkillPoints(_enemy);
            
            if (_player.SkillPoints > _player.CharacterStats.LastSkillPointsAmount)
            {
                _player.CharacterStats.LastSkillPointsAmount = _player.SkillPoints;
            }
            
            ToggleActionButtons(true);
        }
        
        public void StartFight(Fighter player, Fighter enemy)
        {
            InitializePlayer(player);
            InitializeEnemy(enemy);
            StartNewRound();
        }
    }
}