using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[RequireComponent(typeof(UIInteractionDetector))]
public class PopupWindowTrigger : MonoBehaviour
{
    [SerializeField] private PopupWindow popupWindow;

    [Header("Enter triggers")] 
    [SerializeField] private bool onClick;
    [SerializeField] private bool onMouseEnter;
    
    private UIInteractionDetector _uiInteractionDetector;
    
    private void Awake()
    {
        _uiInteractionDetector = GetComponent<UIInteractionDetector>();
    }

    private void Start()
    {
        if (popupWindow != null)
            SubscribeToPopupWindow(popupWindow);
    }

    private void SubscribeToPopupWindow(PopupWindow newPopupWindow)
    {
        if (newPopupWindow == null) return;

        if (popupWindow != null)
        {
            UnsubscribeToPopupWindow();
        }

        popupWindow = newPopupWindow;
        
        if (onMouseEnter)
        {
            _uiInteractionDetector.onMouseEnter += popupWindow.Show;
        }

        if (onClick)
        {
            _uiInteractionDetector.onLeftClick += popupWindow.Show;
        }
        
        _uiInteractionDetector.onMouseExit += popupWindow.Hide;
    }

    private void UnsubscribeToPopupWindow()
    {
        if (popupWindow == null) return;
        
        if (onMouseEnter)
        {
            _uiInteractionDetector.onMouseEnter -= popupWindow.Show;
        }

        if (onClick)
        {
            _uiInteractionDetector.onLeftClick -= popupWindow.Show;
        }
        
        _uiInteractionDetector.onMouseExit -= popupWindow.Hide;
        popupWindow = null;
    }
    
    public void SetTriggerablePopupWindow(PopupWindow newPopupWindow)
    {
        SubscribeToPopupWindow(newPopupWindow);
    }
}