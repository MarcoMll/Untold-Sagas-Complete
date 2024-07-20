using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizationDropdown : MonoBehaviour
{
    [SerializeField] private TextAsset[] localizationFiles;

    private TextAsset _selectedLocalizationFile;
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(UpdateSelectedLocalizationFile);
    }

    private void Start()
    {
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        CreateOptions();
        UpdateSelectedLocalizationFile(0);
    }
    
    private void CreateOptions()
    {
        _dropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (TextAsset textAsset in localizationFiles)
        {
            options.Add(textAsset.name);
        }

        _dropdown.AddOptions(options);
    }

    private void UpdateSelectedLocalizationFile(int dropdownValue)
    {
        if (dropdownValue > localizationFiles.Length)
        {
            Debug.LogError("Index is higher then the amount of available localization files!");
            return;
        }

        _selectedLocalizationFile = localizationFiles[dropdownValue];
        DataPersistenceManager.Instance.SetLocalizationFile(_selectedLocalizationFile);
    }
}