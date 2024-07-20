using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomUtilities
{
    public static class ImageUtility
    {
        public static bool ColorsSimilar(Color col1, Color col2, float tolerance = 0.01f)
        {
            return Mathf.Abs(col1.r - col2.r) < tolerance &&
                   Mathf.Abs(col1.g - col2.g) < tolerance &&
                   Mathf.Abs(col1.b - col2.b) < tolerance;
                   //Mathf.Abs(col1.a - col2.a) < tolerance;
        }
        
        public static Texture2D SpriteToTexture2D(Sprite sprite)
        {
            if (sprite == null)
            {
                return null;
            }

            if (sprite.rect.width != sprite.texture.width || sprite.rect.height != sprite.texture.height)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, 
                    (int)sprite.textureRect.y, 
                    (int)sprite.textureRect.width, 
                    (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
    }
}