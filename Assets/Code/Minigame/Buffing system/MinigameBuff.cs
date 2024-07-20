using System;
using UnityEngine;

namespace MinigameUtilities
{
    [Serializable]
    public abstract class MinigameBuff : ScriptableObject
    {
        [Header("Initial setup")]
        [SerializeField] private Characteristic requiredCharacteristic;
        [SerializeField, TextArea(2, 4)] private string buffEffectDescription;
        [SerializeField] private string modifierSymbol = "%";

        protected string ModifierSymbol => modifierSymbol;
        public string BuffEffectDescription => buffEffectDescription;
        
        public Characteristic RequiredCharacteristic => requiredCharacteristic;

        public abstract void ApplyBuff(Minigame targetMinigame);

        public abstract string GetBuffModifierText();
    }
}