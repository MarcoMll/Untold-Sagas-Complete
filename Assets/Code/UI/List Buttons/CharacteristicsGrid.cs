using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicsGrid : ButtonsListGrid
{
    [SerializeField] private CharacteristicButton buttonPrefab;
    private List<CharacteristicButton> _characterButtons = new List<CharacteristicButton>();
    
    public override void UpdateGrid()
    {
        base.ClearGrid();
        foreach (var characteristic in PlayerStats.Instance.GetAllCharacteristics())
        {
            CharacteristicButton characteristicButton = InstantiateButton(buttonPrefab) as CharacteristicButton;
            characteristicButton.SetImage(characteristic.Icon);
            characteristicButton.SetName(characteristic.Name);
        }
    }
}