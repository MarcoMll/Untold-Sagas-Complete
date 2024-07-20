using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingMinigameLogic
{
    [RequireComponent(typeof(ItemSlotVisualizer))]   
    [RequireComponent(typeof(UIInteractionDetector))]
    public class MinigameItemSlot : MonoBehaviour
    {
        private PopupWindowTrigger _itemInformationButton;
        private ItemSlotVisualizer _slotVisualizer;
        private UIInteractionDetector _uiInteractionDetector;
        private Item _item;
        private Fighter _player;
        
        public Item Item => _item;
        public bool Selected { get; private set; } = false;

        public UIInteractionDetector UIInteractionDetector => _uiInteractionDetector;
        
        private void Awake()
        {
            _slotVisualizer = GetComponent<ItemSlotVisualizer>();  
            _uiInteractionDetector = GetComponent<UIInteractionDetector>();
            _itemInformationButton = GetComponentInChildren<PopupWindowTrigger>();
        }

        private void Start()
        {
            _uiInteractionDetector.onLeftClick += ToggleSelection;
        }

        private void ToggleSelection()
        {
            Selected = !Selected;

            if (Selected == true)
            {
                if (_player.SkillPoints <= 0)
                {
                    Selected = false;
                    return;
                }
                
                _slotVisualizer.Select();
                _player.ChangeSkillPoints(-1);
            }
            else
            {
                _slotVisualizer.Deselect();
                _player.ChangeSkillPoints(1);
            }
        }

        public void SetupInformationButton(ItemInformationPopupWindow itemInformationPopupWindow)
        {
            UIInteractionDetector buttonInteractionDetector = _itemInformationButton.GetComponent<UIInteractionDetector>();
            
            _itemInformationButton.SetTriggerablePopupWindow(itemInformationPopupWindow.PopupWindow);
            buttonInteractionDetector.onLeftClick += () => itemInformationPopupWindow.ShowItemWindow(_item);
        }
        
        public void ManuallyDeselect()
        {
            _slotVisualizer.Deselect();
            Selected = false;
        }
        
        public void Setup(Item item, Fighter player)
        {
            _item = item;
            _player = player;
            
            _slotVisualizer.SetItem(_item);
        }
    }
}