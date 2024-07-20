using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using CookingMinigameLogic;
using CustomInspector;
using CustomUtilities;

[RequireComponent(typeof(MouseDragInteractionController))]
[RequireComponent(typeof(Countdown))]
public class CookingMinigame : Minigame
{
    [HorizontalLine("Minigame Setup")]
    [SerializeField] private Kitchenware[] kitchenware;
    [SerializeField] private int passPercentage = 50;
    [SerializeField] private int rewardPercentage = 90;
    [Header("UI")]
    [SerializeField] private CompletenessBar completenessBar;
    [SerializeField] private TMP_Text ingredientsLeftField;
    [SerializeField] private TMP_Text countdownTextField;
    [SerializeField] private CanvasGroupController submitButton;

    [Header("Audio")]
    [SerializeField] private AudioClip pickUpSound;
    
    [Header("Ingredients")]
    [SerializeField] private AvailableIngredient[] availableIngredients;
    [SerializeField] private RequiredIngredient[] requiredIngredients;
    
    [Serializable]
    public class AvailableIngredient
    {
        public Transform InitialPoint;
        public Ingredient Ingredient;
    }
    
    [Serializable]
    public class RequiredIngredient
    {
        [SerializeField] private Item ingredientItem;
        [SerializeField] private IngredientState ingredientState;

        public Item RequiredIngredientItem => ingredientItem;
        public IngredientState RequiredIngredientState => ingredientState;
    }
    
    private Ingredient _ingredientOnCuttingBoard;
    private Ingredient _selectedIngredient;

    private float _gainPointsAmount = 0f;
    private float _currentPoints = 0f;
    private float _pointsToPass = 0f;
    private float _pointsToGetReward = 0f;
    
    private int _requiredIngredientIndex = 0;
    private int _usedIngredients = 0;
    
    private MouseDragInteractionController _mouseDragInteractionController;
    private Countdown _countdown;

    private void Awake()
    {
        _countdown = GetComponent<Countdown>();
        _mouseDragInteractionController = GetComponent<MouseDragInteractionController>();
    }
    
    private void Update()
    {
        countdownTextField.text = _countdown.TimeLeftInSeconds + " секунд осталось";
        
        if (Input.GetMouseButtonDown(0))
        {
            CheckForIngredientUnderMouse();
        }

        if (_selectedIngredient != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                InteractWithKitchenware();
                Deselect();   
            }
        }   
    }

    public override void StartGame()
    {
        submitButton.SmoothlyChangeAlpha(0f, .5f);
        SetIngredientsToInitialState();
        CalculatePoints();
        _countdown.StartCountdown();
    }

    public override void EndGame()
    {
        _countdown.StopCountdown();
        
        if (_currentPoints >= _pointsToPass)
        {
            MinigameController.Instance.CloseMinigame(true, Reward(_currentPoints, _pointsToGetReward));
            return;
        }
        
        MinigameController.Instance.CloseMinigame(false);
    }
    
    private void CalculatePoints()
    {
        _gainPointsAmount = 100f / requiredIngredients.Length;
        
        _pointsToPass = MathUtility.CalculatePercentageValue(passPercentage, 100f);
        _pointsToGetReward = MathUtility.CalculatePercentageValue(rewardPercentage, 100f);
        
        completenessBar.UpdateBarValue(_currentPoints, _pointsToPass, _pointsToGetReward);
    }
    
    private void ModifyCompleteness(float value)
    {
        _currentPoints += value;
        completenessBar.UpdateBarValue(_currentPoints, _pointsToPass, _pointsToGetReward);
        
        if (_currentPoints >= _pointsToPass || _usedIngredients >= requiredIngredients.Length)
        {
            submitButton.SmoothlyChangeAlpha(1f, 0.5f);
        }
        else
        {
            submitButton.SmoothlyChangeAlpha(0f, 0.5f);
        }
    }

    private void CalculateCompleteness(IngredientState initialState)
    {
        IItemContainable itemContainable = _selectedIngredient.GetComponent<IItemContainable>();
        if (itemContainable == null) return;
        
        if (_requiredIngredientIndex >= requiredIngredients.Length)
        {
            ModifyCompleteness(-_gainPointsAmount / 2);
            return;
        }
        
        var rightIngredient = requiredIngredients[_requiredIngredientIndex].RequiredIngredientItem == itemContainable.GetItem();
        if (rightIngredient == true)
        {
            ModifyCompleteness(_gainPointsAmount / 2);
            
            var rightIngredientState = requiredIngredients[_requiredIngredientIndex].RequiredIngredientState == initialState;
            if (rightIngredientState)
            {
                ModifyCompleteness(_gainPointsAmount / 2);
            }
        }
        else
        {
            ModifyCompleteness(-_gainPointsAmount / 2);
            return;
        }

        _requiredIngredientIndex++;
    }

    private void InteractWithKitchenware()
    {
        var initialIngredientState = _selectedIngredient.State;

        foreach (var kitchenwareElement in kitchenware)
        {
            kitchenwareElement.InsertIngredient(_selectedIngredient);
        }

        if (_selectedIngredient.State != IngredientState.Cooked) return;
        
        _selectedIngredient.Interactable = false;
        _usedIngredients++;
        
        CalculateCompleteness(initialIngredientState);
        UpdateLeftIngredientsField();
    }

    private void UpdateLeftIngredientsField()
    {
        ingredientsLeftField.text = $"{_usedIngredients}/{requiredIngredients.Length} ингридиентов осталось";
    }
    
    private void Select(Ingredient ingredient)
    {
        if (ingredient.Interactable == false) return;
        
        if (_selectedIngredient != null)
        {
            Deselect();    
        }

        _selectedIngredient = ingredient;
        BringToFront(_selectedIngredient.transform);
        SceneComponentsHolder.Instance.PlaySoundEffect(pickUpSound);
    }

    private void Deselect()
    {
        _selectedIngredient = null;
    }

    private void BringToFront(Transform selectedTransform)
    {
        selectedTransform.SetAsLastSibling();
    }

    private void CheckForIngredientUnderMouse()
    {
        List<RaycastResult> results = _mouseDragInteractionController.GetAllObjectsUnderMouse();
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Ingredient>())
            {
                Select(result.gameObject.GetComponent<Ingredient>());
                break; 
            }
        }
    }

    private void SetIngredientsToInitialState()
    {
        foreach (AvailableIngredient ingredientData in availableIngredients)
        {
            ingredientData.Ingredient.SetIngredientState(IngredientState.Default);
            ingredientData.Ingredient.Interactable = true;
            
            IMoveable moveable = ingredientData.Ingredient.GetComponent<IMoveable>();
            if (moveable != null)
            {
                moveable.Move(ingredientData.InitialPoint.position, 0.3f);
            }
        }
    }
    
    public void ResetMiniGame()
    {
        SetIngredientsToInitialState();

        _usedIngredients = 0;
        _currentPoints = 0f;
        _requiredIngredientIndex = 0;
        
        completenessBar.UpdateBarValue(0, _pointsToPass, _pointsToGetReward);
        submitButton.SmoothlyChangeAlpha(0f, .5f);
        UpdateLeftIngredientsField();
    }
}