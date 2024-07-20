using System;
using UnityEngine;

[RequireComponent(typeof(CharacterInformationPopupWindow))]
public class CharacterInformationPopupWindow : InformationPopupWindow
{
    private CharacterInformationVisualizer _characterInformationVisualizer;

    private void Awake()
    {
        InitializeInformationWindow();
        _characterInformationVisualizer = GetComponent<CharacterInformationVisualizer>();
    }

    public void ShowCharacterWindow(Character targetCharacter)
    {
        _characterInformationVisualizer.AssignCharacter(targetCharacter);
        _characterInformationVisualizer.Initialize();
        PopupWindow.Show();
    }
}