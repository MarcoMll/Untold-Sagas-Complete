using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    [SerializeField] private Vector2 initialScale = new Vector2(1f, 1f);
    private Coroutine _scaleCoroutine;
    
    public Vector3 InitialScale => initialScale;

    private void Start()
    {
        transform.localScale = initialScale;
    }
    
    private IEnumerator ChangeScaleCoroutine(Vector3 targetScale, float duration)
    {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float ratio = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, ratio);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        transform.localScale = targetScale;
    }

    public void ChangeScale(Vector3 newScale, float duration = 0.3f)
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }

        _scaleCoroutine = StartCoroutine(ChangeScaleCoroutine(newScale, duration));
    }
}