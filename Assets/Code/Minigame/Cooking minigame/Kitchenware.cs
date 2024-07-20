using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using CustomUtilities;

namespace CookingMinigameLogic
{
    public class Kitchenware : MonoBehaviour
    {
        [SerializeField] private float interactionRadius = 230f;
        [SerializeField] private bool acceptsAnyState = true;
        [SerializeField, ShowIfNot("acceptsAnyState"), Indent(-1)] private IngredientState receivableIngredientState;
        [SerializeField] private IngredientState outputableIngredientState;
        [SerializeField] private AudioClip interactionSound;

        private bool IngredientAccepted(Ingredient targetIngredient)
        {
            var distanceToIngredient = Vector2.Distance(targetIngredient.transform.position, transform.position);
            
            if (distanceToIngredient > interactionRadius) return false;
            if (acceptsAnyState == true) return true;
            
            return targetIngredient.State == receivableIngredientState;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            GizmosUtility.DrawCircle(transform.position, interactionRadius, 36);
        }
        
        public void InsertIngredient(Ingredient targetIngredient)
        {
            if (IngredientAccepted(targetIngredient) == false) return;
            targetIngredient.SetIngredientState(outputableIngredientState);
            SceneComponentsHolder.Instance.PlaySoundEffect(interactionSound);
        }
    }
}