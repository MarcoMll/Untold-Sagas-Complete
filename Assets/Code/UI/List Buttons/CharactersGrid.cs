using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersGrid : ButtonsListGrid
{
    [SerializeField] private Book referenceBook;
    [SerializeField] private Page characterInformationPage;
    [SerializeField] private ExpandedCharacterInformationVisualizer characterInformationVisualizer;
    [SerializeField] private CharacterButton buttonPrefab;

    private void AddCharacterButton(Character targetCharacter)
    {
        CharacterButton characterButton = base.InstantiateButton(buttonPrefab) as CharacterButton;
        characterButton.SetImage(targetCharacter.Idle);
        
        characterButton.AddOnClickAction(() => referenceBook.ActivatePage(characterInformationPage));       
        characterButton.AddOnClickAction(() => characterInformationVisualizer.Initialize(targetCharacter));
    }
    
    public override void UpdateGrid()
    {
        base.ClearGrid();
        foreach (var character in CharactersContainer.Instance.GetAllCharacters())
        {
            if (character.Familiar)
                AddCharacterButton(character);
        }
    }
}