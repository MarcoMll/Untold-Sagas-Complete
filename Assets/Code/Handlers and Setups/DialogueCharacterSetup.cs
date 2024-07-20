using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueCharacterSetup : MonoBehaviour
{
    [SerializeField] private DynamicImage characterIcon;
    [SerializeField] private GameObject[] dialogueComponents;
    [SerializeField] private TMP_Text characterNameField;

    private Character _character;

    private void VisualizeCharacter(Emotion emotion, Color spriteColor)
    {
        if (_character == null)
        {
            Debug.LogError("No character reference!");
            return;
        }

        Color iconColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 255f);

        switch (emotion)
        {
            case Emotion.Idle:
                SetCharacterIcon(_character.Idle, iconColor);
                break;

            case Emotion.Happy:
                SetCharacterIcon(_character.Happy, iconColor);
                break;

            case Emotion.Sad:
                SetCharacterIcon(_character.Sad, iconColor);
                break;

            case Emotion.Angry:
                SetCharacterIcon(_character.Angry, iconColor);
                break;

            default:
                SetCharacterIcon(_character.Idle, Color.white);
                break;
        }

    }

    private void SetCharacterIcon(Sprite icon, Color newColor)
    {
        characterIcon.SmoothlyChangeImage(icon, newColor);
    }

    private void SetCharacterName(string name)
    {
        characterNameField.text = name;
    }

    private void SetCharacter(Character character)
    {
        if (_character == character) return;

        _character = character;
    }

    public void SetupCharacter(Character character, Emotion emotion, Color color)
    {
        SetCharacter(character);
        VisualizeCharacter(emotion, color);
        SetCharacterName(_character.Name);
    }

    public void ToggleDialogueVisibility(bool value)
    {
        if (value == false)
        {
            SetCharacterIcon(null, Color.white);
        }

        foreach (GameObject dialogueComponent in dialogueComponents)
        {
            dialogueComponent.gameObject.SetActive(value);
        }
    }
}