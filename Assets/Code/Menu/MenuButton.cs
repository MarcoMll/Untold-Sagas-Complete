using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;

namespace GameMenu
{
    [RequireComponent(typeof(UIInteractionDetector))]
    [RequireComponent(typeof(AudioSource))]
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private GameObject buttonSelectionObject;
        [SerializeField] private ScaleController textScaleController;
        [SerializeField] private Vector2 selectedTextScale = new Vector2(1.2f, 1.2f);
        [SerializeField] private float selectionDuration = 0.2f;
        [SerializeField] private AudioClip[] selectionClips;
        [SerializeField] private AudioClip onClickSound;

        private AudioSource _buttonAudioSource;

        public UIInteractionDetector UIInteractionDetector { get; private set; }

        private void Awake()
        {
            _buttonAudioSource = GetComponent<AudioSource>();
            UIInteractionDetector = GetComponent<UIInteractionDetector>();
        }

        private void Start()
        {
            Deselect();

            UIInteractionDetector.onMouseEnter += Select;
            UIInteractionDetector.onMouseExit += Deselect;
            UIInteractionDetector.onLeftClick += () => _buttonAudioSource.PlayOneShot(onClickSound);
        }

        private void Select()
        {
            textScaleController.ChangeScale(selectedTextScale, selectionDuration);
            buttonSelectionObject.SetActive(true);

            AudioClip selectionClip = RandomUtility.GetRandomElement(selectionClips);
            _buttonAudioSource.PlayOneShot(selectionClip);
        }

        private void Deselect()
        {
            textScaleController.ChangeScale(Vector2.one, selectionDuration);
            buttonSelectionObject.SetActive(false);
        }
    }
}