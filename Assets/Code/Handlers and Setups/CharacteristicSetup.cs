using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicSetup : MonoBehaviour
{
    [SerializeField] private Image characteristicIcon;
    [SerializeField] private TMP_Text characteristicName;

    public void SetupCharacteristic(Characteristic characteristic)
    {
        characteristicIcon.sprite = characteristic.Icon;
        characteristicName.text = characteristic.Name;
    }
}
