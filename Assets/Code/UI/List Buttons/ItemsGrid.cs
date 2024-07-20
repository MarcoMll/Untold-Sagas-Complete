using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemsGrid : ButtonsListGrid
{
    [SerializeField] private QuickSlotsController quickSlotsController;
    [SerializeField] private Book bookReference;
    [SerializeField] private Page itemInformationPage;
    [SerializeField] private ExpandedItemInformationVisualizer itemInformationVisualizer;
    [SerializeField] private ItemButton buttonPrefab;

    private void Start()
    {
        quickSlotsController.SetItemsGrid(this);
    }

    private void InitializeButton(ItemButton targetButton, Item targetItem)
    {
        targetButton.SetImage(targetItem.Icon);
        targetButton.SetName(targetItem.Name);
        
        targetButton.AddOnClickAction(() => bookReference.ActivateSubpage(itemInformationPage));
        targetButton.AddOnClickAction(() => itemInformationVisualizer.Initialize(targetItem));
        targetButton.AddOnClickAction(() => SelectItemButton(targetButton)); 
        targetButton.AddOnClickAction(() => quickSlotsController.Hide());
        
        targetButton.AddOnEquipButtonClickAction(() => quickSlotsController.Initialize(targetItem));
    }
    
    private void CreateEquippedItemButton(Item targetItem)
    {
        ItemButton itemButton = base.InstantiateButton(buttonPrefab) as ItemButton;
        InitializeButton(itemButton, targetItem);
        itemButton.ActivateEquippedIcon();
    }
    
    private void CreateItemButton(Item targetItem)
    {
        ItemButton itemButton = base.InstantiateButton(buttonPrefab) as ItemButton;
        if (targetItem.Stackable == true)
        {
            int quantity = PlayerStats.Instance.ItemHandler.GetItemsQuantity(targetItem);
            if (quantity > 1)
            {
                itemButton.SetQuantity(quantity);
            }
        }
        
        InitializeButton(itemButton, targetItem);
    }

    private void CreateAbsentItemButton(Item targetItem)
    {
        ItemButton itemButton = base.InstantiateButton(buttonPrefab) as ItemButton;
        InitializeButton(itemButton, targetItem);
        itemButton.Deactivate();
    }

    private void DeselectAllButtons(ItemButton exception)
    {
        foreach (var listButton in buttonsList)
        {
            var itemButton = (ItemButton)listButton;
            if (exception != itemButton)
                itemButton.OnDeselect();
        }
    }

    private void SelectItemButton(ItemButton targetButton)
    {
        DeselectAllButtons(targetButton);
        targetButton.OnSelect();
    }
    
    public override void UpdateGrid()
    {
        base.ClearGrid();
        List<Item> absentItems = new List<Item>();

        foreach (Item item in PlayerStats.Instance.ItemHandler.QuickSlots)
        {
            CreateEquippedItemButton(item);
        }
        
        foreach (Item item in PlayerStats.Instance.ItemHandler.AllKnownItems)
        {
            if (PlayerStats.Instance.ItemHandler.HasItem(item))
            {
                CreateItemButton(item);
            }
            else
            {
                if (PlayerStats.Instance.ItemHandler.QuickSlots.Contains(item) == false)
                    absentItems.Add(item);
            }
        }

        foreach (Item absentItem in absentItems)
        {
            CreateAbsentItemButton(absentItem);
        }
    }
}