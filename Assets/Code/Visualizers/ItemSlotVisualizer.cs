using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CustomInspector;

public class ItemSlotVisualizer : MonoBehaviour
{
    [HorizontalLine("Slot setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject selectionBox;
    [SerializeField] private Button slotButton;

    public void AddOnClickAction(UnityAction action)
    {
        slotButton.onClick.AddListener(action);
    }

    public void Select()
    {
        selectionBox.SetActive(true);
    }
    
    public void Deselect()
    {
        selectionBox.SetActive(false);
    }
    
    public void SetItem(Item item)
    {
        itemIcon.gameObject.SetActive(true);
        
        if (item == null)
        {
            itemIcon.gameObject.SetActive(false);
            return;
        } 
        
        itemIcon.sprite = item.Icon;
    }
}