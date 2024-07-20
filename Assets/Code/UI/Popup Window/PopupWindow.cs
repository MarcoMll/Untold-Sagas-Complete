using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScaleController))]
public class PopupWindow : MonoBehaviour
{
    [SerializeField] private float animationDurationInSeconds = 0.2f;
    private ScaleController _scaleController;

    public float AnimationDurationInSeconds => animationDurationInSeconds;
    
    private void Awake()
    {
        _scaleController = GetComponent<ScaleController>();
    }

    public void Show()
    {
        _scaleController.ChangeScale(Vector3.one, animationDurationInSeconds);
    }

    public void Hide()
    {
        _scaleController.ChangeScale(_scaleController.InitialScale, animationDurationInSeconds);
    }
}