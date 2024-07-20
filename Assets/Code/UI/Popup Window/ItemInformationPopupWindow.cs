using UnityEngine;

[RequireComponent(typeof(ItemInformationPopupWindow))]
public class ItemInformationPopupWindow : InformationPopupWindow
{
    private ItemInformationVisualizer _itemInformationVisualizer;
    
    private void Awake()
    {
        InitializeInformationWindow();
        _itemInformationVisualizer = GetComponent<ItemInformationVisualizer>();
    }

    public void ShowItemWindow(Item item)
    {
        _itemInformationVisualizer.Initialize(item);
        PopupWindow.Show();
    }
}