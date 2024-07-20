using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInformationVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text nameField;
    [SerializeField] private TMP_Text descriptionField;
    [SerializeField] private TMP_Text effectsField;
    [SerializeField] private Image icon;
    
    public void Initialize(Item item)
    {
        if (item == null) return;
        
        if (nameField != null) nameField.text = item.Name;
        if (descriptionField != null) descriptionField.text = item.Description;
        if (effectsField != null)
        {
            string effects = String.Empty;
            foreach (var effect in item.Effects)
            {
                effects += $"{effect}\n";
            }

            effectsField.text = effects;
        }

        if (icon != null)
        {
            icon.sprite = item.Icon;
        }
    }
}
