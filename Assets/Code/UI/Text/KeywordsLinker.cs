using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class KeywordsLinker : MonoBehaviour, IWordHoverable
{
    private TMP_Text _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
    }

    private string AddLinkToWord(string text, string keyword)
    {
        string pattern = $@"\b{Regex.Escape(keyword)}\b";
        return Regex.Replace(text, pattern, match => string.Format("<link={0}>{1}</link>", keyword, match.Value), RegexOptions.IgnoreCase);
    }

    public void LinkKeywords(string[] keywords)
    {
        if (_textComponent == null)
        {
            _textComponent = GetComponent<TMP_Text>();
        }
        
        string linkedText = _textComponent.text;
        foreach (var keyword in keywords)
        {
            linkedText = AddLinkToWord(linkedText, keyword);
        }
        
        _textComponent.text = linkedText;
    }
    
    public string GetHoveredWord(Vector2 mousePosition)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textComponent, mousePosition, null);
        
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _textComponent.textInfo.linkInfo[linkIndex];
            string link = linkInfo.GetLinkID();

            return link;
        }
        else
        {
            return string.Empty;
        }
    }
}