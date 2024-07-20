using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour, IItemContainable
{
    [SerializeField] private Item item;
    
    public Item GetItem()
    {
        return item;
    }
}