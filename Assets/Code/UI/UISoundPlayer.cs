using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using CustomUtilities;
using UnityEngine;

[RequireComponent(typeof(UIInteractionDetector))]
public class UISoundPlayer : MonoBehaviour
{
    [Serializable]
    private class UIInteractionType
    {
        [SerializeField] private bool multipleSounds;
        [SerializeField, ShowIfNot("multipleSounds"), Indent(-1)]
        private AudioClip interactionSound;
        [SerializeField, ShowIf("multipleSounds")]
        private AudioClip[] interactionSoundsList;

        public bool MultipleSounds => multipleSounds;
        public AudioClip InteractionSound => interactionSound;
        public AudioClip[] InteractionSoundsList => interactionSoundsList;
    }

    [SerializeField] private UIInteractionType onLeftClick;
    [SerializeField] private UIInteractionType onRightClick;
    [SerializeField] private UIInteractionType onMiddleClick;
    [SerializeField] private UIInteractionType onMouseEnter;
    [SerializeField] private UIInteractionType onMouseExit;

    private UIInteractionDetector _uiInteractionDetector;

    private void Awake()
    {
        _uiInteractionDetector = GetComponent<UIInteractionDetector>();
    }

    private void Start()
    {
        _uiInteractionDetector.onLeftClick += () => PlaySound(onLeftClick);
        _uiInteractionDetector.onRightClick += () => PlaySound(onRightClick);
        _uiInteractionDetector.onMiddleClick += () => PlaySound(onMiddleClick);
        _uiInteractionDetector.onMouseEnter += () => PlaySound(onMouseEnter);
        _uiInteractionDetector.onMouseExit += () => PlaySound(onMouseExit);
    }

    private void PlaySound(UIInteractionType uiInteractionType)
    {
        if (uiInteractionType.InteractionSoundsList.Length == 0 && uiInteractionType.InteractionSound == null) return;

        var audioClip = uiInteractionType.MultipleSounds
            ? RandomUtility.GetRandomElement(uiInteractionType.InteractionSoundsList)
            : uiInteractionType.InteractionSound;

        SceneComponentsHolder.Instance.PlaySoundEffect(audioClip);
    }
}