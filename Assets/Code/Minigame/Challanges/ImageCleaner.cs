using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;
using UnityEngine.UI;

public class ImageCleaner : Challenge
{
    [SerializeField] private MousePositionMask targetImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private ScaleController item; 
    [SerializeField] private ProgressBar progressBar;

    private float _speed = 0.01f;
    private float _currentValue = 0f;
    private bool _initialized = false;
    private Vector2 _lastMousePosition;

    private Coroutine _changingValueCoroutine;
    
    private void Update()
    {
        if (!_initialized) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_changingValueCoroutine != null) StopCoroutine(_changingValueCoroutine);
            progressBar.SetCurrentValue(_currentValue);
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 uvPos = Camera.main.ScreenToViewportPoint(mousePos);
            float distanceMoved = Vector2.Distance(mousePos, _lastMousePosition);

            if (distanceMoved <= 10f)
            {
                _changingValueCoroutine ??= StartCoroutine(ChangeCurrentValue(0f, 5f));
            }
            else
            {
                if (_changingValueCoroutine != null) StopCoroutine(_changingValueCoroutine);
            }
            
            float increment = (distanceMoved * _speed * Time.deltaTime);
            _currentValue = Mathf.Min(_currentValue + increment, 1f);

            targetImage.FadeArea(uvPos);
            UpdateUI();
            
            _lastMousePosition = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_changingValueCoroutine != null) StopCoroutine(_changingValueCoroutine);
            _changingValueCoroutine = StartCoroutine(ChangeCurrentValue(0f, 5f));
        }

        if (_currentValue >= 1f)
        {
            _currentValue = 1f;
            Stop();
        }
    }
    
    public override void Launch(string objective)
    {
        WriteTitle(objective);
        TimeUtility.CallWithDelay(1f ,() => StartCoroutine(ApplyDifficultyAdjusters(0.5f)));
    }

    public override void Stop()
    {
        if (_currentValue >= 1f)
        {
            base.OnChallengeComplete?.Invoke();
        }
        else
        {
            base.OnChallengeFail?.Invoke();
        }
        
        Destroy(gameObject);
    }
    
    private IEnumerator ChangeCurrentValue(float targetValue, float duration)
    {
        float elapsed = 0f;
        float startValue = _currentValue;
        
        while (elapsed < duration)
        {
            _currentValue = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            elapsed += Time.deltaTime;
            UpdateUI();
            yield return null;
        }

        _currentValue = targetValue;
        UpdateUI();
        yield break;
    }

    private void UpdateUI()
    {
        progressBar.SetCurrentValue(_currentValue);
        SetBackgroundImageAlpha(1f - _currentValue);
    }
    
    private void SetBackgroundImageAlpha(float value)
    {
        Color oldColor = backgroundImage.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, value);
        backgroundImage.color = newColor;
    }

    private IEnumerator ApplyDifficultyAdjusters(float delay = 0.3f)
    {
        /*foreach (var difficultyAdjuster in DifficultyAdjusters)
        {
            if (PlayerStats.Instance != null && PlayerStats.Instance.HasCharacteristic(difficultyAdjuster.RequiredCharacteristic) == false)
            {
                continue;
            }

            float width = item.InitialScale.x;
            float height = item.InitialScale.y;
            
            width += MathUtility.CalculatePercentageValue(difficultyAdjuster.ModificationValue, width);
            height += MathUtility.CalculatePercentageValue(difficultyAdjuster.ModificationValue, height);

            Vector3 newScale = new Vector3(width, height, 0f);
            item.ChangeScale(newScale, delay / 2f);
            
            _speed += MathUtility.CalculatePercentageValue(difficultyAdjuster.ModificationValue, _speed);
            
            VisualizeDifficultyAdjuster(difficultyAdjuster.RequiredCharacteristic, 
                difficultyAdjuster.ModificationValue, delay / 2f);

            yield return new WaitForSeconds(delay);
        }*/

        TimeUtility.CallWithDelay(0.5f, StartCountdown);
        TimeUtility.CallWithDelay(0.5f, () => _initialized = true);
        yield break;
    }
}
