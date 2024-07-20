using System;
using CustomUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialSequenceController : MonoBehaviour, ISkippable
    {
        [Header("Tutorial setup")]
        [SerializeField] private CanvasGroupController tutorialBackgroundPanel;
        [SerializeField] private TutorialStaticData[] tutorials;
        [SerializeField] private CanvasGroupController[] tutorialSceneElementsList;
        [SerializeField] private float elementAppearanceDuration = 0.2f;
        
        [Header("Prefabs")]
        [SerializeField] private TutorialWindow tutorialWindow;

        private bool _windowInitialized = false;
        private int _currentTutorialIndex = -1;
        
        public Action onTutorialEnd;
        
        private Vector2 GetScreenPosition(Vector2 normalizedPosition)
        {
            Vector2 screenSize = new Vector2(1920f, 1080f);
            Vector2 windowSize = tutorialWindow.GetComponent<RectTransform>().sizeDelta;

            float screenXPosition = MathUtility.ChangeValueRange(normalizedPosition.x, new Vector2(0f, 1f),
                new Vector2(0f, screenSize.x));
            
            float screenYPosition = MathUtility.ChangeValueRange(normalizedPosition.y, new Vector2(0f, 1f),
                new Vector2(0, screenSize.y));

            if (screenXPosition < windowSize.x / 2)
            {
                screenXPosition = windowSize.x / 2;
            } 
            else if (screenXPosition > 1920f - windowSize.x / 2)
            {
                screenXPosition = 1920f - windowSize.x / 2;
            }
            
            if (screenYPosition < windowSize.y / 2)
            {
                screenYPosition = windowSize.y / 2;
            } 
            else if (screenYPosition > 1080f - windowSize.y / 2)
            {
                screenYPosition = 1080f - windowSize.y / 2;
            }
            
            return new Vector2(screenXPosition, screenYPosition);
        }
        
        private void RedrawWindow(TutorialStaticData tutorialStaticData)
        {
            if (_windowInitialized == false)
            {
                _windowInitialized = true;
                tutorialWindow.ShowWindow();
            }
            
            var newWindowPosition = GetScreenPosition(tutorialStaticData.TutorialWindowPosition);
            
            tutorialWindow.MoveWindow(newWindowPosition);
            tutorialWindow.RedrawWindow(tutorialStaticData);
        }
        
        public void ShowNextWindow()
        {
            _currentTutorialIndex++;
            if (_currentTutorialIndex >= tutorials.Length)
            {
                EndTutorial();
                return;
            }
            
            EventsManager.Instance.SetSkippable(this);

            TutorialStaticData tutorialStaticData = tutorials[_currentTutorialIndex];
            RedrawWindow(tutorialStaticData);
        }

        public void ShowTutorialSceneElement(CanvasGroupController targetElement)
        {
            foreach (var tutorialSceneElement in tutorialSceneElementsList)
            {
                if (tutorialSceneElement == targetElement) continue;
                tutorialSceneElement.SmoothlyChangeAlpha(0f, elementAppearanceDuration);
            }

            targetElement.SmoothlyChangeAlpha(1f, elementAppearanceDuration);
        }

        public void HideAllSceneElements()
        {
            foreach (var tutorialSceneElement in tutorialSceneElementsList)
            {
                tutorialSceneElement.SmoothlyChangeAlpha(0f, elementAppearanceDuration);
            }
        }
        
        public void StartTutorial()
        {
            tutorialBackgroundPanel.SmoothlyChangeAlpha(0.7f, elementAppearanceDuration);
            ShowNextWindow();
        }
        
        public void EndTutorial()
        {
            tutorialBackgroundPanel.SmoothlyChangeAlpha(0f, elementAppearanceDuration);
            
            TimeUtility.CallWithDelay(elementAppearanceDuration, () => Destroy(gameObject));
            TimeUtility.CallWithDelay(elementAppearanceDuration, () => onTutorialEnd?.Invoke());
        }
        
        public void OnSkipButtonPressed()
        {
            ShowNextWindow();
            Debug.Log("Skip button pressed!");
        }
    }
}