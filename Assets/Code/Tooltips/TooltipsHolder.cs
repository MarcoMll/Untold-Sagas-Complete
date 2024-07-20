using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class TooltipsHolder : MonoBehaviour, IDataPersistence, IResetable
{
    public TooltipData[] Tooltips;

    public static TooltipsHolder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.ShownTooltips.Count; i++)
        {
            Tooltips[i].FieldName = data.ShownTooltips[i].FieldName;
            Tooltips[i].Shown = data.ShownTooltips[i].Shown;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.ShownTooltips.Clear();

        foreach (TooltipData tooltipData in Tooltips)
        {
            GameData.ShownTooltipsData newTooltipData = new GameData.ShownTooltipsData();
            newTooltipData.FieldName = tooltipData.FieldName;
            newTooltipData.Shown = tooltipData.Shown;
            data.ShownTooltips.Add(newTooltipData);
        }
    }
     
    private void OnValidate()
    {
        foreach (TooltipData tooltipData in Tooltips)
        {
            tooltipData.UpdateNameInInspector();
        }
    }

    public void Reset()
    {
        foreach (TooltipData tooltipData in Tooltips)
        {
            tooltipData.Shown = false;
        }
    }
}

[System.Serializable]
public class TooltipData
{
    [HideInInspector] public string FieldName;

    public TooltipDataType TooltipType;
    public string[] Keywords;

    public enum TooltipDataType
    {
        Character,
        Item,
        Information
    }

    [ShowIf("CharacterType")]
    public Character Character;

    [ShowIf("ItemType")] 
    public Item Item;
    
    [ShowIf("InformationType")]
    public string tooltipTitle;
    
    [ShowIf("InformationType")]
    public string tooltipText;

    public bool Shown { get; set; }

    private bool CharacterType()
    {
        return TooltipType == TooltipDataType.Character;
    }

    private bool ItemType()
    {
        return TooltipType == TooltipDataType.Item;
    }
    
    private bool InformationType()
    {
        return TooltipType == TooltipDataType.Information;
    }

    public void UpdateNameInInspector()
    {
        switch (TooltipType)
        {
            case TooltipDataType.Character:
                FieldName = Character.Name;
                break;
            case TooltipDataType.Item:
                FieldName = Item.Name;
                break;
            default:
                FieldName = "Name unasigned";
                break;
        }
    }
}