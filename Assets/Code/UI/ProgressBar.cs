using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private float maximumValue = 100f;
    [SerializeField] private Image mask;

    [Header("Progress Bar Events")]
    [SerializeField] private UnityEvent onProgressBarFull;

    private float _currentValue = 0f;
    private Coroutine _valueChangeCoroutine;
    
    public float Value => _currentValue;
    
    private IEnumerator ChangeValueOverTime(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _currentValue = Mathf.Lerp(startValue, endValue, elapsed / duration);
            elapsed += Time.deltaTime;
            UpdateCurrentFillValue();
            yield return null;
        }

        _currentValue = endValue;
        UpdateCurrentFillValue();

        if (_currentValue >= maximumValue)
        {
            onProgressBarFull?.Invoke();
        }
    }

    private void UpdateCurrentFillValue()
    {
        float fillAmount = _currentValue / maximumValue;
        mask.fillAmount = fillAmount;
    }

    public void SetCurrentValue(float value)
    {
        _currentValue = value;
        UpdateCurrentFillValue();
    }
    
    public void SetTargetValue(float targetValue, float duration)
    {
        if (targetValue > maximumValue) targetValue = maximumValue;
        StopChangingValue();
        
        _valueChangeCoroutine = StartCoroutine(ChangeValueOverTime(_currentValue, targetValue, duration));
    }

    public void StopChangingValue()
    {
        if (_valueChangeCoroutine != null) 
            StopCoroutine(_valueChangeCoroutine);
    }
    
    public void DestroyItself()
    {
        Destroy(gameObject);
    }
}