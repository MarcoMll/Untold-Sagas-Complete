using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpandedCharacterInformationVisualizer : MonoBehaviour
{
    [SerializeField] private CharacterInformationVisualizer characterInformationVisualizer;
    [SerializeField] private Image dynastyIcon;
    [SerializeField] private TMP_Text dynastyNameField;
    [SerializeField] private Image cityIcon;
    [SerializeField] private TMP_Text cityNameField;
    [SerializeField] private Sprite nullSprite;
    
    public void Initialize(Character character)
    {
        characterInformationVisualizer.AssignCharacter(character);
        characterInformationVisualizer.Initialize();
        
        dynastyNameField.text = character.Dynasty != null ? character.Dynasty.Name : "Неизвестно";
        dynastyIcon.sprite = character.Dynasty != null ? character.Dynasty.CoatOfArms : nullSprite;

        cityNameField.text = character.HomeTown != null ? character.HomeTown.Name : "Неизвестно";
        cityIcon.sprite = character.HomeTown != null ? character.HomeTown.Icon : nullSprite;
    }
}