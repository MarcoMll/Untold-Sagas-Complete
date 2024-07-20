using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonsListGrid : MonoBehaviour
{
    [SerializeField] private Transform parentGrid;
    protected List<ListButton> buttonsList = new List<ListButton>();
    
    public void ClearGrid()
    {
        foreach (var button in buttonsList)
        {
            Destroy(button.gameObject);  
        }
        
        buttonsList.Clear();
    }

    public ListButton InstantiateButton(ListButton prefab)
    {
        ListButton button = Instantiate(prefab, parentGrid);
        buttonsList.Add(button);
        return button;
    }

    public abstract void UpdateGrid();
}