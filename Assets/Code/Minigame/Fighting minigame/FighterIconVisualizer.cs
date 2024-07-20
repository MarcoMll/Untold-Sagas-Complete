using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FightingMinigameLogic
{
    [RequireComponent(typeof(CanvasGroupController))]
    public class FighterIconVisualizer : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image characterImage;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image backgroundImage;
        [Header("Appearance")]
        [SerializeField] private ButtonAppearance enabledAppearance;
        [SerializeField] private ButtonAppearance disabledAppearance;
        [Header("Timings")] 
        [SerializeField] private float appearanceDuration = 0.3f;
        
        private Fighter _fighter;
        private CanvasGroupController _canvasGroupController;

        private void Awake()
        {
            _canvasGroupController = GetComponent<CanvasGroupController>();
        }

        private void Start()
        {
            _canvasGroupController.CanvasGroup.alpha = 0f;
        }

        private void ChangeAppearance(ButtonAppearance newAppearance)
        {
            frameImage.sprite = enabledAppearance.FrameSprite;
            backgroundImage.sprite = enabledAppearance.BackgroundSprite;
        }
        
        public void Select()
        {
            ChangeAppearance(enabledAppearance);
        }

        public void Deselect()
        {
            ChangeAppearance(disabledAppearance);
        }
        
        public void Initialize(Fighter newFighter)
        {
            _fighter = newFighter;
            characterImage.sprite = _fighter.Character.Idle;
            _canvasGroupController.SmoothlyChangeAlpha(1f, appearanceDuration);
        }
    }
}