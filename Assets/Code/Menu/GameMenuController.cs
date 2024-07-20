using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMenu
{
    public class GameMenuController : MonoBehaviour
    {
        [Header("Buttons")] 
        [SerializeField] private MenuButton newGameButton;
        [SerializeField] private MenuButton loadGameButton;
        [SerializeField] private MenuButton quitGameButton;

        [Header("Menu panels setup")] 
        [SerializeField] private MenuPanel startPanel;
        [SerializeField] private float animationDuration;
        [SerializeField] private Transform showPosition, hidePosition;

        [Header("Scene Management")] 
        [SerializeField] private SceneLoader sceneLoader;
        
        private MenuPanel _currentMenuPanel;

        private void Start()
        {
            SwitchCurrentMenuPanel(startPanel);
            
            newGameButton.UIInteractionDetector.onLeftClick += StartNewGame;
            loadGameButton.UIInteractionDetector.onLeftClick += LoadGame;
            quitGameButton.UIInteractionDetector.onLeftClick += QuitApplication;
        }

        private void StartNewGame()
        {
            DataPersistenceManager.Instance.CreateNewGameData();
            ShowSceneLoader();
        }

        private void LoadGame()
        {
            ShowSceneLoader();
        }

        private void QuitApplication()
        {
            Application.Quit();
        }
        
        private void ShowSceneLoader()
        {
            HideCurrentPanel();
            sceneLoader.Show();
            sceneLoader.LoadGameScene();
        }
        
        private void HideCurrentPanel()
        {
            if (_currentMenuPanel != null)
                _currentMenuPanel.Hide();
        }
        
        public void SwitchCurrentMenuPanel(MenuPanel targetMenuPanel)
        {
            if (_currentMenuPanel == targetMenuPanel) return;
            
            HideCurrentPanel();
            
            if (targetMenuPanel.IsInitialized() == false)
            {
                targetMenuPanel.Setup(showPosition, hidePosition, animationDuration);
            }
            
            _currentMenuPanel = targetMenuPanel;
            _currentMenuPanel.Show();
        }
    }
}