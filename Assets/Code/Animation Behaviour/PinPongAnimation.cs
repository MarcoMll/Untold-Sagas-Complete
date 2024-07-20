using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPongAnimation : UIAnimationBehaviour
{
    [SerializeField] private float moveDistance = 2f;
    
    private Vector3 _startPosition = Vector3.zero;
    private Coroutine _animationCoroutine;
    
    private void Start()
    {
        _startPosition = transform.position;
        if (onAwake == true)
        {
            PlayAnimation();   
        }
    }

    private IEnumerator MoveUpDown()
    {
        Vector3 endPosition = _startPosition + new Vector3(0, moveDistance, 0);
        bool movingUp = true;

        while (true)
        {
            // Determine the target position based on the direction of movement.
            Vector3 targetPosition = movingUp ? endPosition : _startPosition;

            // Move towards the target position.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationSpeed * Time.deltaTime);

            // Check if the target position is reached.
            if (transform.position == targetPosition)
            {
                // Reverse the direction of movement.
                movingUp = !movingUp;
            }

            yield return null;
        }
    }
    
    public override void PlayAnimation()
    {
        if (_animationCoroutine != null) StopAnimation();
        _animationCoroutine = StartCoroutine(MoveUpDown());
    }

    public override void StopAnimation()
    {
        if (_animationCoroutine == null) return;
        
        StopCoroutine(_animationCoroutine);
        _animationCoroutine = null;
    }
}