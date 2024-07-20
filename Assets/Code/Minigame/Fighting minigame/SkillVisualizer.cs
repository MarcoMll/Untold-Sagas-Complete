using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace FightingMinigameLogic
{
    [RequireComponent(typeof(ObjectMover))]
    [RequireComponent(typeof(ScaleController))]
    public class SkillVisualizer : MonoBehaviour
    {
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Image skillIcon;
        [SerializeField] private ImageVisualizer leftArrow, rightArrow;
        
        private int _skillCount = 0;
        private int _maxValue = 0;
        
        private ObjectMover _mover;
        private ScaleController _scaleController;

        private void Awake()
        {
            _mover = GetComponent<ObjectMover>();
            _scaleController = GetComponent<ScaleController>();
        }

        public void UpdateCount(int newCount)
        {
            countText.text = newCount.ToString();
        }

        public void SetSkillIcon(Sprite sprite)
        {
            skillIcon.sprite = sprite;
        }
        
        public void Move(Vector3 targetPosition, float duration)
        {
            _mover.Move(targetPosition, duration);
        }

        public void Show(float duration)
        {
            _scaleController.ChangeScale(new Vector3(1f, 1f, 1f), duration);
        }

        public void Hide(float duration)
        {
            _scaleController.ChangeScale(Vector3.zero, duration);
        }

        public void ToggleLeftArrow(bool active)
        {
            if (active == true)
            {
                leftArrow.Show();
            }
            else
            {
                leftArrow.Hide();
            }
        }
        
        public void ToggleRightArrow(bool active)
        {
            if (active == true)
            {
                rightArrow.Show();
            }
            else
            {
                rightArrow.Hide();
            }
        }
    }
}