using System;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

[RequireComponent(typeof(Image))]
public class ImageResizer : MonoBehaviour
{
    [SerializeField] private Vector2 dividingCoefficient = new Vector2(2.029f, 2.048f);
    [SerializeField] private Vector2 targetSize = new Vector2(670f, 1000f);
    
    private Image _image;
    private Sprite _currentSprite;

    private void Start()
    {
        Resize();
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _currentSprite = _image.sprite;
    }

    private void Resize()
    {
        Vector2 spriteSize = new Vector2(_currentSprite.rect.width, _currentSprite.rect.height);
        if (spriteSize.x < targetSize.x)
        {
            spriteSize.x = targetSize.x;
            return;
        }

        Vector2 newSpriteSize = new Vector2(_currentSprite.rect.width / dividingCoefficient.x, 1000f);
        _image.rectTransform.sizeDelta = newSpriteSize;
        
        //(670 - 813) / 2 = -71.5;
        Reposition((targetSize.x - newSpriteSize.x) / 2f);
        Debug.Log($"Expected calculation: (670 - 813) / 2 = -71.5f" +
                  $"\nOutput: ({targetSize.x} - {Mathf.FloorToInt(newSpriteSize.x)}) / 2f = {(targetSize.x - newSpriteSize.x) / 2f}f" +
                  $"\nLeft aligned: {_image.rectTransform.position.x < 1920f / 2f}");
    }

    private void Reposition(float newX)
    {
        bool leftAligned = _image.rectTransform.position.x < 1920f / 2f;
        
        if (leftAligned)
        {
            _image.rectTransform.position = new Vector2(_image.rectTransform.position.x - newX, _image.rectTransform.position.y);
        }
        else
        {
            _image.rectTransform.position = new Vector2(_image.rectTransform.position.x + newX, _image.rectTransform.position.y);
        }
    }
}