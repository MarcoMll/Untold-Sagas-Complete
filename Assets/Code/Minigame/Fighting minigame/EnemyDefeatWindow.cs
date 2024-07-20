using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingMinigameLogic
{
    [RequireComponent(typeof(ScaleController))]
    public class EnemyDefeatWindow : MonoBehaviour, ISkippable
    {
        [SerializeField] private TextVisualizer title;
        [SerializeField] private ScaleController victoryImage;
        [SerializeField] private float windowAppearanceAnimationDuration = 0.35f;
        [SerializeField] private float victoryImageAppearanceAnimationDuration = 0.3f;
        [SerializeField] private float showTime = 2f;
        [SerializeField] private bool hideAutomatically = true;
        
        private ScaleController _windowScaleController;
        private Coroutine _animationCoroutine;

        public Action onWindowShown;
        public Action onWindowHidden;
        
        private void Awake()
        {
            _windowScaleController = GetComponent<ScaleController>();
        }

        private void Start()
        {
            ResetWindow();
        }

        private void ResetWindow()
        {
            _windowScaleController.ChangeScale(new Vector2(0f,1f), 0f);
            victoryImage.ChangeScale(Vector2.zero, 0f);
            title.WriteText(string.Empty);
        }
        
        private IEnumerator ShowWindowCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            _windowScaleController.ChangeScale(Vector2.one, windowAppearanceAnimationDuration);
            
            yield return new WaitForSeconds(windowAppearanceAnimationDuration);
            
            title.WriteText("Противник побежден!");
            victoryImage.ChangeScale(Vector2.one, victoryImageAppearanceAnimationDuration);
            onWindowShown?.Invoke();
            
            yield return new WaitForSeconds(showTime + victoryImageAppearanceAnimationDuration);
            
            if (hideAutomatically == true) HideWindow();
        }
        
        private IEnumerator HideWindowCoroutine()
        {
            _windowScaleController.ChangeScale(new Vector2(0f,1f), windowAppearanceAnimationDuration);
            yield return new WaitForSeconds(windowAppearanceAnimationDuration);
            
            onWindowHidden?.Invoke();
            ResetWindow();
        }
        
        private void HideWindow()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            
            _animationCoroutine = StartCoroutine(HideWindowCoroutine());
        }
        
        public void ShowWindow()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            
            _animationCoroutine = StartCoroutine(ShowWindowCoroutine());
        }

        public void OnSkipButtonPressed()
        {
            HideWindow();
        }
    }
}