using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWordHoverable
{ 
    string GetHoveredWord(Vector2 mousePosition);
}