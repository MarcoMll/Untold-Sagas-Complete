using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour, IMoveable
{
    [SerializeField] private bool localPositionOriented = false;
    private Coroutine _movementCoroutine;
    
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f; // Initialize elapsed time

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float fractionCompleted = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionCompleted);
            yield return null;
        }

        transform.position = targetPosition;
    }
    
    private IEnumerator MoveToLocalPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f; // Initialize elapsed time

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float fractionCompleted = elapsedTime / duration;
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fractionCompleted);
            yield return null;
        }

        transform.localPosition = targetPosition;
    }
    
    public void Move(Vector3 position, float duration)
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
        }
        
        if (localPositionOriented == true)
        {
            _movementCoroutine = StartCoroutine(MoveToLocalPosition(position, duration));
            return;
        }
        
        _movementCoroutine = StartCoroutine(MoveToPosition(position, duration));
    }
}