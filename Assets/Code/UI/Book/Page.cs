using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using UnityEngine.Events;

public class Page : MonoBehaviour
{
    [HorizontalLine("Page Customization")]
    [SerializeField] private CanvasGroupController canvasGroupController;
    [SerializeField] private UnityEvent onPageActivation;
    
    public void Show(float animationDuration)
    {
        canvasGroupController.SmoothlyChangeAlpha(1f, animationDuration);
        canvasGroupController.CanvasGroup.blocksRaycasts = true;
        onPageActivation?.Invoke();
    }

    public void Hide(float animationDuration)
    {
        canvasGroupController.SmoothlyChangeAlpha(0f, animationDuration);
        canvasGroupController.CanvasGroup.blocksRaycasts = false;
    }
}