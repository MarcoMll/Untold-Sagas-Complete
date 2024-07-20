using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInformationVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text nameField;
    [SerializeField] private TMP_Text roleField;
    [SerializeField] private TMP_Text descriptionField;
    [SerializeField] private Image characterImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private RelationshipSlider relationshipBar;
    
    [SerializeField] private bool includeLastName;
    
    private Character _character;

    public void AssignCharacter(Character newCharacter)
    {
        _character = newCharacter;
    }
    
    public void Initialize()
    {
        if (_character == null) return;

        if (nameField != null) nameField.text = includeLastName ? $"{_character.Name} {_character.FamilyName}" : $"{_character.Name}";
        if (roleField != null) roleField.text = _character.Role;
        if (descriptionField != null) descriptionField.text = _character.Description;
        if (characterImage != null) characterImage.sprite = _character.Idle;      
        if (backgroundImage != null) backgroundImage.sprite = _character.Background;
        if (relationshipBar != null)
        {
            relationshipBar.SetSliderValues(relationshipBar.Value, _character.Relationship);
            relationshipBar.TriggerSlider();
        }
    }
}