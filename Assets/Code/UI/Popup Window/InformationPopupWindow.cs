using CustomUtilities;
using UnityEngine;

[RequireComponent(typeof(PopupWindow))]
public abstract class InformationPopupWindow : MonoBehaviour
{
    public PopupWindow PopupWindow { private set; get; }

    protected virtual void InitializeInformationWindow()
    {
        PopupWindow = GetComponent<PopupWindow>();
    }

    public virtual void HideWindow()
    {
        PopupWindow.Hide();
    }
}