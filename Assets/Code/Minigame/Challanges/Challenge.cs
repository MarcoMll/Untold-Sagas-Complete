using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Challenge : MonoBehaviour
{
    [SerializeField] private TextVisualizer challengeTitle;
    [SerializeField] private TMP_Text timerText;
    
    private float _timeLeft = 0f;
    //private MinigameStaticData.DifficultyAdjuster[] _difficultyAdjusters;
    
    public Action OnChallengeComplete;
    public Action OnChallengeFail;
    
    //protected MinigameStaticData.DifficultyAdjuster[] DifficultyAdjusters => _difficultyAdjusters;
    
    private IEnumerator Countdown()
    {
        while (_timeLeft > 0f)
        {
            _timeLeft -= 1f * Time.deltaTime;
            UpdateTimerUI();
            yield return new WaitForEndOfFrame();
        }
        
        StopCountdown();
        Stop();
    }
    
    private void UpdateTimerUI()
    {
        timerText.text = $"{_timeLeft:F2} секунд осталось";
    }
    
    public abstract void Launch(string objective);
    public abstract void Stop();

    protected void StartCountdown()
    {
        StartCoroutine((Countdown()));
    }

    protected void StopCountdown()
    {
        StopCoroutine((Countdown()));
    }

    protected void WriteTitle(string title)
    {
        challengeTitle.WriteText(title);   
    }
    
    protected void VisualizeDifficultyAdjuster(Characteristic characteristic, float modificationAmount,
        float animationDuration = 0.3f)
    {
        //difficultyAdjustersGrid.SpawnBonus(characteristic, modificationAmount, animationDuration);
    }
    
    public void SetChallengeDifficultyAdjusters()
    {
        //_difficultyAdjusters = difficultyAdjusters;
    }

    public void SetChallengeDuration(float duration)
    {
        _timeLeft = duration;
    }
}