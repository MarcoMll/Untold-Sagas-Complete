using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMenu
{
    [RequireComponent(typeof(CanvasGroupController))]
    [RequireComponent(typeof(ObjectMover))]
    public class MenuPanel : MonoBehaviour
    {
        private float _animationDuration;
        private Transform _showPosition, _hidePosition;
        private bool _initialized = false;

        private CanvasGroupController _groupController;
        private ObjectMover _mover;
        
        private void Awake()
        {
            _groupController = GetComponent<CanvasGroupController>();
            _mover = GetComponent<ObjectMover>();
        }

        public void Setup(Transform showPosition, Transform hidePosition, float animationDuration)
        {
            _showPosition = showPosition;
            _hidePosition = hidePosition;
            _animationDuration = animationDuration;
            _initialized = true;
        }

        public void Show()
        {
            if (_initialized == false)
            {
                Debug.LogError("The panel is not initialized! Make sure to call Setup method first.");
                return;
            }
            
            _mover.Move(_showPosition.position, _animationDuration);
            _groupController.SmoothlyChangeAlpha(1f, _animationDuration);
        }
        
        public void Hide()
        {
            if (_initialized == false)
            {
                Debug.LogError("The panel is not initialized! Make sure to call Setup method first.", this);
                return;
            }
            
            _mover.Move(_hidePosition.position, _animationDuration);
            _groupController.SmoothlyChangeAlpha(0f, _animationDuration);
        }

        public bool IsInitialized()
        {
            return _initialized;
        }
    }
}