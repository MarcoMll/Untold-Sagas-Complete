using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CustomUtilities;

namespace FightingMinigameLogic
{
    public class StatsVisualizer : MonoBehaviour
    {
        [SerializeField] private ProgressBar colbFiller;
        [SerializeField] private TMP_Text characterHealthPointsField;
        [SerializeField] private TMP_Text characterSkillPointsField;
        [SerializeField] private AudioClip[] glassCrackClips;
        [SerializeField] private GameObject[] glassCracksElements;
        [SerializeField] private GameObject[] bloodElements;

        private float _lastGlassDamage = 0;
        
        private Fighter _fighter;
        private int _maxHealth = 0;

        private void OnDestroy()
        {
            UnsubscribeFromFighter();
        }

        private void UpdateFillerUI(int healthPoints)
        {
            float fillRange = MathUtility.NormalizeValue(healthPoints, 0f, _maxHealth);
            SetFillerAmount(fillRange);
            UpdateAdditionalElements(healthPoints);
        }

        private void UpdateAdditionalElements(int healthPoints)
        {
            DisableAdditionalElements();
            Vector2 healthRange = new Vector2(0, _maxHealth);

            Vector2 glassCracksElementsAmount = new Vector2(0, glassCracksElements.Length);
            float glassDamageLevel = MathUtility.ChangeValueRange(_maxHealth - healthPoints, healthRange, glassCracksElementsAmount);
            
            Vector2 bloodElementsAmount = new Vector2(0, bloodElements.Length);
            float bloodDamageAmount = MathUtility.ChangeValueRange(_maxHealth - healthPoints, healthRange, bloodElementsAmount);

            if (glassDamageLevel > _lastGlassDamage) SceneComponentsHolder.Instance.PlaySoundEffect(RandomUtility.GetRandomElement(glassCrackClips));
            _lastGlassDamage = glassDamageLevel;
            
            for (int i = 0; i < glassDamageLevel; i++)
            {
                glassCracksElements[i].SetActive(true);
            }
            
            for (int i = 0; i < bloodDamageAmount; i++)
            {
                bloodElements[i].SetActive(true);
            }
        }

        private void SetFillerAmount(float targetFill)
        {
            colbFiller.StopChangingValue();
            colbFiller.SetTargetValue(targetFill, 0.3f); 
        }

        private void SetHealthPoints(int newHealthPoints)
        {
            if (newHealthPoints < 0) newHealthPoints = 0;
            if (newHealthPoints > _maxHealth) _maxHealth = newHealthPoints;
            
            characterHealthPointsField.text = newHealthPoints.ToString();
            UpdateFillerUI(newHealthPoints);
        }

        private void SetSkillPoints(int newSkillPoints)
        {
            characterSkillPointsField.text = newSkillPoints.ToString();
        }

        private void SubscribeToFighter(Fighter fighter)
        {
            if (_fighter != null)
            {
                UnsubscribeFromFighter();
            }
            
            _fighter = fighter;
            _fighter.onHealthAmountChanged += SetHealthPoints;
            _fighter.onSkillsAmountChanged += SetSkillPoints;
        }

        private void UnsubscribeFromFighter()
        {
            if (_fighter == null) return;
            _fighter.onHealthAmountChanged -= SetHealthPoints;
            _fighter.onSkillsAmountChanged -= SetSkillPoints;
            _fighter = null;
        }

        private void DisableAdditionalElements()
        {
            for (int i = 0; i < glassCracksElements.Length; i++)
            {
                glassCracksElements[i].SetActive(false);
            }
            
            for (int i = 0; i < bloodElements.Length; i++)
            {
                bloodElements[i].SetActive(false);
            }
        }
        
        private void ClearData()
        {
            _maxHealth = 0;
            UnsubscribeFromFighter();
            DisableAdditionalElements();
        }
        
        public void Initialize(Fighter fighter)
        {
            ClearData();
            SubscribeToFighter(fighter);
            
            _maxHealth = fighter.HealthPoints;
            int skillPoints = fighter.SkillPoints;
            SetHealthPoints(_maxHealth);
            SetSkillPoints(skillPoints);
        }
    }
}