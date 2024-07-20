using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TextVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private float typeSmoothness = 0.01f;

    private int _currentChaptersAmount;
    private string _targetText;

    private Coroutine _textWritingCoroutine;
    
    public TMP_Text Text => textField;
    
    public void WriteText(string text)
    {
        if (textField == null)
        {
            Debug.LogError("No text field assigned!");
            return;
        } 
        if (text == string.Empty)
        {
            textField.text = string.Empty;
            return;
        }
        
        _targetText = text;
        
        if (_textWritingCoroutine != null)
        {
            StopCoroutine(_textWritingCoroutine);    
        }
        
        _textWritingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        while (_currentChaptersAmount <= _targetText.Length)
        {
            textField.text = _targetText.Substring(0, _currentChaptersAmount + 1);
            _currentChaptersAmount++;

            if (_currentChaptersAmount == _targetText.Length)
            {
                _currentChaptersAmount = 0;
                yield break;
            }

            yield return new WaitForSeconds(typeSmoothness);
        }
    }
}