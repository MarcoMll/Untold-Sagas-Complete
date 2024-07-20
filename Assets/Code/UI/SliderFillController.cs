using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtilities;

public class SliderFillController : MonoBehaviour
{
    [SerializeField] private RectTransform sliderRect;
    [SerializeField] private RectTransform backgroundFill;
    [SerializeField] private RectTransform targetFill;

    private float _sliderWidth = 0f;
    private float _currentTargetFillWidth = 0f;

    private IEnumerator SmoothlyChangeFillWidth(RectTransform fill, float targetWidth, float duration)
    {
        float elapsedTime = 0f;
        float startingWidth = fill.sizeDelta.x; // Initial width of the RectTransform

        if (targetWidth > _sliderWidth) targetWidth = _sliderWidth;
        
        while (elapsedTime < duration)
        {
            // Calculate the new width based on the elapsed time
            float newWidth = Mathf.Lerp(startingWidth, targetWidth, elapsedTime / duration);
            fill.sizeDelta = new Vector2(newWidth, fill.sizeDelta.y); // Apply the new width

            elapsedTime += Time.deltaTime; // Increment the elapsed time
            yield return null; // Wait for the next frame before continuing the loop
        }

        // Ensure the width is set to the target value at the end of the interpolation
        fill.sizeDelta = new Vector2(targetWidth, fill.sizeDelta.y);
    }

    private float GetPercentageValueFromSliderWidth(float percentage)
    {
        if (_sliderWidth <= 0) _sliderWidth = sliderRect.sizeDelta.x;
        return MathUtility.CalculatePercentageValue(percentage, _sliderWidth);
    }
    
    public void Initialize(float initialTargetFillPercentage)
    {
        _sliderWidth = sliderRect.sizeDelta.x;
        
        TimeUtility.CallWithDelay(0.35f, () => ModifyTargetFillWidth(initialTargetFillPercentage));
        StartCoroutine(SmoothlyChangeFillWidth(backgroundFill, _sliderWidth, 0.3f));
    }
    
    public void ModifyTargetFillWidth(float value)
    {
        _currentTargetFillWidth += GetPercentageValueFromSliderWidth(value);
        StartCoroutine(SmoothlyChangeFillWidth(targetFill, _currentTargetFillWidth, 0.3f));
    }
}