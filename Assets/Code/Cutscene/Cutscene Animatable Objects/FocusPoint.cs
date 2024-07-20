using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPoint : MonoBehaviour, IAnimatable
{
    [SerializeField] private ImageZoom mainImage;
    [SerializeField, Range(0, 5)] private int targetZoom;

    private bool _zoomed = false;

    public void ExecuteAnimation(float delay = 0)
    {
        mainImage.ZoomOnObject(transform, targetZoom, false);
        _zoomed = true;
    }

    public bool AnimationFinished()
    {
        return _zoomed;   
    }
}