using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInspector;
using CustomUtilities;

[RequireComponent(typeof(Image))]
public class ImageVisualizer : MonoBehaviour
{
    [SerializeField] private VisualizationEffect visualizationEffect;
    [ShowIf("ColorVisualizationEffect"), Indent(-1)]
    [SerializeField] private Color targetColor;
    [ShowIf("DurationIncludedInCalculation"), Indent(-1)]
    [SerializeField] private float duration = 1f;
    
    private enum VisualizationEffect
    {
        Dissolve,
        AlphaChange,
        ChangeColor,
        Fill
    }

    private bool ColorVisualizationEffect()
    {
        return visualizationEffect == VisualizationEffect.ChangeColor;
    }

    private bool DurationIncludedInCalculation()
    {
        if (visualizationEffect == VisualizationEffect.Fill ||
            visualizationEffect == VisualizationEffect.AlphaChange)
        {
            return true;
        }

        return false;
    }
    
    private Image _image;
    private Material _characteristicMaterial;

    public Image Image => _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private IEnumerator ChangeAlphaCoroutine(float targetAlpha, float tempDuration)
    {
        float initialAlpha = _image.color.a;
        float elapsed = 0f;

        while (elapsed < tempDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / tempDuration; // Calculate the interpolation factor
            float currentAlpha = Mathf.Lerp(initialAlpha, targetAlpha, t);

            Color color = _image.color;
            color.a = currentAlpha;
            _image.color = color;

            yield return null;
        }

        // Ensure the alpha is set to the target value at the end
        Color finalColor = _image.color;
        finalColor.a = targetAlpha;
        _image.color = finalColor;
    }


    private IEnumerator ShowImageDisolve()
    {
        float initialDissolveLevel = 1f;
        float dissolveTime = 0f;
        float dissolveSpeed = .5f;

        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float currentDissolveLevel = Mathf.Lerp(initialDissolveLevel, 0, dissolveTime);
            _characteristicMaterial.SetFloat("_Level", currentDissolveLevel);
            yield return null;
        }

        _characteristicMaterial.SetFloat("_Level", 0);
        _characteristicMaterial = null;
    }

    private IEnumerator HideImageDisolve()
    {
        float initialDissolveLevel = 0f;
        float dissolveTime = 0f;
        float dissolveSpeed = .5f;

        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float currentDissolveLevel = Mathf.Lerp(initialDissolveLevel, 1f, dissolveTime);
            _characteristicMaterial.SetFloat("_Level", currentDissolveLevel);
            yield return null;
        }

        _characteristicMaterial.SetFloat("_Level", 1f);
        _characteristicMaterial = null;
    }

    private IEnumerator ChangeColor(float duration)
    {
        float elapsed = 0f;
        Color startColor = _image.color;
        targetColor.a = startColor.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;
            _image.color = Color.Lerp(startColor, targetColor, normalizedTime);
            yield return null;
        }

        _image.color = targetColor;
    }

    private IEnumerator ChangeFillAmount(float targetFillAmount)
    {
        float initialFillAmount = _image.fillAmount;
        float elapsedTime = 0;

        while (Math.Abs(_image.fillAmount - targetFillAmount) > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;  // Normalized time
            _image.fillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, t);
            yield return null;
        }

        _image.fillAmount = targetFillAmount;
    }

    
    public void SetupImage(Sprite sprite)
    {
        _characteristicMaterial = new Material(_image.material);
        _image.material = _characteristicMaterial;

        _image.sprite = sprite;
    }

    public void Show()
    {
        if (_characteristicMaterial == null)
        {
            SetupImage(_image.sprite);
        }

        switch (visualizationEffect)
        {
            case VisualizationEffect.Dissolve:
                StartCoroutine(ShowImageDisolve());
                break;

            case VisualizationEffect.AlphaChange:
                StartCoroutine(ChangeAlphaCoroutine(1f, duration));
                break;
        }
    }

    public void Hide()
    {
        if (_characteristicMaterial == null)
        {
            SetupImage(_image.sprite);
        }

        switch (visualizationEffect)
        {
            case VisualizationEffect.Dissolve:
                StartCoroutine(HideImageDisolve());
                break;

            case VisualizationEffect.AlphaChange:
                StartCoroutine(ChangeAlphaCoroutine(0f, duration));
                break;
        }
    }

    public void ManuallyChangeAlpha(float targetValue)
    {
        StartCoroutine((ChangeAlphaCoroutine(targetValue, duration)));
    }

    public void ChangeImageColorToTarget(float duration)
    {
        StartCoroutine(ChangeColor(duration));
    }

    public void ChangeImageFillAmount(float targetValue)
    {
        StartCoroutine(ChangeFillAmount(targetValue));
    }

    public void SetTargetColor(Color color)
    {
        targetColor = color;
    }

    public void StopAllAnimations()
    {
        StopAllCoroutines();
    }
}