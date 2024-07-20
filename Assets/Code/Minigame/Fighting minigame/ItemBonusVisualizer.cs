using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FightingMinigameLogic
{
    [RequireComponent(typeof(ObjectMovementController))]   
    [RequireComponent(typeof(CanvasGroupController))]
    public class ItemBonusVisualizer : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemNameField;
        [SerializeField] private TMP_Text skillAmountField;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image skillIcon;
        [SerializeField] private Sprite attackIcon, defenceIcon, healthIcon;
        
        private ObjectMovementController _movementController;
        private CanvasGroupController _canvasGroupController;

        private void Awake()
        {
            _movementController = GetComponent<ObjectMovementController>();
            _canvasGroupController = GetComponent<CanvasGroupController>();
        }

        public void Show(float animationDuration)
        {
            _movementController.MoveToInitial(animationDuration);
            _canvasGroupController.SmoothlyChangeAlpha(1f, animationDuration);
        }
        
        public void Hide(float animationDuration)
        {
            _movementController.MoveToTarget(animationDuration);
            _canvasGroupController.SmoothlyChangeAlpha(0f, animationDuration);
        }
        
        public void Setup(Item item)
        {
            itemNameField.text = item.Name;
            itemIcon.sprite = item.Icon;

            var skillPoints = 0;
            
            if (item.HealthEffect > 0)
            {
                skillPoints = item.HealthEffect;
                skillIcon.sprite = healthIcon;
            } 
            else if (item.AttackEffect > 0)
            {
                skillPoints = item.AttackEffect;
                skillIcon.sprite = attackIcon;
            } 
            else if (item.DefenceEffect > 0)
            {
                skillPoints = item.DefenceEffect;
                skillIcon.sprite = defenceIcon;
            }
            
            skillAmountField.text = skillPoints.ToString();
        }
    }
}