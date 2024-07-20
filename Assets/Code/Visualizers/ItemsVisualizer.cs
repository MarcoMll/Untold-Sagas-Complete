using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsVisualizer : MonoBehaviour
{
    [SerializeField] private Transform itemsGrid;
    [SerializeField] private Image itemObject;

    private List<Image> _itemIconsOnScene = new List<Image>();
    private int _maxItemAmount = 3;

    public void UpdateItemsGrid(List<Item> items)
    {
        DeleteCurrentItemIcons();
        SetNewItemIcons(items);
    }

    private void DeleteCurrentItemIcons()
    {
        foreach (Image itemIcon in _itemIconsOnScene)
        {
            Destroy(itemIcon.gameObject);
        }
        _itemIconsOnScene.Clear();
    }

    private void SetNewItemIcons(List<Item> items)
    {
        foreach (Item item in items)
        {
            Image itemIcon = SpawnItemIcon();
            itemIcon.sprite = item.Icon;
            _itemIconsOnScene.Add(itemIcon);
        }
    }

    private Image SpawnItemIcon()
    {
        Image itemIcon = Instantiate(itemObject, itemsGrid);
        return itemIcon;
    }

    public int GetMaxIconsAmount()
    {
        return _maxItemAmount;
    }
}
