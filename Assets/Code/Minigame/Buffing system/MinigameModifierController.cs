using System;
using System.Collections;
using System.Collections.Generic;
using MinigameUtilities;
using UnityEngine;

namespace MinigameUtilities
{
    public class MinigameModifierController : MonoBehaviour, ISkippable
    {
        [SerializeField] private MinigameBuff[] minigameBuffsList;
        [SerializeField] private MinigameBuffWindow minigameBuffWindow;

        private Minigame _targetMinigame;
        private CoroutineQueue _coroutineQueue;
        
        public bool AllBuffsApplied { get; private set; }
        
        private IEnumerator BuffVisualizationRoutine(MinigameBuff minigameBuff)
        {
            minigameBuffWindow.ShowBuffWindow(minigameBuff);
            minigameBuff.ApplyBuff(_targetMinigame);
            yield return new WaitForSeconds(3f);
            minigameBuffWindow.HideWindow();
            yield return new WaitForSeconds(0.5f);
        }

        private void StopApplyingBuffs()
        {
            AllBuffsApplied = true;
            _targetMinigame.StartGame();
        }
        
        public void StartApplyingBuffs(Minigame targetMinigame)
        {
            _targetMinigame = targetMinigame;

            if (minigameBuffsList.Length == 0)
            {
                StopApplyingBuffs();
                return;
            }
            
            _coroutineQueue = new CoroutineQueue(this);
            _coroutineQueue.onQueueEmpty += StopApplyingBuffs;
            
            foreach (var minigameBuff in minigameBuffsList)
            {
                _coroutineQueue.EnqueueCoroutine(BuffVisualizationRoutine(minigameBuff));
            }
            
            _coroutineQueue.StartQueue();
        }
        
        public void OnSkipButtonPressed()
        {
            _coroutineQueue.StopCurrentCoroutine(startNext: true);
        }
    }
}