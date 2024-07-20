using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject brokenHeartPrefab;
    [SerializeField] private Transform heartGrid;

    public void UpdateHeartGrid(int heartsAmount)
    {
        foreach (Transform child in heartGrid.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < heartsAmount; i++)
        {
            Instantiate(heartPrefab, heartGrid);
        }
    }

    public IEnumerator ShowBrokenHearts(int heartsAmount, int heartsLost)
    {
        for (int i = 0; i < heartsLost; i++)
        {
            Transform heartObject = Instantiate(brokenHeartPrefab, heartGrid).transform;

            yield return new WaitForSeconds(.4f);

            Destroy(heartObject.gameObject);
        }

        UpdateHeartGrid(heartsAmount);
    }
}