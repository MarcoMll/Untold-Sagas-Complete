using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Vector2 offset;

    private void Update()
    {
        SetPosition(); 
    }

    private void SetPosition()
    {
        transform.position = Input.mousePosition + new Vector3(offset.x, offset.y, 0f);
    }
}