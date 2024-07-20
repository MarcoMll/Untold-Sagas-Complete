using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CustomInspector;
using CustomUtilities;

public class MinigameNotification : MonoBehaviour
{
    [SerializeField] private CanvasGroupController canvasGroupController;
    [HorizontalLine("Success Panel")]
    [SerializeField] private ObjectMovementController successPanel;
    [SerializeField] private TextVisualizer textArea;
    [SerializeField] private ImageVisualizer icon;
    [HorizontalLine("Reward Panel")]
    [SerializeField] private CanvasGroupController rewardPanel;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TMP_Text rewardNameField;

    private string _message = String.Empty;
    private Color _textColor = Color.clear;
    private Color _iconColor = Color.clear;
    private string _rewardName = String.Empty;
    private Sprite _rewardSprite = null;
    private bool _hasReward = false;
    
    public void Setup(string message, Color textColor, Color iconColor)
    {
        _message = message;
        _textColor = textColor;
        _iconColor = iconColor;
    }

    public void SetCharacteristicReward(string rewardName, Sprite rewardSprite)
    {
        _rewardName = rewardName;
        _rewardSprite = rewardSprite;
        _hasReward = true;
    }
    
    public void Initialize()
    {
        canvasGroupController.SmoothlyChangeAlpha(1f, 0.3f);
        textArea.WriteText(_message);
        textArea.Text.color = _textColor;
        icon.SetTargetColor(_iconColor);
        icon.ChangeImageColorToTarget(0.5f);
        if (_hasReward)
        {
            TimeUtility.CallWithDelay(0.6f, () => successPanel.MoveToTarget(0.5f));
            TimeUtility.CallWithDelay(1.2f, () => rewardPanel.SmoothlyChangeAlpha(1f, 0.5f));
            rewardIcon.sprite = _rewardSprite;
            rewardNameField.text = _rewardName;
        }
    }
}