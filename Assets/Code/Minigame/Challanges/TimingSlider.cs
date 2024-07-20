using System;
using System.Collections;
using CustomInspector;
using CustomUtilities;
using UnityEngine;

public class TimingSlider : Challenge
{
    [HorizontalLine("Setup")]
    [SerializeField] private RectTransform sliderRectTransform;
    [SerializeField] private RectTransform handleRectTransform;
    [HorizontalLine("UI")]
    [SerializeField] private SliderFillController fill;

    private int _winningChance = 0;
    private Vector2 _permissibleValuesRange = Vector2.zero;
    
    private float _interpolationSpeed = 100f;
    private float _currentValue = 0f;
    private float _minimumValue = 0f;
    private float _maximumValue = 100f;

    private bool _interactable = false;

    public float Value => _currentValue;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _interactable == true)
        {
            Stop();
        }
    }

    private IEnumerator InterpolateCurrentValue()
    {
        _currentValue = _maximumValue / 2;
        
        while (true)
        {
            while (_currentValue < _maximumValue)
            {
                _currentValue = Mathf.MoveTowards(_currentValue, _maximumValue, _interpolationSpeed * Time.deltaTime);
                UpdateHandlerPosition();
                yield return null;
            }
            while (_currentValue > _minimumValue)
            {
                _currentValue = Mathf.MoveTowards(_currentValue, _minimumValue, _interpolationSpeed * Time.deltaTime);
                UpdateHandlerPosition();
                yield return null;
            }
        }
    }

    public override void Launch(string objective)
    {
        WriteTitle(objective);
        fill.Initialize(_winningChance);
        
        TimeUtility.CallWithDelay(1f, () => StartCoroutine(ApplyDifficultyAdjusters()));
    }

    public override void Stop()
    {
        Debug.Log(_currentValue);
        if (_currentValue >= _permissibleValuesRange.x && _currentValue <= _permissibleValuesRange.y)
        {
            base.OnChallengeComplete?.Invoke();
            Debug.Log("You won!");
        }
        else
        {
            base.OnChallengeFail?.Invoke();
            Debug.Log("You failed!");
        }
        
        StopCoroutine(InterpolateCurrentValue());
        Destroy(gameObject);
    }
    
    private IEnumerator ApplyDifficultyAdjusters(float delay = 0.3f)
    {
        /*foreach (var difficultyAdjuster in DifficultyAdjusters)
        {
            if (PlayerStats.Instance != null && PlayerStats.Instance.HasCharacteristic(difficultyAdjuster.RequiredCharacteristic) == false)
            {
                continue;
            }

            _winningChance += difficultyAdjuster.ModificationValue;
            fill.ModifyTargetFillWidth(difficultyAdjuster.ModificationValue);
            VisualizeDifficultyAdjuster(difficultyAdjuster.RequiredCharacteristic, difficultyAdjuster.ModificationValue,
                delay / 2f);
            
            yield return new WaitForSeconds(delay);
        }
        */
        
        _permissibleValuesRange = CalculatePermissibleValuesRange();

        TimeUtility.CallWithDelay(0.5f, () => StartCoroutine(InterpolateCurrentValue()));
        TimeUtility.CallWithDelay(1f, () => _interactable = true);
        TimeUtility.CallWithDelay(1f, StartCountdown);
        yield break;
    }

    private Vector2 CalculatePermissibleValuesRange()
    {
        float minimum = _maximumValue / 2 - _maximumValue * (_winningChance / 100f) / 2;
        float maximum = _maximumValue / 2 + _maximumValue * (_winningChance / 100f) / 2;
        return new Vector2(minimum, maximum);
    }
    
    private void UpdateHandlerPosition()
    {
        float handlerXPosition = Mathf.Lerp(-sliderRectTransform.rect.width / 2, sliderRectTransform.rect.width / 2, _currentValue / _maximumValue);
        handleRectTransform.anchoredPosition = new Vector2(handlerXPosition, handleRectTransform.anchoredPosition.y);
    }
    
    public void SetInterpolationSpeed(float newSpeed)
    {
        _interpolationSpeed = newSpeed;
    }

    public void SetInitialWinningChance(int chance)
    {
        _winningChance = chance;
    }
}