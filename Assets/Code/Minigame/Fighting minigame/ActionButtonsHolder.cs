using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingMinigameLogic
{
    public class ActionButtonsHolder : MonoBehaviour
    {
        [SerializeField] private ActionButton attackButton;
        [SerializeField] private ActionButton restButton;
        [SerializeField] private ActionButton defenceButton;

        public ActionButton AttackButton => attackButton;
        public ActionButton RestButton => restButton;
        public ActionButton DefenceButton => defenceButton;

        public void ResetButtonsSkillPoints()
        {
            attackButton.ResetPoints();
            restButton.ResetPoints();
            defenceButton.ResetPoints();
        }

        public void ToggleInteractability(bool interactable)
        {
            attackButton.ToggleInteractability(interactable);
            restButton.ToggleInteractability(interactable);
            defenceButton.ToggleInteractability(interactable);
        }
    }
}