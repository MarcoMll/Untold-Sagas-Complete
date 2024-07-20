using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;

[RequireComponent(typeof(ScaleController))]
[RequireComponent(typeof(RotationController))]
public class CardTransformHandler : MonoBehaviour
{
    [Header("Card sides")] 
    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject inferiorSide;
    [Header("Animation")]
    [SerializeField] private Vector2 highlightedScale = new Vector2(1.07f, 1.07f);
    [SerializeField] private float scaleAnimationDuration = 0.2f;
    [SerializeField] private float rotationAnimationDuration = 0.45f;
    
    private Vector2 _initialScale = Vector2.zero;
    private readonly Vector3 _shownRotation = Vector3.zero;
    private readonly Vector3 _hiddenRotation = new Vector3(0f, 90f, 0f);
    
    private ScaleController _cardScaleController;
    private RotationController _cardRotationController;
    private DecisionCard _decisionCard;
    
    public bool Selected { get; private set; }

    private void Awake()
    {
        _cardScaleController = GetComponent<ScaleController>();
        _cardRotationController = GetComponent<RotationController>();
        _decisionCard = GetComponent<DecisionCard>();
        
        var cardTransform = transform;
        _initialScale = cardTransform.localScale;
    }
    
    public void ScaleUp()
    {
        _cardScaleController.ChangeScale(highlightedScale, scaleAnimationDuration);
    }

    public void ScaleDown()
    {
        _cardScaleController.ChangeScale(_initialScale, scaleAnimationDuration);
    }

    private void ChangeCardSide(GameObject targetCardSide)
    {
        frontSide.SetActive(false);
        inferiorSide.SetActive(false);
        targetCardSide.SetActive(true);
    }
    
    private IEnumerator SelectCardRoutine()
    {
        _cardRotationController.Rotate(_hiddenRotation, rotationAnimationDuration);
        yield return new WaitForSeconds(rotationAnimationDuration);
        Selected = true;
        ChangeCardSide(inferiorSide);
        _cardRotationController.Rotate(_shownRotation, rotationAnimationDuration);
    }
    
    public void ShowCard()
    {
        ChangeCardSide(frontSide);
        _cardRotationController.Rotate(_shownRotation, rotationAnimationDuration);
    }
    
    public void HideCard(bool playEvent = false)
    {
        _cardRotationController.Rotate(_hiddenRotation, rotationAnimationDuration);
        if (playEvent) TimeUtility.CallWithDelay(rotationAnimationDuration, _decisionCard.PlayEvent);
    }

    public void SelectCard()
    {
        StartCoroutine(SelectCardRoutine());
    }
}
