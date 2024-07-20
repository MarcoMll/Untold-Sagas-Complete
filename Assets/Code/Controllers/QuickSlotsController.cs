using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsController : MonoBehaviour
{
    [SerializeField] private ItemSlotVisualizer[] quickSlots;
    [SerializeField] private Button confirmationButton;
    [SerializeField] private ScaleController scaleController;

    private ItemsGrid _itemsGrid;
    private Item _targetItem;
    private ItemSlotVisualizer _selectedSlot;

    private void DeselectSlots(ItemSlotVisualizer exception = null)
    {
        foreach (var quickSlot in quickSlots)
        {
            if (exception != null && quickSlot == exception) continue;
            quickSlot.Deselect();
        }
    }

    private void SelectSlot(ItemSlotVisualizer targetSlot)
    {
        if (targetSlot == null || _targetItem == null) return;
        _selectedSlot = targetSlot;
        
        DeselectSlots(_selectedSlot);
        SetInitialItems();
        _selectedSlot.Select();
        _selectedSlot.SetItem(_targetItem);
        confirmationButton.interactable = true;
    }
    
    private void SetInitialItems()
    {
        for (int i = 0; i < PlayerStats.Instance.ItemHandler.QuickSlots.Count; i++)
        { 
            ItemSlotVisualizer itemSlot = quickSlots[i];
            Item item = PlayerStats.Instance.ItemHandler.QuickSlots[i];
            
            itemSlot.SetItem(item);
        }
    }

    private void SetupSlots()
    {
        foreach (var quickSlot in quickSlots)
        {
            quickSlot.AddOnClickAction(() => SelectSlot(quickSlot));
        }   
    }

    private void ConfirmChoice()
    {
        int slotIndex = 0;
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (_selectedSlot == quickSlots[i])
            {
                slotIndex = i;
                Debug.Log($"Slot index: {i}");
                break;
            }
        }
        
        PlayerStats.Instance.ItemHandler.MoveToQuickSlotByIndex(_targetItem, slotIndex);
    }
    
    private void SetupButton()
    {
        confirmationButton.interactable = false;
        confirmationButton.onClick.RemoveAllListeners();
        confirmationButton.onClick.AddListener(ConfirmChoice);
        confirmationButton.onClick.AddListener(Hide);
        confirmationButton.onClick.AddListener(_itemsGrid.UpdateGrid);
    }
    
    public void Initialize(Item item)
    {
        _targetItem = item;
        scaleController.ChangeScale(new Vector3(1f,1f,0f), .3f);
        
        SetupButton();
        DeselectSlots();
        SetInitialItems();
        SetupSlots();
    }

    public void Hide()
    {
        DeselectSlots();
        SetInitialItems();
        scaleController.ChangeScale(scaleController.InitialScale, .3f);
    }

    public void SetItemsGrid(ItemsGrid itemsGrid)
    {
        _itemsGrid = itemsGrid;
    }
}
