using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingMinigameLogic
{
    public class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField] private FightController fightController;
        [SerializeField] private MinigameItemSlot itemSlotPrefab;
        [SerializeField] private ItemInformationPopupWindow itemInformationPopupWindow;
        [SerializeField] private Transform inventoryGrid;
        
        private List<Item> _inventory = new List<Item>();
        private List<MinigameItemSlot> _slots = new List<MinigameItemSlot>();
        
        private void ClearSlots()
        {
            if (_slots.Count == 0) return;
            
            foreach (var slot in _slots)
            {
                Destroy(slot.gameObject);
            }
            _slots.Clear();
        }
        
        private void VisualizeInventoryItems()
        {
            ClearSlots();
            foreach (var item in _inventory)
            {
                var slot = Instantiate(itemSlotPrefab, inventoryGrid);
                slot.Setup(item, fightController.Player);
                slot.SetupInformationButton(itemInformationPopupWindow);
                
                _slots.Add(slot);
            }
        }
        
        public void InitializeInventory(List<Item> itemsList)
        {
            _inventory = itemsList;
            VisualizeInventoryItems();
        }

        public void DeselectAllSlots()
        {
            foreach (var slot in _slots)
            {
                slot.ManuallyDeselect();
            }
        }

        public void ToggleInteractabilityToAllSlots(bool interactable)
        {
            foreach (var slot in _slots)
            {
                slot.UIInteractionDetector.ToggleInteractability(interactable);
            }
        }
        
        public void UpdatePlayerInventory()
        {
            var itemsList = new List<Item>();
            
            foreach (var slot in _slots)
            {
                if (slot.Selected == true) 
                    itemsList.Add(slot.Item);
            }
            
            fightController.Player.SelectItems(itemsList);
        }
    }
}