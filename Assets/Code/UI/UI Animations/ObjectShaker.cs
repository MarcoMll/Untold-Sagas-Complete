using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class ObjectShaker : MonoBehaviour
{
    private Vector3 _initialPosition = Vector3.zero;
    private Coroutine _shakingCoroutine;
    
    private void Start()
    {
        _initialPosition = transform.localPosition;
    }
    
    private IEnumerator ShakeCoroutine(float force, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * force;
            float y = Random.Range(-1f, 1f) * force;

            transform.localPosition = new Vector3(_initialPosition.x + x, _initialPosition.y + y, _initialPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _initialPosition;
    }
    
    public void Shake(float force = 5f)
    {
        if (_shakingCoroutine != null) StopCoroutine(_shakingCoroutine);
        _shakingCoroutine = StartCoroutine(ShakeCoroutine(force, 0.5f));
    }

    public void InitializeShaking(float force, float duration)
    {
        if (_shakingCoroutine != null) StopCoroutine(_shakingCoroutine);
        StartCoroutine(ShakeCoroutine(force, duration));
    }
}