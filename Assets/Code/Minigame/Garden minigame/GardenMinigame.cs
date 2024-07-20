using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using TMPro;

[RequireComponent(typeof(MouseDragInteractionController))]
public class GardenMinigame : Minigame
{
    [HorizontalLine("Minigame Setup")]
    [Header("Scene")]
    [SerializeField] private PlantSlot[] slots;
    [SerializeField] private GardenChallenge[] challenges;
    [Header("UI")]
    [SerializeField] private TMP_Text grownPlantsTextField;
    [SerializeField] private PlantNotification plantNotificationPrefab;

    private int _grownPlants = 0;
    
    private IItemContainable _currentTool = null;
    
    private PlantSlot _currentPlantSlot = null;
    private Challenge _currentChallengePrefab = null;
    private GardenChallenge _currentGardenChallenge = null;
    private PlantNotification _plantNotification = null;

    private MouseDragInteractionController _mouseDragInteractionController;
    
    [Serializable]
    public class PlantSlot
    {
        [SerializeField] private Plant plant;
        [SerializeField] private float interactionRadius = 50f;
        [SerializeField] private Vector2 offset = Vector2.zero;
        
        public Plant Plant => plant;
        public float InteractionRadius => interactionRadius;
        public Vector2 CenterOffset => offset;
    }
    
    [Serializable]
    public class GardenChallenge
    {
        [SerializeField, TextArea(2, 4)] private string objective;
        [SerializeField] private float challengeDuration = 10f;
        [SerializeField] private Challenge challengePrefab;
        [SerializeField] private Item item;
        
        public string Objective => objective;
        public float Duration => challengeDuration;
        public Challenge Prefab => challengePrefab;
        public Item Item => item;
    }

    private void Awake()
    {
        _mouseDragInteractionController = GetComponent<MouseDragInteractionController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForToolsUnderMouse();
        }

        if (_currentTool != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                UseGardenTool();
                _currentTool = null;
            }
        }

        if (_plantNotification != null)
        {
            if (_plantNotification.Value <= 0f)
            {
                _currentPlantSlot.Plant.ChangeGrowthSpeed(2f);
                DegradeCurrentPlant();
                DestroyPlantNotification(true);
                Clear();
                StartCoroutine(StartChallengeAfterTime());
            }
        }
        
        UpdateGrownPlantsAmount();
        UpdateUI();
    }

    public override void StartGame()
    {
        StartCoroutine(StartChallengeAfterTime());
    }

    public override void EndGame()
    {
        UpdateGrownPlantsAmount();
        if (_grownPlants >= slots.Length)
        {
            MinigameController.Instance.CloseMinigame(true, Reward(_grownPlants, slots.Length));
            return;
        }
        
        MinigameController.Instance.CloseMinigame(false);
    }
    
    private IEnumerator StartChallengeAfterTime()
    {
        float timeAmount = Random.Range(3f, 10f);
        yield return new WaitForSeconds(timeAmount);
        StartGardenChallenge();
    }
    
    private void DestroyPlantNotification(bool afterAnimation)
    {
        if (_plantNotification == null) return;
        _plantNotification.Freeze();
        _plantNotification.DestroyNotification(afterAnimation);
    }

    private void Clear()
    {
        _currentPlantSlot = null;
        _currentChallengePrefab = null;
        _currentGardenChallenge = null;
    }
    
    private void CheckForToolsUnderMouse()
    {
        List<RaycastResult> results = _mouseDragInteractionController.GetAllObjectsUnderMouse();
        foreach (var result in results)
        {
            IItemContainable itemContainable = result.gameObject.GetComponent<IItemContainable>();
            if (itemContainable != null)
            {
                _currentTool = itemContainable;
                Debug.Log("Tool found!");
                break;
            }   
        }
    }
    
    private void StartGardenChallenge()
    {
        _currentGardenChallenge = PickRandomChallenge();
        _currentPlantSlot = PickRandomPlantSlot();
        if (_currentPlantSlot == null) return;
        
        _currentPlantSlot.Plant.ChangeGrowthSpeed(-2f);
            
        _plantNotification = Instantiate(plantNotificationPrefab, _currentPlantSlot.Plant.NotificationPlaceholder);
        _plantNotification.Setup(_currentGardenChallenge.Item, 5f);
    }

    private void InstantiateChallenge(GardenChallenge gardenChallenge)
    {
        _currentChallengePrefab = Instantiate(gardenChallenge.Prefab, transform);
        if (_currentChallengePrefab is TimingSlider timingSlider)
        {
            timingSlider.SetInterpolationSpeed(100f);
            //timingSlider.SetInitialWinningChance(minigameStaticData.PassPercentage);
        }

        AssignOnChallengeEndEvents();
        
        //_currentChallengePrefab.SetChallengeDifficultyAdjusters(minigameStaticData.ModifiersList);
        _currentChallengePrefab.SetChallengeDuration(_currentGardenChallenge.Duration);
        _currentChallengePrefab.Launch(_currentGardenChallenge.Objective);
    }

    private void AssignOnChallengeEndEvents()
    {
        _currentChallengePrefab.OnChallengeComplete += () => _currentPlantSlot.Plant.ChangeGrowthSpeed(2f);
        _currentChallengePrefab.OnChallengeComplete += () => StartCoroutine(StartChallengeAfterTime());
        _currentChallengePrefab.OnChallengeComplete += UpgradeCurrentPlant;
        _currentChallengePrefab.OnChallengeComplete += Clear;
        
        _currentChallengePrefab.OnChallengeFail += () => _currentPlantSlot.Plant.ChangeGrowthSpeed(2f);
        _currentChallengePrefab.OnChallengeFail += () => StartCoroutine(StartChallengeAfterTime());
        _currentChallengePrefab.OnChallengeFail += DegradeCurrentPlant;
        _currentChallengePrefab.OnChallengeFail += Clear;
    }
    
    private void UpgradeCurrentPlant()
    {
        ModifyPlantGrowth(_currentPlantSlot.Plant.TargetGrowth / 5f);
    }

    private void DegradeCurrentPlant()
    {
        ModifyPlantGrowth(_currentPlantSlot.Plant.TargetGrowth / 5f * -1f);
    }
    
    private void ModifyPlantGrowth(float value)
    {
        if (_currentPlantSlot == null) return;
        _currentPlantSlot.Plant.ChangeCurrentGrowth(value);
    }
    
    private void UseGardenTool()
    {
        if (_currentPlantSlot == null) return;

        Vector2 plantCenter = new Vector2(_currentPlantSlot.Plant.transform.position.x,
            _currentPlantSlot.Plant.transform.position.y);
        
        float distance = Vector2.Distance(plantCenter + _currentPlantSlot.CenterOffset, Input.mousePosition);
        if (distance > _currentPlantSlot.InteractionRadius) return;

        bool rightTool = _currentTool.GetItem() == _currentGardenChallenge.Item;
        DestroyPlantNotification(!rightTool);
        
        if (rightTool)
        {
            InstantiateChallenge(_currentGardenChallenge);
        }
        else
        {
            _currentPlantSlot.Plant.ChangeGrowthSpeed(2f);
            StartCoroutine(StartChallengeAfterTime());
            DegradeCurrentPlant();
            Clear();
        }
    }

    private void UpdateUI()
    {
        grownPlantsTextField.text = $"Растений выращено: {_grownPlants}/{slots.Length}";
    }

    private void UpdateGrownPlantsAmount()
    {
        _grownPlants = 0;
        
        foreach (var slot in slots)
        {
            if (slot.Plant.CurrentGrowth >= slot.Plant.TargetGrowth)
            {
                _grownPlants++;
            }   
        }

        if (_grownPlants >= slots.Length)
        {
            EndGame();
        }
    }
    
    private PlantSlot PickRandomPlantSlot()
    {
        List<PlantSlot> temporarySlots = new List<PlantSlot>();
        foreach (var slot in slots)
        {
            if (slot.Plant.CurrentGrowth < slot.Plant.TargetGrowth)
                temporarySlots.Add(slot);
        }

        if (temporarySlots.Count == 0) return null;
        
        int randomIndex = Random.Range(0, temporarySlots.Count);
        return temporarySlots[randomIndex];
    }

    private GardenChallenge PickRandomChallenge()
    {
        int randomIndex = Random.Range(0, challenges.Length);
        return challenges[randomIndex];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        foreach (PlantSlot slot in slots)
        {
            if (slot.Plant == null || slot.InteractionRadius <= 0f) continue;
            DrawInteractionCircle(slot.Plant.transform.position + new Vector3(slot.CenterOffset.x, slot.CenterOffset.y, 0f), slot.InteractionRadius, 30);       
        }
    }
    
    private void DrawInteractionCircle(Vector3 center, float radius, int segments)
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            // Calculate x and y coordinates of the point
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            Vector3 point = new Vector3(center.x + x, center.y + y, center.z);

            // Draw a line to the next point
            if (i > 0)
            {
                Gizmos.DrawLine(point, center + new Vector3(
                    Mathf.Sin(Mathf.Deg2Rad * (angle - 360f / segments)) * radius, 
                    Mathf.Cos(Mathf.Deg2Rad * (angle - 360f / segments)) * radius, 
                    center.z));
            }

            angle += (360f / segments);
        }
    }
}