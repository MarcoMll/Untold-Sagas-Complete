using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomUtilities
{
    public static class MathUtility
    {
        public static float CalculatePercentageValue(float percent, float value)
        {
            return value * (percent / 100f);
        }

        public static float NormalizeValue(float value, float minValue, float maxValue)
        {
            float normalizedValue = (value - minValue) / (maxValue - minValue);
            Debug.Log($"Input value: {value}, output: {normalizedValue}");
            return normalizedValue;
        }

        public static float ChangeValueRange(float value, Vector2 oldValueRange, Vector2 newValueRange)
        {
            float newValue = (value - oldValueRange.x) / (oldValueRange.y - oldValueRange.x) * (newValueRange.y - newValueRange.x) + newValueRange.x;
            Debug.Log($"Input value: {value}, output: {newValue}");
            return newValue;
        }
    }
}