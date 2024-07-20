using UnityEngine;
using System;

[Serializable]
public class ButtonAppearance
{
    [SerializeField] private Sprite nameFieldSprite;
    [SerializeField] private Sprite frameSprite;
    [SerializeField] private Sprite backgroundSprite;
        
    public Sprite NameFieldSprite => nameFieldSprite;
    public Sprite FrameSprite => frameSprite;
    public Sprite BackgroundSprite => backgroundSprite;
}
