using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISmoothlyAlphaChangeable
{
    public void SmoothlyChangeAlpha(float targetAlpha, float duration);
}