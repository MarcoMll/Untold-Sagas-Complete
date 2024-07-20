using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [Header("Rewarding")]
    [SerializeField] private Characteristic rewardCharacteristic;
    [SerializeField] private Item rewardItem;

    public Characteristic RewardCharacteristic => rewardCharacteristic;
    public Item RewardItem => rewardItem;
    
    public abstract void StartGame();
    public abstract void EndGame();

    protected bool Reward(float currentPoints, float requiredPoints)
    {
        if (rewardCharacteristic == null && rewardItem == null) return false;
        if (currentPoints < requiredPoints) return false;

        if (rewardCharacteristic != null)
        {
            PlayerStats.Instance.AddCharacteristic(rewardCharacteristic);
            SecondaryEventsVisualizer.Instance.VisualizeAddableCharacteristic(rewardCharacteristic);
        }

        if (rewardItem != null)
        {
            PlayerStats.Instance.ItemHandler.AddItem(rewardItem);
        }

        return true;
    }
}