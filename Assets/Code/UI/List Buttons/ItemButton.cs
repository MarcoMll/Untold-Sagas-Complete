using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemButton : ListButton
{
    [SerializeField] private TMP_Text itemNameField;
    [SerializeField] private Image itemNameFieldImage;
    [SerializeField] private TMP_Text quantityField;
    [SerializeField] private GameObject equippedIcon;
    [SerializeField] private Image frameImage;
    [SerializeField] private Image buttonBackground;
    [SerializeField] private DynamicButton equipButton;
    
    [Header("On Idle")] 
    [SerializeField] private ButtonAppearance idleButtonAppearance;
    [Header("On Selected")]
    [SerializeField] private ButtonAppearance selectedButtonAppearance;
    [Header("On Deactivated")]
    [SerializeField] private ButtonAppearance deactivatedButtonAppearance;

    private bool _deactivated = false;
    private bool _equipped = false;

    private void ShowEquipButton()
    {
        if (_deactivated || _equipped) return;
        equipButton.Show(0.3f);
    }

    private void HideEquipButton()
    {
        if (_deactivated || _equipped) return;
        equipButton.Hide(0.3f);
    }
    
    private void ChangeButtonAppearance(ButtonAppearance newButtonAppearance)
    {
        if (newButtonAppearance.NameFieldSprite != null) 
            itemNameFieldImage.sprite = newButtonAppearance.NameFieldSprite;
        
        if (newButtonAppearance.FrameSprite != null) 
            frameImage.sprite = newButtonAppearance.FrameSprite;
        
        if (newButtonAppearance.BackgroundSprite != null)
            buttonBackground.sprite = newButtonAppearance.BackgroundSprite;
    }
    
    public void SetName(string itemName)
    {
        itemNameField.text = itemName;
    }

    public void SetQuantity(int quantity)
    {
        quantityField.transform.parent.gameObject.SetActive(true);
        quantityField.text = "x" + quantity;
    }

    public void Deactivate()
    {
        equipButton.gameObject.SetActive(false);
        ChangeButtonAppearance(deactivatedButtonAppearance);
        _deactivated = true;
    }

    public void ActivateEquippedIcon()
    {
        equipButton.gameObject.SetActive(false);
        equippedIcon.SetActive(true);
        _equipped = true;
    }

    public void OnSelect()
    {
        ChangeButtonAppearance(selectedButtonAppearance);
        ShowEquipButton();
    }

    public void OnDeselect()
    {
        if (_deactivated == true)
        {
            ChangeButtonAppearance(deactivatedButtonAppearance);
        }
        else
        {
            ChangeButtonAppearance(idleButtonAppearance);
            HideEquipButton();
        }
    }

    public void AddOnEquipButtonClickAction(UnityAction action)
    {
        equipButton.Button.onClick.AddListener(action);
    }
}