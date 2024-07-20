using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour, IDataPersistence
{
    [SerializeField] private ItemsVisualizer itemsVisualizer;

    private List<Item> _quickSlots = new List<Item>();
    private List<Item> _inventoryList = new List<Item>();
    private List<Item> _allItems = new List<Item>();

    public List<Item> Inventory => _inventoryList;
    public List<Item> QuickSlots => _quickSlots;
    public List<Item> AllKnownItems => _allItems;
    
    public static ItemHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void LoadData(GameData data)
    {
        if (data.Inventory.Count > 0)
        {
            _inventoryList = data.Inventory;
            _quickSlots = data.QuickSlots;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.Inventory = _inventoryList;
        data.QuickSlots = _quickSlots;
    }

    private void Start()
    {
        itemsVisualizer.UpdateItemsGrid(_quickSlots);
    }

    private void AddToQuickSlot(Item item)
    {
        if (_quickSlots.Count >= itemsVisualizer.GetMaxIconsAmount())
        {
            MoveToQuickSlotByIndex(item, 0);
        }
        else
        {
            _quickSlots.Add(item);
            _inventoryList.Remove(item);
        }
    }
    
    public void AddItem(Item item)
    {
        _inventoryList.Add(item);
        
        if (_allItems.Contains(item) == false)
            _allItems.Add(item);
        
        AddToQuickSlot(item);
        itemsVisualizer.UpdateItemsGrid(_quickSlots);
    }

    public void RemoveItem(Item item)
    {
        if (HasItem(item) == false && _quickSlots.Contains(item) == false)
        {
            Debug.LogError("Can't remove item because its not available to the player!");
            return;
        }

        if (_quickSlots.Contains(item))
        {
            _quickSlots.Remove(item);
        }
        else
        {
            _inventoryList.Remove(item);
        }
        
        itemsVisualizer.UpdateItemsGrid(_quickSlots);
    }

    public void MoveToQuickSlotByIndex(Item targetItem, int quickSlotIndex)
    {
        if (quickSlotIndex > _quickSlots.Count)
        {
            Debug.LogError("Quick slot index is bigger than the amount of available quick slots!");
            return;
        } 
        else if (quickSlotIndex < 0)
        {
            Debug.LogError("Quick slot index can't be less than zero!");
            return;
        } 
        else if (targetItem == null)
        {
            Debug.LogError("The passed item is null, operation is impossible to complete!");
            return;
        }

        if (_quickSlots[quickSlotIndex] != null)
        {
            Item removableItem = _quickSlots[quickSlotIndex];
            _inventoryList.Add(removableItem);
        }
        
        _quickSlots[quickSlotIndex] = targetItem;
        _inventoryList.Remove(targetItem);
        itemsVisualizer.UpdateItemsGrid(_quickSlots);
    }
    
    public bool HasItem(Item requiredItem)
    {
        return _inventoryList.Contains(requiredItem);
    }
    
    public int GetItemsQuantity(Item referenceItem)
    {
        int quantity = 0;
        string debugLog = $"Inventory Log: {_inventoryList.Count} items found!\n";
        
        foreach (Item item in _inventoryList)
        {
            debugLog += $"{item.Name}\n";
            if (item == referenceItem)
            {
                quantity++;
            }   
        }

        Debug.Log(debugLog);
        return quantity;
    }
}
