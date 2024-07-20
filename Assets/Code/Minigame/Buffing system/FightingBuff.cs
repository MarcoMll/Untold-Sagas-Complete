using System;
using UnityEngine;

namespace MinigameUtilities
{
    [CreateAssetMenu(fileName = "New Fighting Buff", menuName = "Custom/Game Event Interactions/New Fighting buff")]
    public class FightingBuff : MinigameBuff
    {
        [Header("Fighting Buff setup")]
        [SerializeField] private int healthPointsModifier;
        [SerializeField] private int skillPointsModifier;

        public int HealthPointsModifier => healthPointsModifier;
        public int SkillPointsModifier => skillPointsModifier;
        
        public override void ApplyBuff(Minigame targetMinigame)
        {
            var fightingMinigame = targetMinigame as FightingMinigame;
            if (fightingMinigame == null)
            {
                Debug.LogError("THe given minigame is not a FightingMinigame!");
                return;
            } 
            if (PlayerStats.Instance.HasCharacteristic(base.RequiredCharacteristic) == false)
            {
                return;
            }
            
            //change player health
            //change player skills
            //reinitialize player
        }

        public override string GetBuffModifierText()
        {
            var modifier = healthPointsModifier == 0 ? skillPointsModifier : healthPointsModifier;
            var numberSign = modifier > 0 ? "+" : "-";
            return $"{numberSign}{modifier} {base.ModifierSymbol}";
        }
    }
}