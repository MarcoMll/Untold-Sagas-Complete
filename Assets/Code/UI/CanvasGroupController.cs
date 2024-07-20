using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : MonoBehaviour, ISmoothlyAlphaChangeable
{
    private CanvasGroup _canvasGroup;
    private Coroutine _coroutine;

    public CanvasGroup CanvasGroup => _canvasGroup;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator ChangeAlpha(float targetAlpha, float duration)
    {
        float initialAlpha = _canvasGroup.alpha;
        float elapsed = 0f;
        targetAlpha = Mathf.Clamp01(targetAlpha);
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // Calculate the interpolation factor
            float currentAlpha = Mathf.Lerp(initialAlpha, targetAlpha, t);

            _canvasGroup.alpha = currentAlpha;
            yield return null;
        }

        // Ensure the alpha is set to the target value at the end
        _canvasGroup.alpha = targetAlpha;
    } 
    
    public void SmoothlyChangeAlpha(float targetAlpha, float duration)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(ChangeAlpha(targetAlpha, duration));
    }
}