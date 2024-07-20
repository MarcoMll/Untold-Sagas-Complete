using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField] private List<CanvasGroupController> uiList;

    public static UserInterfaceController Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
    }

    public void HideAll()
    {
        foreach (var uiCanvasGroup in uiList)
        {
            uiCanvasGroup.SmoothlyChangeAlpha(0f, 0.5f);     
        }
    }

    public void ShowAll()
    {
        foreach (var uiCanvasGroup in uiList)
        {
            uiCanvasGroup.SmoothlyChangeAlpha(1f, 0.5f);   
        }
    }
}