using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScalableTextField : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private float minFontSize = 20f;
    [SerializeField] private float maxFontSize = 23;
    [SerializeField] private float minHeight = 200f;
    [SerializeField] private float maxHeight = 300f;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = textComponent.GetComponent<RectTransform>();
    }

    [ContextMenu("Adjust Size")]
    public void AdjustSize()
    {
        textComponent.enableAutoSizing = false;
        textComponent.fontSize = maxFontSize;

        float preferredHeight = textComponent.preferredHeight;
        if (preferredHeight < minHeight)
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, minHeight);
        }
        else if (preferredHeight > maxHeight)
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, maxHeight);

            textComponent.enableAutoSizing = true;
            textComponent.fontSizeMin = minFontSize;
            textComponent.fontSizeMax = maxFontSize;
        }
        else
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, preferredHeight);
        }
    }
}