using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpandedItemInformationVisualizer : MonoBehaviour
{
    [SerializeField] private ItemInformationVisualizer itemInformationVisualizer;

    public void Initialize(Item item)
    {
        itemInformationVisualizer.Initialize(item);
    }
}