using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectShaker))]
public class PlantNotification : MonoBehaviour
{
    [SerializeField] private Image toolIcon;
    [SerializeField] private ProgressBar deadlineBar;
    [SerializeField] private CanvasGroupController canvasGroup;

    private ObjectShaker _shaker;
    
    public float Value => deadlineBar.Value;

    private void Awake()
    {
        _shaker = GetComponent<ObjectShaker>();
    }

    public void Setup(Item item, float time)
    {
        toolIcon.sprite = item.Icon;
        deadlineBar.SetCurrentValue(1f);
        deadlineBar.SetTargetValue(0f, time);
        canvasGroup.SmoothlyChangeAlpha(1f, 0.3f);
    }

    public void DestroyNotification(bool afterAnimation = false)
    {
        if (afterAnimation)
        {
            _shaker.Shake();
            TimeUtility.CallWithDelay(0.2f, () => canvasGroup.SmoothlyChangeAlpha(0f, 0.3f));
            TimeUtility.CallWithDelay(0.5f, () => Destroy(gameObject));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Freeze()
    {
        deadlineBar.StopChangingValue();
    }
}