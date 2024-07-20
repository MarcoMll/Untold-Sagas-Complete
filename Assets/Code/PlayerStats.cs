using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacteristicSetup))]
[RequireComponent(typeof(ItemHandler))]
[RequireComponent(typeof(HeartsVisualizer))]
public class PlayerStats : MonoBehaviour, IDataPersistence
{
    private int _heartsAmount = 3;

    private List<Characteristic> _acquiredCharacteristics = new List<Characteristic>();
    private CharacteristicSetup _lastCharacteristic;

    private HeartsVisualizer _heartsVisualizer;

    public int Hearts => _heartsAmount;
    
    public ItemHandler ItemHandler { get; private set; }

    public static PlayerStats Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _heartsVisualizer.UpdateHeartGrid(_heartsAmount);
    }

    public void LoadData(GameData data)
    {
        GetAllComponents();

        _heartsAmount = data.HeartsAmount;

        if (data.Characteristics.Count > 0)
        {
            _acquiredCharacteristics = data.Characteristics;

            int lastCharacteristicIndex = _acquiredCharacteristics.Count - 1;
            _lastCharacteristic.SetupCharacteristic(_acquiredCharacteristics[lastCharacteristicIndex]);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.HeartsAmount = _heartsAmount;
        data.Characteristics = _acquiredCharacteristics;
    }

    private void GetAllComponents()
    {
        _lastCharacteristic = GetComponent<CharacteristicSetup>();
        ItemHandler = GetComponent<ItemHandler>();
        _heartsVisualizer = GetComponent<HeartsVisualizer>();
    }

    public void AddCharacteristic(Characteristic characteristic)
    {
        if (_acquiredCharacteristics.Contains(characteristic)) return;

        _acquiredCharacteristics.Add(characteristic);
        _lastCharacteristic.SetupCharacteristic(characteristic);
    }
    
    public List<Characteristic> GetAllCharacteristics()
    {
        return _acquiredCharacteristics;
    }

    public bool HasCharacteristic(Characteristic targetCharacteristic)
    {
        foreach (var characteristic in _acquiredCharacteristics)
        {
            if (characteristic == targetCharacteristic)
            {
                return true;
            }
        }

        return false;
    }
    
    public void ChangeHeartsAmount(int amount)
    {
        _heartsAmount += amount;

        if (amount < 0)
        {
            _heartsVisualizer.UpdateHeartGrid(_heartsAmount);
            StartCoroutine(_heartsVisualizer.ShowBrokenHearts(_heartsAmount, Mathf.Abs(amount)));
        }
        else
        {
            _heartsVisualizer.UpdateHeartGrid(_heartsAmount);
        }
    }

    public void ChangeRelationship(Character character, int amount)
    {
        character.Relationship += amount;
    }
}