using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;

namespace CookingMinigameLogic
{
    [RequireComponent(typeof(ProgressBar))]
    public class CompletenessBar : MonoBehaviour
    {
        [SerializeField] private CompletenessBarIndicator passIndicator;
        [SerializeField] private CompletenessBarIndicator rewardIndicator;
        [SerializeField] private float progressBarValueChangeDuration = 0.3f;

        private float _passValue = 0f;
        private float _rewardValue = 0f;
        private float _currentValue = 0f;
        
        private Vector2 _completenessBarSize;
        private RectTransform _rectTransform;
        private ProgressBar _progressBar;

        private void Awake()
        {
            _progressBar = GetComponent<ProgressBar>();
            _rectTransform = GetComponent<RectTransform>();
            _completenessBarSize = _rectTransform.sizeDelta;
        }

        private float ConvertToIndicatorHorizontalPosition(float indicatorValue)
        {
            return MathUtility.ChangeValueRange(indicatorValue, new Vector2(0f, 1f), new Vector2(-_completenessBarSize.x/2, _completenessBarSize.x/2));
        }
        
        private void UpdateIndicators()
        {
            if (_currentValue >= _passValue)
            {
                passIndicator.SetPassState();
            }
            else
            {
                passIndicator.SetIdleState();
            }
            
            passIndicator.MovePoint(ConvertToIndicatorHorizontalPosition(_passValue));
            
            if (_currentValue >= _rewardValue)
            {
                rewardIndicator.SetPassState();
            }
            else
            {
                rewardIndicator.SetIdleState();
            }
            
            Debug.Log($"Reward pos: {ConvertToIndicatorHorizontalPosition(_rewardValue)}");
            rewardIndicator.MovePoint(ConvertToIndicatorHorizontalPosition(_rewardValue));
        }

        private void SetPassIndicatorValue(float newPassValue)
        {
            _passValue = MathUtility.NormalizeValue(newPassValue, 0f, 100f);
        }

        private void SetRewardIndicatorValue(float newRewardValue)
        {
            _rewardValue = MathUtility.NormalizeValue(newRewardValue, 0f, 100f);
        }
        
        public void UpdateBarValue(float newValue, float passValue, float rewardValue)
        {
            var normalizedSliderValue = MathUtility.NormalizeValue(newValue, 0f, 100f);
            
            _progressBar.SetTargetValue(normalizedSliderValue, progressBarValueChangeDuration);
            _currentValue = normalizedSliderValue;
            
            SetPassIndicatorValue(passValue);
            SetRewardIndicatorValue(rewardValue);
            UpdateIndicators();
        }
    }
}