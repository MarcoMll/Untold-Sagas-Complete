using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomUtilities;

[RequireComponent(typeof(Image))]
public class DynamicImage : MonoBehaviour
{
    [SerializeField] private float transitionSpeed = 1f;
    private Image _image;
    private float _transitionAlpha;

    private Coroutine _currentCoroutine;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _transitionAlpha = _image.color.a;
    }

    private IEnumerator ChangeImage(Sprite newImage, Color newColor)
    {
        // Fade out using _transitionAlpha
        while (_transitionAlpha > 0f)
        {
            _transitionAlpha -= transitionSpeed * Time.deltaTime;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _transitionAlpha);
            yield return null;
        }

        if (newImage == null)
        {
            _image.sprite = null;
            yield break;
        }

        _image.sprite = newImage; // Set the new sprite
        _image.color = new Color(newColor.r, newColor.g, newColor.b, 0);

        // Fade in using _transitionAlpha
        while (_transitionAlpha < 1f)
        {
            _transitionAlpha += transitionSpeed * Time.deltaTime;
            _image.color = new Color(newColor.r, newColor.g, newColor.b, _transitionAlpha);
            yield return null;
        }
    }

    public void SmoothlyChangeImage(Sprite newImage, Color? color = null)
    {
        if (_image == null || _image.gameObject.activeInHierarchy == false) return;
        
        Color finalColor = color ?? Color.white;

        if (newImage != null)
        {
            //Debug.Log($"Old color: {_image.color} \nNew color: {finalColor}");
            //Debug.Log($"\nObserving object: {gameObject.name} \nNew sprite is not null: {newImage != null} \nColors are the same: { ImageUtility.ColorsSimilar(finalColor, _image.color)} \nOld color: {_image.color} \nNew color: {finalColor} \nSprites are the same: {newImage == _image.sprite}");
            if (_image.sprite == newImage && ImageUtility.ColorsSimilar(finalColor, _image.color))
            {
                return;
            }
        }

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        _currentCoroutine = StartCoroutine(ChangeImage(newImage, finalColor));
    }
}