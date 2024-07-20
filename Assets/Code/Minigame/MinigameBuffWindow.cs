using MinigameUtilities;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacteristicSetup))]
[RequireComponent(typeof(PopupWindow))]
public class MinigameBuffWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text buffEffectTextField;
    [SerializeField] private TMP_Text buffModifierAmountTextField;
    
    private CharacteristicSetup _characteristicSetup;
    private PopupWindow _popupWindow;

    private void Awake()
    {
        _characteristicSetup = GetComponent<CharacteristicSetup>();
        _popupWindow = GetComponent<PopupWindow>();
    }
    
    public void ShowBuffWindow(MinigameBuff minigameBuff)
    {
        _characteristicSetup.SetupCharacteristic(minigameBuff.RequiredCharacteristic);
        buffEffectTextField.text = minigameBuff.BuffEffectDescription;
        buffModifierAmountTextField.text = minigameBuff.GetBuffModifierText();
        
        _popupWindow.Show();
    }

    public void HideWindow()
    {
        _popupWindow.Hide();
    }
}