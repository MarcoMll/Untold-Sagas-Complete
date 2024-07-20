using UnityEngine;

public interface IDraggable
{
    public void Drag(Vector3 mousePosition);
    public void Resize(Vector3 targetScale);
    public void ResizeToInitial();
    public void SetShadowTransparency(float value);
}