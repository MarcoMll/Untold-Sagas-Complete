using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacteristicButton : ListButton
{
    [SerializeField] private TMP_Text nameField;

    public void SetName(string newName)
    {
        nameField.text = newName;
    }
}