using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersContainer : MonoBehaviour, IDataPersistence, IResetable
{
    [SerializeField] private List<Character> characters;

    public static CharactersContainer Instance { private set; get; }
    
    private void Awake()
    {
        Instance = this;
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.CharacterDataList.Count; i++)
        {
            GameData.CharacterData characterData = data.CharacterDataList[i];
            Character character = characters[i];

            character.Relationship = characterData.Relationship;
            character.Familiar = characterData.Familiar;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.CharacterDataList.Clear();

        foreach (Character character in characters)
        {
            GameData.CharacterData characterData = new GameData.CharacterData();
            characterData.Relationship = character.Relationship;
            characterData.Familiar = character.Familiar;
            data.CharacterDataList.Add(characterData);
        }
    }

    public void Reset()
    {
        foreach (Character character in characters)
        {
            character.Relationship = character.InitialRelationship;
            character.Familiar = false;
        }
    }

    public List<Character> GetAllCharacters()
    {
        return new List<Character>(characters);
    }
}