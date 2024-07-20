using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int HeartsAmount;
    public int CurrentEventID;
    public Chapter CurrentChapter;
    public List<CharacterData> CharacterDataList;
    public List<Characteristic> Characteristics;
    public List<Item> Inventory;
    public List<Item> QuickSlots;
    public List<ShownTooltipsData> ShownTooltips;
    public TextAsset Language;
    public AudioClip MusicClip;

    public GameData()
    {
        //Player Parameters
        this.HeartsAmount = 3;
        this.CurrentEventID = 0;
        this.CurrentChapter = null;
        this.CharacterDataList = new List<CharacterData>();
        this.Characteristics = new List<Characteristic>();
        this.Inventory = new List<Item>();
        this.QuickSlots = new List<Item>();
        this.ShownTooltips = new List<ShownTooltipsData>();

        //Game Parameters
        this.Language = null;
        this.MusicClip = null;
    }

    [System.Serializable]
    public class CharacterData
    {
        public int Relationship;
        public bool Familiar;
    }

    [System.Serializable]
    public class ShownTooltipsData
    {
        public string FieldName;
        public bool Shown;
    }
}