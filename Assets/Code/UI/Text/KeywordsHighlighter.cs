using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class KeywordsHighlighter : MonoBehaviour
{
    [SerializeField] private Color disabledTextColor;
    [SerializeField] private Color highlightedTextColor;
    
    private TMP_Text _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
    }

    private void ChangeWordColor(string keyword, Color newColor)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(newColor);
        string text = _textComponent.text;
    
        string pattern = $@"\b{Regex.Escape(keyword)}\b";
        _textComponent.text = Regex.Replace(text, pattern, match => $"<color=#{colorHex}>{match.Value}</color>", RegexOptions.IgnoreCase);
    }


    public void HighlightWords(string[] keywords)
    {
        if (_textComponent == null)
        {
            _textComponent = GetComponent<TMP_Text>();
        }
        
        foreach (var keyword in keywords)
        {
            ChangeWordColor(keyword, highlightedTextColor);
        }
    }
}