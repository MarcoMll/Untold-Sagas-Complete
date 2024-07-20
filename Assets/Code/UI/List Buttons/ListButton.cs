using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ListButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    
    public void AddOnClickAction(UnityAction action)
    {
        _button.onClick.AddListener(action);  
    }

    public void SetImage(Sprite sprite)
    {
        buttonImage.sprite = sprite;
    }
}