using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    [RequireComponent(typeof(PopupWindow))]
    [RequireComponent(typeof(ObjectMover))]
    public class TutorialWindow : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text titleTextField;
        [SerializeField] private TextVisualizer windowTextField;
        [SerializeField] private Transform windowElementsGrid;
        [Header("Prefabs")]
        [SerializeField] private Transform gridPrefab;
        [SerializeField] private Button windowButtonPrefab;

        private List<GameObject> _windowElementsList = new List<GameObject>();
        
        private PopupWindow _popupWindow;
        private ObjectMover _windowMover;
        
        private void Awake()
        {
            _popupWindow = GetComponent<PopupWindow>();
            _windowMover = GetComponent<ObjectMover>();
        }

        private void DestroyAllWindowElements()
        {
            foreach (var windowElement in _windowElementsList)
            {
                Destroy(windowElement);
            }
            
            _windowElementsList.Clear();
        }
        
        private void SpawnButtonsGrid(TutorialWindowElement.WindowButton[] windowButtons)
        {
            Transform buttonsGrid = Instantiate(gridPrefab, windowElementsGrid);

            foreach (var buttonData in windowButtons)
            {
                Button button = Instantiate(windowButtonPrefab, buttonsGrid);
                TMP_Text buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

                buttonText.text = buttonData.Text;
                button.onClick.AddListener(buttonData.InvokeOnClickEvents);
                button.onClick.AddListener(() => button.interactable = false);
                
                _windowElementsList.Add(button.gameObject);
            }
        }
        
        private void CustomizeWindow(TutorialWindowElement[] windowElements)
        {
            foreach (var windowElement in windowElements)
            {
                switch (windowElement.Type)
                {
                    case TutorialWindowElement.ElementType.ButtonsGroup:
                        SpawnButtonsGrid(windowElement.WindowButtons);
                        break;
                }
            }
        }
        
        public void RedrawWindow(TutorialStaticData tutorialStaticData)
        {
            DestroyAllWindowElements();
            CustomizeWindow(tutorialStaticData.WindowElements);

            titleTextField.text = tutorialStaticData.TutorialTitle;
            windowTextField.WriteText(tutorialStaticData.TutorialText);
            tutorialStaticData.InvokeTutorialEvents();
        }

        public void MoveWindow(Vector2 newPosition)
        {
            var distance = Vector2.Distance(newPosition, transform.position);

            if (distance <= 300f)
            {
                _windowMover.Move(newPosition, 0.2f);
            }
            else
            {
                HideWindow();
                TimeUtility.CallWithDelay(0.2f, () => _windowMover.Move(newPosition, 0f));
                TimeUtility.CallWithDelay(0.2f, ShowWindow);
            }
        }

        public void ShowWindow()
        {
            _popupWindow.Show();
        }
        
        public void HideWindow()
        {
            _popupWindow.Hide();
        }
    }
}