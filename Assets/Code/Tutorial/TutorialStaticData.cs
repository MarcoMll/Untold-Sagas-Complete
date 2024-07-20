using System;
using CustomInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial
{
    [Serializable]
    public class TutorialStaticData
    {
        [SerializeField] private string title;
        [SerializeField, TextArea(5,10)] private string text;
        [SerializeField] private UnityEvent onActivationEvents;
        [SerializeField, Range(0f, 1f)] private float windowXPosition = 0.5f, windowYPosition = 0.5f;
        [SerializeField] private TutorialWindowElement[] windowElements;

        public string TutorialTitle => title;
        public string TutorialText => text;
        public Vector2 TutorialWindowPosition => new Vector2(windowXPosition, windowYPosition);
        public TutorialWindowElement[] WindowElements => windowElements;
        
        public void InvokeTutorialEvents()
        {
            onActivationEvents?.Invoke();
        }
    }

    [Serializable]
    public class TutorialWindowElement
    {
        [SerializeField] private ElementType elementType;
        [ShowIf("TextLabel"), SerializeField, TextArea(3, 6)] 
        private string text;
        [ShowIf("TextLabel"), SerializeField] 
        private bool animated = true;

        [ShowIf("ButtonsGroup"), SerializeField]
        private ButtonsList buttonsList;
        
        private bool TextLabel()
        {
            return elementType == ElementType.TextLabel;
        }

        private bool ButtonsGroup()
        {
            return elementType == ElementType.ButtonsGroup;
        }
        
        public enum ElementType
        {
            TextLabel, SingleImage, ImageGroup, ButtonsGroup
        }

        [Serializable]
        public class ButtonsList
        {
            [SerializeField] private WindowButton[] buttons;
            public WindowButton[] Buttons => buttons;
        }
        
        [Serializable]
        public class WindowButton
        {
            [SerializeField] private string buttonText;
            [SerializeField] private UnityEvent onClickEvents;

            public string Text => buttonText;
            public void InvokeOnClickEvents()
            {
                onClickEvents.Invoke();
            }
        }

        public ElementType Type => elementType;
        public string Text => text;
        public bool Animated => animated;
        public WindowButton[] WindowButtons => buttonsList.Buttons;
    }
}