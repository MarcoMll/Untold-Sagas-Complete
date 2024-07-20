using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicStatsSetup : SecondaryEvent
{
    [SerializeField] private Text nameField;
    [SerializeField] private Image icon;

    public void SetupCharacteristic(Characteristic characteristic)
    {
        nameField.text = characteristic.Name;
        icon.sprite = characteristic.Icon;
    }
}
