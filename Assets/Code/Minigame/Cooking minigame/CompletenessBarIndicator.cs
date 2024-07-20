using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CookingMinigameLogic
{
    [RequireComponent(typeof(ObjectMover))]
    public class CompletenessBarIndicator : MonoBehaviour
    {
        [SerializeField] private Image indicator;
        [SerializeField] private Sprite passIndicatorSprite, idleIndicatorSprite;
        [SerializeField] private ObjectMover indicatorMover;
        [SerializeField] private float idleYPosition = 80f, passYPosition = 60f;

        private float _yPosition;
        
        private void Awake()
        {
            indicatorMover = GetComponent<ObjectMover>();
        }

        private void Start()
        {
            _yPosition = transform.localPosition.y;
        }

        public void MovePoint(float newX, bool instant = false)
        {
            Vector2 newPosition = new Vector2(newX, _yPosition);
            
            if (instant)
            {
                indicatorMover.transform.localPosition = newPosition;
                return;
            }
            
            indicatorMover.Move(newPosition, 0.3f);
        }

        private void SetYPosition(float newY)
        {
            _yPosition = newY;
        }
        
        public void SetPassState()
        {
           SetYPosition(passYPosition);
           indicator.sprite = passIndicatorSprite;
        }

        public void SetIdleState()
        {
            SetYPosition(idleYPosition);
            indicator.sprite = idleIndicatorSprite;
        }
    }
}