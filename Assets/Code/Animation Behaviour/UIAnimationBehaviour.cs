using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIAnimationBehaviour : MonoBehaviour
{
    [SerializeField] protected bool onAwake = true;
    [SerializeField] protected float animationSpeed = 5f;
    
    public abstract void PlayAnimation();
    public abstract void StopAnimation();
}