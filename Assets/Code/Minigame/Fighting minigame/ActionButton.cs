using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;  

namespace FightingMinigameLogic
{
    [RequireComponent(typeof(UIInteractionDetector))]
    public class ActionButton : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SkillsCountVisualizer skillsCountVisualizer;
        [SerializeField] private FightController fightController;
        [Header("Audio")]
        [SerializeField] private AudioClip[] onAddSkillClipsList;
        [SerializeField] private AudioClip[] onRemoveSkillClipsList;
        [Header("UI")] 
        [SerializeField] private Image buttonIconImage;
        [SerializeField] private Image buttonBackgroundImage;
        [SerializeField] private ButtonAppearance enabledButtonAppearance;
        [SerializeField] private ButtonAppearance disabledButtonAppearance;

        private int _pointsCount;
        
        private UIInteractionDetector _uiInteractionDetector;

        public int PointsCount => _pointsCount;
        
        private void Awake()
        {
            _uiInteractionDetector = GetComponent<UIInteractionDetector>();
        }

        private void Start()
        {
            SetupButton();
        }

        private void AddPointOnClick()
        {
            if (fightController.Player == null || fightController.Player.SkillPoints <= 0) return;

            skillsCountVisualizer.InstantiateSkillPoint();
            fightController.Player.ChangeSkillPoints(-1);

            if (onAddSkillClipsList != null) SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(onAddSkillClipsList));

            _pointsCount++;
        }

        private void RemovePointOnCLick()
        {
            if (_pointsCount <= 0) return;

            DestroyLastAddedSkillPoint();

            if (onRemoveSkillClipsList != null) SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(onRemoveSkillClipsList));
            
            fightController.Player.ChangeSkillPoints(+1);
            _pointsCount--;
        }

        private void SetupButton()
        {
            _uiInteractionDetector.onLeftClick += AddPointOnClick;
            _uiInteractionDetector.onRightClick += RemovePointOnCLick;
        }

        public void DestroyLastAddedSkillPoint()
        {
            skillsCountVisualizer.DestroyLastAddedSkillPoint();
        }

        public void ResetPoints()
        {
            _pointsCount = 0;
            skillsCountVisualizer.DestroyAllPoints();
        }

        public void ToggleInteractability(bool interactable)
        {
            _uiInteractionDetector.ToggleInteractability(interactable);

            var iconSprite = interactable ? enabledButtonAppearance.FrameSprite : disabledButtonAppearance.FrameSprite;
            var backgroundSprite = interactable ? enabledButtonAppearance.BackgroundSprite : disabledButtonAppearance.BackgroundSprite;

            buttonIconImage.sprite = iconSprite;
            buttonBackgroundImage.sprite = backgroundSprite;
        }
    }
}