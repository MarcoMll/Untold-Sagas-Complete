using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    private Coroutine _rotationCoroutine;
    
    private IEnumerator RotateToTarget(Vector3 targetRotation, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(initialRotation, targetQuaternion, elapsed / duration);
            yield return null;
        }

        transform.rotation = targetQuaternion;
    }
    
    public void Rotate(Vector3 targetRotation, float animationDuration = 0.3f)
    {
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }

        _rotationCoroutine = StartCoroutine(RotateToTarget(targetRotation, animationDuration));
    }
}