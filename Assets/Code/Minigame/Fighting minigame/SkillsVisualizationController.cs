using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace FightingMinigameLogic
{
    public class SkillsVisualizationController : MonoBehaviour
    {
        [Header("General setup")]
        [SerializeField] private ActionButtonsHolder actionButtonsHolder;
        [SerializeField] private FightController fightController;
        [SerializeField] private SkillVisualizer skillPrefab;
        [SerializeField] private ItemBonusVisualizer playerItemBonusVisualizer;
        [SerializeField] private ItemBonusVisualizer enemyItemBonusVisualizer;
        
        [Header("Spawn points")]
        [SerializeField] private Transform playerSide;
        [SerializeField] private Transform enemySide;
        [SerializeField] private Transform center;
        
        [Header("UI")]
        [SerializeField] private Sprite attackSkillIcon;
        [SerializeField] private Sprite defenceSkillIcon;
        [SerializeField] private Sprite restSkillIcon;
        [Header("Audio")]
        [SerializeField] private AudioClip[] attackSkillSoundsList;
        [SerializeField] private AudioClip[] defenceSkillSoundsList;
        [SerializeField] private AudioClip[] restSkillSoundsList;
        [SerializeField] private AudioClip[] hitSoundsList;
        [Header("Timings settings")]
        [SerializeField] private float skillAnimationDuration = 0.2f;
        [SerializeField] private float delayBetweenStages = 0.35f;
        [SerializeField] private float endCoroutineDelay = 1f;

        private enum StageType
        {
            Buffing, Attack, Defence, Rest
        }
        
        private CoroutineQueue _coroutineQueue;
        
        private void Awake()
        {
            _coroutineQueue = new CoroutineQueue(this);
            
            _coroutineQueue.onQueueEmpty += actionButtonsHolder.ResetButtonsSkillPoints;
            _coroutineQueue.onQueueEmpty += fightController.StartNewRound;
        }

        private SkillVisualizer InstantiateSkill(Sprite skillIcon, Transform side)
        {
            SkillVisualizer skill = Instantiate(skillPrefab, side);
            skill.SetSkillIcon(skillIcon);
            skill.UpdateCount(0);
            return skill;
        }

        private IEnumerator SmoothlyChangeSkillAmount(SkillVisualizer targetSkill, ActionButton targetButton, int targetSkillPoints, AudioClip[] soundsList)
        {
            var skillPoints = 0;
            
            while (skillPoints < targetSkillPoints)
            {
                skillPoints++;
                
                targetSkill.UpdateCount(skillPoints);
                targetButton.DestroyLastAddedSkillPoint();
                SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(soundsList));
                yield return new WaitForSeconds(.35f);
            }
        }

        private IEnumerator ShowItemBonuses(ItemBonusVisualizer bonusVisualizer, SkillVisualizer targetSkill, Fighter targetFighter, StageType stageType)
        {
            if (targetFighter.SelectedItems.Count == 0)
            {
                yield break;
            }
            
            var suitableItems = new List<Item>();
            var newSkillPoints = stageType switch
            {
                StageType.Buffing => 0,
                StageType.Attack => targetFighter.CharacterStats.AttackSkillPoints,
                StageType.Defence => targetFighter.CharacterStats.DefenceSkillPoints,
                StageType.Rest => targetFighter.CharacterStats.RestSkillPoints,
                _ => 0
            };

            foreach (var selectedItem in targetFighter.SelectedItems)
            {
                switch (stageType)
                {
                    case StageType.Buffing:
                        if (selectedItem.HealthEffect > 0) suitableItems.Add(selectedItem);
                        break;
                    case StageType.Attack:
                        if (selectedItem.AttackEffect > 0) suitableItems.Add(selectedItem);
                        break;
                    case StageType.Defence:
                        if (selectedItem.DefenceEffect > 0) suitableItems.Add(selectedItem);
                        break;
                    case StageType.Rest:
                        if (selectedItem.SkillEffect > 0) suitableItems.Add(selectedItem);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(stageType), stageType, null);
                }
            }

            foreach (var item in suitableItems)
            {
                bonusVisualizer.Setup(item);
                bonusVisualizer.Show(skillAnimationDuration);

                newSkillPoints += stageType switch
                {
                    StageType.Buffing => item.HealthEffect,
                    StageType.Attack => item.AttackEffect,
                    StageType.Defence => item.DefenceEffect,
                    StageType.Rest => item.SkillEffect,
                    _ => throw new ArgumentOutOfRangeException(nameof(stageType), stageType, null)
                };

                if (targetSkill!= null) targetSkill.UpdateCount(newSkillPoints);
                SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(attackSkillSoundsList));
                yield return new WaitForSeconds(endCoroutineDelay); 
                
                bonusVisualizer.Hide(skillAnimationDuration);
                yield return new WaitForSeconds(skillAnimationDuration + delayBetweenStages);
            }

            switch (stageType)
            {
                case StageType.Buffing:
                    targetFighter.ChangeHealthPoints(newSkillPoints);
                    break;
                case StageType.Attack:
                    targetFighter.CharacterStats.AttackSkillPoints = newSkillPoints;
                    break;
                case StageType.Defence:
                    targetFighter.CharacterStats.DefenceSkillPoints = newSkillPoints;
                    break;
                case StageType.Rest:
                    targetFighter.CharacterStats.RestSkillPoints = newSkillPoints;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stageType), stageType, null);
            }
        }
        
        private IEnumerator PlayerAttackVisualizationCoroutine(Fighter player, Fighter enemy)
        {
            yield return ShowItemBonuses(playerItemBonusVisualizer, null, player, StageType.Buffing);
            yield return ShowItemBonuses(enemyItemBonusVisualizer, null, enemy, StageType.Buffing);

            SkillVisualizer attackSkill = InstantiateSkill(attackSkillIcon, playerSide);
            attackSkill.ToggleRightArrow(true);
            attackSkill.Show(skillAnimationDuration);
            
            SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(attackSkillSoundsList));
            
            yield return new WaitForSeconds(skillAnimationDuration + delayBetweenStages);

            StartCoroutine(SmoothlyChangeSkillAmount(attackSkill, actionButtonsHolder.AttackButton,
                player.CharacterStats.AttackSkillPoints, attackSkillSoundsList));
                
            yield return new WaitForSeconds(player.CharacterStats.AttackSkillPoints * .35f + delayBetweenStages);
            yield return ShowItemBonuses(playerItemBonusVisualizer, attackSkill, player, StageType.Attack);

            SkillVisualizer defenceSkill = InstantiateSkill(defenceSkillIcon, enemySide);
            defenceSkill.UpdateCount(enemy.CharacterStats.DefenceSkillPoints);
            defenceSkill.ToggleLeftArrow(true);
            defenceSkill.Show(skillAnimationDuration);
            
            SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(defenceSkillSoundsList));
            
            yield return new WaitForSeconds(skillAnimationDuration + delayBetweenStages);
            yield return ShowItemBonuses(enemyItemBonusVisualizer, defenceSkill, enemy, StageType.Defence);
            
            MoveSkillsToCenter(attackSkill, defenceSkill);
            attackSkill.ToggleRightArrow(false);
            defenceSkill.ToggleLeftArrow(false);
            
            var pointsDifference = CompareSkillPoints(player, enemy, attackSkill, defenceSkill);
            var absPoints = Mathf.Abs(pointsDifference);
            yield return new WaitForSeconds(skillAnimationDuration);
            
            attackSkill.UpdateCount(absPoints);
            defenceSkill.UpdateCount(absPoints);
            
            yield return new WaitForSeconds(delayBetweenStages);

            attackSkill.Hide(skillAnimationDuration);
            defenceSkill.Hide(skillAnimationDuration);

            if (pointsDifference > 0)
            {
                enemy.ChangeHealthPoints(-pointsDifference);
                SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(hitSoundsList));
            }
            
            yield return new WaitForSeconds(skillAnimationDuration + endCoroutineDelay);
        }

        private IEnumerator EnemyAttackVisualizationCoroutine(Fighter player, Fighter enemy)
        {
            SkillVisualizer attackSkill = InstantiateSkill(attackSkillIcon, enemySide);
            attackSkill.UpdateCount(enemy.CharacterStats.AttackSkillPoints);
            attackSkill.ToggleLeftArrow(true);
            attackSkill.Show(skillAnimationDuration);
            
            SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(attackSkillSoundsList));
            
            yield return new WaitForSeconds(skillAnimationDuration + delayBetweenStages);
            yield return ShowItemBonuses(enemyItemBonusVisualizer, attackSkill, enemy, StageType.Attack);
            
            SkillVisualizer defenceSkill = InstantiateSkill(defenceSkillIcon, playerSide);
            defenceSkill.ToggleRightArrow(true);
            defenceSkill.Show(skillAnimationDuration);
            
            SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(defenceSkillSoundsList));
            
            yield return new WaitForSeconds(skillAnimationDuration + delayBetweenStages);
            StartCoroutine(SmoothlyChangeSkillAmount(defenceSkill, actionButtonsHolder.DefenceButton, player.CharacterStats.DefenceSkillPoints, defenceSkillSoundsList));
            
            yield return new WaitForSeconds(player.CharacterStats.DefenceSkillPoints * .35f + delayBetweenStages);
            yield return ShowItemBonuses(playerItemBonusVisualizer, defenceSkill, player, StageType.Defence);
            
            MoveSkillsToCenter(attackSkill, defenceSkill);
            attackSkill.ToggleLeftArrow(false);
            defenceSkill.ToggleRightArrow(false);
            
            var pointsDifference = CompareSkillPoints(enemy, player, attackSkill, defenceSkill);
            var absPoints = Mathf.Abs(pointsDifference);
            yield return new WaitForSeconds(skillAnimationDuration);
            
            attackSkill.UpdateCount(absPoints);
            defenceSkill.UpdateCount(absPoints);
            
            yield return new WaitForSeconds(delayBetweenStages);

            attackSkill.Hide(skillAnimationDuration);
            defenceSkill.Hide(skillAnimationDuration);

            if (pointsDifference > 0)
            {
                player.ChangeHealthPoints(-pointsDifference);
                SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(hitSoundsList));
            }
            
            yield return new WaitForSeconds(skillAnimationDuration + endCoroutineDelay);
        }

        private IEnumerator PlayerRestVisualizationCoroutine(Fighter player)
        {
            SkillVisualizer restSkill = InstantiateSkill(restSkillIcon, playerSide);
            restSkill.Show(skillAnimationDuration);
            
            SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(restSkillSoundsList));
            
            yield return new WaitForSeconds(skillAnimationDuration + delayBetweenStages);
            StartCoroutine(SmoothlyChangeSkillAmount(restSkill, actionButtonsHolder.RestButton, player.CharacterStats.RestSkillPoints, restSkillSoundsList));

            yield return new WaitForSeconds(player.CharacterStats.RestSkillPoints * .35f);
            yield return ShowItemBonuses(playerItemBonusVisualizer, restSkill, player, StageType.Rest);
            yield return new WaitForSeconds(delayBetweenStages);

            player.ChangeSkillPoints(player.CharacterStats.RestSkillPoints);
            restSkill.Hide(skillAnimationDuration);
            
            yield return new WaitForSeconds(skillAnimationDuration + endCoroutineDelay);
        }
        
        private IEnumerator EnemyRestVisualizationCoroutine(Fighter enemy)
        {
            SkillVisualizer restSkill = InstantiateSkill(restSkillIcon, enemySide);
            restSkill.UpdateCount(enemy.CharacterStats.RestSkillPoints);
            restSkill.Show(skillAnimationDuration);
  
            SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(restSkillSoundsList));
            
            yield return new WaitForSeconds(skillAnimationDuration);
            yield return ShowItemBonuses(enemyItemBonusVisualizer, restSkill, enemy, StageType.Rest);
            yield return new WaitForSeconds(delayBetweenStages);

            enemy.ChangeSkillPoints(enemy.CharacterStats.RestSkillPoints);
            restSkill.Hide(skillAnimationDuration);
            
            yield return new WaitForSeconds(skillAnimationDuration + endCoroutineDelay);
        }
        
        private int CompareSkillPoints(Fighter attacker, Fighter defender, SkillVisualizer attackSkill, SkillVisualizer defenceSkill)
        {
            var pointsDifference = attacker.CharacterStats.AttackSkillPoints - defender.CharacterStats.DefenceSkillPoints;
            
            attackSkill.transform.SetParent(center);
            defenceSkill.transform.SetParent(center);
            
            if (pointsDifference > 0)
            {
                defenceSkill.transform.SetAsFirstSibling();
            }
            else
            {
                attackSkill.transform.SetAsFirstSibling();
            }

            return pointsDifference;
        }

        private void MoveSkillsToCenter(SkillVisualizer skill1, SkillVisualizer skill2)
        {
            var centerPosition = center.position;
            skill1.Move(centerPosition, skillAnimationDuration);
            skill2.Move(centerPosition, skillAnimationDuration);
        }
        
        public void VisualizeSkills(Fighter player, Fighter enemy)
        { 
            _coroutineQueue.ClearQueue();
            
            _coroutineQueue.EnqueueCoroutine(PlayerAttackVisualizationCoroutine(player, enemy));
            _coroutineQueue.EnqueueCoroutine(EnemyAttackVisualizationCoroutine(player, enemy));
            _coroutineQueue.EnqueueCoroutine(PlayerRestVisualizationCoroutine(player));
            _coroutineQueue.EnqueueCoroutine(EnemyRestVisualizationCoroutine(enemy));
            _coroutineQueue.StartQueue();
        }

        public void InterruptAnimations()
        {
            _coroutineQueue.StopCurrentCoroutine();

            var restSkillPoints = fightController.Player.CharacterStats.RestSkillPoints;
            if (restSkillPoints > 0)
            {
                fightController.Player.ChangeSkillPoints(restSkillPoints);
            }
            
            actionButtonsHolder.ResetButtonsSkillPoints();
        }
    }
}