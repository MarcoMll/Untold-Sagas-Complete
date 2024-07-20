using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

[RequireComponent(typeof(ObjectMovementController))]
[RequireComponent(typeof(CanvasGroupController))]
[RequireComponent(typeof(Button))]
public class DynamicButton : MonoBehaviour
{
    private ObjectMovementController _movementController;
    private CanvasGroupController _canvasGroup;
    public Button Button { get; private set; }

    private void Awake()
    {
        _movementController = GetComponent<ObjectMovementController>();
        _canvasGroup = GetComponent<CanvasGroupController>();
        
        Button = GetComponent<Button>();
    }
    
    public void Show(float animationDuration)
    {
        _movementController.MoveToTarget(animationDuration);
        _canvasGroup.SmoothlyChangeAlpha(1f, animationDuration);
        _canvasGroup.CanvasGroup.interactable = true;
    }

    public void Hide(float animationDuration)
    {
        _movementController.MoveToInitial(animationDuration);
        _canvasGroup.SmoothlyChangeAlpha(0f, animationDuration);
        _canvasGroup.CanvasGroup.interactable = false;
    }
}