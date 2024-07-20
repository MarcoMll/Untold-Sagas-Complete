using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIInteractionDetector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool _interactable = true;

    public Action onMouseEnter;
    public Action onMouseExit;
    public Action onLeftClick;
    public Action onRightClick;
    public Action onMiddleClick;
    
    public void ToggleInteractability(bool interactable)
    {
        _interactable = interactable;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_interactable == false) return;

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Right:
                onRightClick?.Invoke();
                break;
            
            case PointerEventData.InputButton.Left:
                onLeftClick?.Invoke();
                break;
            
            case PointerEventData.InputButton.Middle:
                onMiddleClick?.Invoke();
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseExit?.Invoke();
    }
}