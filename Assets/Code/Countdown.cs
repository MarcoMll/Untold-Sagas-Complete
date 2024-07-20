using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private float timeInSeconds;
    [SerializeField] private bool startOnAwake;

    private float _timeLeft;
    private Coroutine _countdownCoroutine;
    
    public Action onTimeEnd;
    public float TimeLeftInSeconds => _timeLeft;
    
    private void Start()
    {
        if (startOnAwake)
        {
            StartCountdown();
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        _timeLeft = timeInSeconds;
        
        while (_timeLeft > 0f)
        {
            yield return null;
            _timeLeft -= Time.deltaTime;
        }
        
        _timeLeft = 0f;
        onTimeEnd?.Invoke();
    }

    public void StartCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCountdown();
        }
        
        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    public void StopCountdown()
    {
        if (_countdownCoroutine == null) return;
        
        StopCoroutine(_countdownCoroutine);
        _countdownCoroutine = null;
    }
}