using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SecondaryEvent : MonoBehaviour
{
    public int PriorityLevel = 0;
    public bool Separate;

    public void Initialize(List<Transform> interactionsList)
    {
        gameObject.SetActive(false);
        interactionsList.Add(transform);
    }
}