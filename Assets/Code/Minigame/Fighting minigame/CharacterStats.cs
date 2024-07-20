using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace FightingMinigameLogic
{
    [Serializable]
    public class CharacterStats
    {
        [FormerlySerializedAs("AttackSkills")] public int AttackSkillPoints;
        [FormerlySerializedAs("ActionSkills")] public int RestSkillPoints;
        [FormerlySerializedAs("DefenceSkills")] public int DefenceSkillPoints;

        public int LastSkillPointsAmount;

        public void ResetValues()
        {
            AttackSkillPoints = 0;
            RestSkillPoints = 0;
            DefenceSkillPoints = 0;
        }
    }
}