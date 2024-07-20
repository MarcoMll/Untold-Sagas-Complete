using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private float targetGrowth = 100f;
    [SerializeField] private ImageVisualizer[] plantStages;
    [SerializeField] private ProgressBar growthProgressBar;
    [SerializeField] private Transform notificationPlaceholder;
    
    private float _growthPerStage = 0f;
    private float _currentGrowth = 0f;
    private float _growthSpeed = 1f;
    private bool _growth = true;
    
    public float CurrentGrowth => _currentGrowth;
    public float TargetGrowth => targetGrowth;
    public Transform NotificationPlaceholder => notificationPlaceholder;
    
    private void Start()
    {
        _growthPerStage = targetGrowth / plantStages.Length;
    }

    private void Update()
    {
        growthProgressBar.SetCurrentValue(_currentGrowth);
        
        if (_growth == true && _currentGrowth <= targetGrowth)
        {
            _currentGrowth += _growthSpeed * Time.deltaTime;
        }

        if (_currentGrowth < 0f) _currentGrowth = 0f;
        
        for (int i = 0; i < plantStages.Length; i++)
        {
            if (Math.Abs(_currentGrowth - i * _growthPerStage) < 0.1f)
            {
                ChangePlantStage(i);
            }
        }
    }

    private IEnumerator ChangeGrowthSpeedOverTime(float targetSpeed, float duration)
    {
        yield return new WaitForSeconds(duration);
        _growthSpeed = targetSpeed;
    }

    private IEnumerator ChangeCurrentGrowthValue(float targetValue, float duration = 0.5f)
    {
        float elapsedTime = 0f;
        float startValue = _currentGrowth;
        _growth = false;
        
        while (Math.Abs(_currentGrowth - targetValue) > 0.01f)
        {
            _currentGrowth = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration) break;
            yield return null;
        }

        _growth = true;
        _currentGrowth = targetValue;  
    }

    private void ChangePlantStage(int stageIndex)
    {
        for (int i = 0; i < plantStages.Length; i++)
        {
            plantStages[i].Hide();
        }  
        
        plantStages[stageIndex].Show();
    }
    
    public void ChangeCurrentGrowth(float value)
    {
        StartCoroutine(ChangeCurrentGrowthValue(_currentGrowth + value));
    }
    
    public void ChangeGrowthSpeed(float newSpeed)
    {
        _growthSpeed += newSpeed;
        Debug.Log("New plan's growth speed set to: " + _growthSpeed);
    }

    public void TemporaryChangeGrowthSpeed(float newSpeed, float duration)
    {
        float tempSpeed = _growthSpeed;
        _growthSpeed = newSpeed;
        StartCoroutine(ChangeGrowthSpeedOverTime(tempSpeed, duration));
    }
}