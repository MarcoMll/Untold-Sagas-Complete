using System;
using System.Collections;
using UnityEngine;
using CustomInspector;
using CustomUtilities;

public class InformationWindowController : MonoBehaviour
{
    [SerializeField] private Vector2 windowOffset = new Vector2(200f, 200f);
    [SerializeField] private Canvas canvas;
    [Header("Delay")]
    [SerializeField] private ProgressBar delayBarPrefab;
    [SerializeField, Range(0f, 1f)] private float windowSpawnDelay = 0.5f;
    [HorizontalLine("Window Prefabs", color: FixedColor.Gray)]
    [SerializeField] private CharacterInformationPopupWindow characterWindowPrefab;
    [SerializeField] private ItemInformationPopupWindow itemWindowPrefab;

    private InformationPopupWindow _currentKeywordInformationWindow = null;
    private string _currentKeyword = String.Empty;

    private Coroutine _windowDelayCoroutine;
    private ProgressBar _temporaryWindowDelayBar;
    
    public static InformationWindowController Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator DelayedWindowSpawnCoroutine(TooltipData tooltipData)
    {
        _temporaryWindowDelayBar = Instantiate(delayBarPrefab, canvas.transform);
        _temporaryWindowDelayBar.SetTargetValue(1f, windowSpawnDelay);
        
        yield return new WaitForSeconds(windowSpawnDelay);
        
        SpawnWindowByTooltipData(tooltipData);
        StopDelayedWindowSpawn();
    }

    private void StopDelayedWindowSpawn()
    {
        if (_windowDelayCoroutine != null)
        {
            StopCoroutine(_windowDelayCoroutine);
            _windowDelayCoroutine = null;
        }

        if (_temporaryWindowDelayBar != null)
        {
            _temporaryWindowDelayBar.DestroyItself();
            _temporaryWindowDelayBar = null;
        }
    }
    
    private void SpawnWindowWithDelay(TooltipData tooltipData)
    {
        StopDelayedWindowSpawn();
        _windowDelayCoroutine = StartCoroutine(DelayedWindowSpawnCoroutine(tooltipData));
    }
    
    public CharacterInformationPopupWindow SpawnCharacterInformationWindow(Character targetCharacter, Vector2 newWindowPosition)
    {
        var characterWindow = Instantiate(characterWindowPrefab, canvas.transform);
        characterWindow.transform.position = newWindowPosition;
        characterWindow.ShowCharacterWindow(targetCharacter);

        return characterWindow;
    }

    public ItemInformationPopupWindow SpawnItemInformationWindow(Item targetItem, Vector2 newWindowPosition)
    {
        var itemWindow = Instantiate(itemWindowPrefab, canvas.transform);
        itemWindow.transform.position = newWindowPosition;
        itemWindow.ShowItemWindow(targetItem);

        return itemWindow;
    }
    
    private Vector2 AdjustPosition(RectTransform windowRect, Vector2 initialPosition)
    {
        const float referenceWidth = 1920f;
        const float referenceHeight = 1080f;

        float canvasScaleFactor = canvas.scaleFactor;

        float canvasWidth = referenceWidth * canvasScaleFactor;
        float canvasHeight = referenceHeight * canvasScaleFactor;

        var rect = windowRect.rect;
        var windowWidth = rect.width * canvasScaleFactor;
        var windowHeight = rect.height * canvasScaleFactor;
    
        var adjustedX = initialPosition.x;
        var adjustedY = initialPosition.y;

        // Apply offset based on position relative to the screen center
        if (initialPosition.x < canvasWidth / 2)
        {
            adjustedX += windowOffset.x * canvasScaleFactor;
        }
        else
        {
            adjustedX -= windowOffset.x * canvasScaleFactor;
        }

        if (initialPosition.y < canvasHeight / 2)
        {
            adjustedY += windowOffset.y * canvasScaleFactor;
        }
        else
        {
            adjustedY -= windowOffset.y * canvasScaleFactor;
        }
    
        // Adjust X position to ensure it stays within the canvas boundaries
        if (adjustedX - windowWidth / 2 < 0)
        {
            adjustedX = windowWidth / 2;
        }
        else if (adjustedX + windowWidth / 2 > canvasWidth)
        {
            adjustedX = canvasWidth - windowWidth / 2;
        }
    
        // Adjust Y position to ensure it stays within the canvas boundaries
        if (adjustedY - windowHeight / 2 < 0)
        {
            adjustedY = windowHeight / 2;
        }
        else if (adjustedY + windowHeight / 2 > canvasHeight)
        {
            adjustedY = canvasHeight - windowHeight / 2;
        }

        return new Vector2(adjustedX, adjustedY);
    }
    
    private void SpawnWindowByTooltipData(TooltipData tooltipData)
    {
        StopDelayedWindowSpawn();
        
        switch (tooltipData.TooltipType)
        {
            case TooltipData.TooltipDataType.Character:
                var characterWindowRect = characterWindowPrefab.GetComponent<RectTransform>();
                _currentKeywordInformationWindow = SpawnCharacterInformationWindow(tooltipData.Character, AdjustPosition(characterWindowRect, Input.mousePosition));
                break;
            case TooltipData.TooltipDataType.Item:
                var itemWindowRect = itemWindowPrefab.GetComponent<RectTransform>();
                _currentKeywordInformationWindow = SpawnItemInformationWindow(tooltipData.Item, AdjustPosition(itemWindowRect, Input.mousePosition));
                break;
            case TooltipData.TooltipDataType.Information:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        tooltipData.Shown = true;
    }

    private void DestroyCurrentKeywordWindow()
    {
        _currentKeyword = string.Empty;
        
        if (_currentKeywordInformationWindow == null) return;
        Destroy(_currentKeywordInformationWindow.gameObject);
    }
    
    public void FindKeyword(string targetKeyword)
    {
        if (targetKeyword == string.Empty)
        {
            DestroyCurrentKeywordWindow();
            StopDelayedWindowSpawn();
            return;
        }
        
        if (_currentKeyword.Equals(targetKeyword, StringComparison.OrdinalIgnoreCase) == true) return;
        
        DestroyCurrentKeywordWindow();
        
        foreach (var tooltipData in TooltipsHolder.Instance.Tooltips)
        {
            foreach (var keyword in tooltipData.Keywords)
            {
                if (targetKeyword.Equals(keyword, StringComparison.OrdinalIgnoreCase) == false) continue;
                
                _currentKeyword = targetKeyword;

                if (tooltipData.Shown)
                {
                    SpawnWindowWithDelay(tooltipData);
                    return;
                }
                    
                SpawnWindowByTooltipData(tooltipData);
            }
        }
    }
}