using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CookingMinigameLogic
{
    [RequireComponent(typeof(ItemContainer))]
    [RequireComponent(typeof(DraggableObject))]
    
    public class Ingredient : MonoBehaviour
    {
        [SerializeField] private GameObject initial;
        [SerializeField] private GameObject sliced;
        [SerializeField] private GameObject cooked;
        [SerializeField] private GameObject initialCooked;

        private IngredientState _state = IngredientState.Default;

        public IngredientState State => _state;
        public bool Interactable { get; set; } = true;

        private void ChangeIngredientAppearance(IngredientState oldState, IngredientState newState)
        {
            switch (newState)
            {
                case IngredientState.Default:
                    initial.SetActive(true);
                    sliced.SetActive(false);
                    cooked.SetActive(false);
                    initialCooked.SetActive(false);
                    break;
                case IngredientState.Sliced:
                    initial.SetActive(false);
                    sliced.SetActive(true);
                    cooked.SetActive(false);
                    initialCooked.SetActive(false);
                    break;
                case IngredientState.Cooked:
                    initial.SetActive(false);
                    sliced.SetActive(false);
                    if (oldState == IngredientState.Sliced)
                    {
                        initialCooked.SetActive(false);
                        cooked.SetActive(true);
                    }
                    else
                    {
                        initialCooked.SetActive(true);
                        cooked.SetActive(false);
                    }

                    break;
            }
        }

        public void SetIngredientState(IngredientState newState)
        {
            ChangeIngredientAppearance(_state, newState);
            _state = newState;
        }
    }

    public enum IngredientState
    {
        Default,
        Sliced,
        Cooked
    }
}