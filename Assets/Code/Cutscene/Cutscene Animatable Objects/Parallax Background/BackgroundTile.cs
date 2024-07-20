using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BackgroundTile : MonoBehaviour
{
    private Transform _targetPoint = null;
    private float _movementSpeed = 0f;
    private Coroutine _movementCoroutine;

    public float Length => GetComponent<RectTransform>().rect.width;
    public bool HasSpawnedNext { get; set; } = false;

    private IEnumerator Move()
    {
        while (true)
        {
            Vector3 targetPosition = _targetPoint.position;
            targetPosition.x += Length / 2;
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _movementSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void StartMoving(Transform target, float speed)
    {
        StopMoving();

        _targetPoint = target;
        _movementSpeed = speed;
        _movementCoroutine = StartCoroutine(Move());
    }

    public void StopMoving()
    {
        if (_movementCoroutine == null) return;
        StopCoroutine(_movementCoroutine);
        _movementCoroutine = null;
    }
}