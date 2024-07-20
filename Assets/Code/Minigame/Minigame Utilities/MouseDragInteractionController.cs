using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MouseDragInteractionController : MonoBehaviour
{
    private Vector3 _mousePosition;
    private IDraggable _draggable;
    private IPositionReturnable _returnable;
    
    private void Update()
    {
        _mousePosition = Input.mousePosition;
        if (_draggable != null) _draggable.Drag(_mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            CheckForDraggableUnderMouse();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DeselectDraggable();
        }
    }

    private void CheckForDraggableUnderMouse()
    {
        List<RaycastResult> results = GetAllObjectsUnderMouse();
        foreach (RaycastResult result in results)
        {
            IDraggable draggable = result.gameObject.GetComponent<IDraggable>();
            if (draggable != null)
            {
                SelectDraggable(draggable);
                _returnable = result.gameObject.GetComponent<IPositionReturnable>();
                break;
            }
        }
    }

    private void SelectDraggable(IDraggable draggable)
    {
        _draggable = draggable;
        draggable.Resize(new Vector3(1.2f, 1.2f, 1.2f));
        draggable.SetShadowTransparency(0f);
    }

    private void DeselectDraggable()
    {
        if (_draggable == null) return;
        _draggable.ResizeToInitial();
        _draggable.SetShadowTransparency(0.5f);
        _draggable = null;

        if (_returnable != null)
        {
            _returnable.MoveToInitial(0.3f);
            _returnable = null;
        }
    }

    public List<RaycastResult> GetAllObjectsUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results;
    }
}