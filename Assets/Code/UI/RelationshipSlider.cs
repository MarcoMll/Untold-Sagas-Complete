using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtilities;
using TMPro;

public class RelationshipSlider : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TMP_Text valueTextField;
    [SerializeField] private Vector2 relationshipBoundaries = new Vector2(-15f, 15f);

    private float _normalizedInitial = 0f;
    private float _normalizedTarget = 0f;

    private int _initialRelationshipValue = 0;
    private int _targetRelationshipValue = 0;
    private int _currentRelationshipValue = 0;
    
    private Coroutine _triggerDelayCoroutine = null;
    private Coroutine _changeValueCoroutine = null;

    public int Value => _currentRelationshipValue;

    public float NormalizedValue =>
        MathUtility.NormalizeValue(_currentRelationshipValue, relationshipBoundaries.x, relationshipBoundaries.y);
    
    private void UpdateTextUI()
    {
        valueTextField.text = _currentRelationshipValue.ToString();
    }
    
    private IEnumerator DelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_changeValueCoroutine != null)
        {
            progressBar.StopChangingValue();
            StopCoroutine(_changeValueCoroutine);
        }
        _changeValueCoroutine =
            StartCoroutine(SliderValueIterationCoroutine(_initialRelationshipValue, _targetRelationshipValue, 0.5f));
    }
    
    private IEnumerator SliderValueIterationCoroutine(int startValue, int endValue, float duration)
    {
        float elapsed = 0f;

        progressBar.SetTargetValue(_normalizedTarget, duration);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            _currentRelationshipValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            UpdateTextUI();

            yield return null;
        }

        _currentRelationshipValue = endValue;
        UpdateTextUI();
    }
    
    public void SetSliderValues(int initialRelationship, int targetRelationship)
    {
        _initialRelationshipValue = initialRelationship;
        _targetRelationshipValue = targetRelationship;
        
        _normalizedInitial = MathUtility.NormalizeValue(initialRelationship, relationshipBoundaries.x,
            relationshipBoundaries.y);

        _normalizedTarget = MathUtility.NormalizeValue(targetRelationship, relationshipBoundaries.x,
            relationshipBoundaries.y);
        
        _currentRelationshipValue = initialRelationship;
        progressBar.SetCurrentValue(_normalizedInitial); 
        UpdateTextUI();
    }
    
    public void TriggerSlider(float animationDelayInSeconds = 0f)
    {
        if (_triggerDelayCoroutine != null) StopCoroutine(_triggerDelayCoroutine);
        _triggerDelayCoroutine = StartCoroutine(DelayCoroutine(animationDelayInSeconds));
    }
}