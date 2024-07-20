using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class ObjectMovementController : MonoBehaviour, IPositionReturnable
{
    [SerializeField] private ObjectMover mover;
    [SerializeField] private Transform initial;
    [SerializeField] private Transform target;

    public void MoveToInitial(float duration)
    {
        mover.Move(initial.position, duration);
    }

    public void MoveToTarget(float duration)
    {
        mover.Move(target.position, duration);
    }
}