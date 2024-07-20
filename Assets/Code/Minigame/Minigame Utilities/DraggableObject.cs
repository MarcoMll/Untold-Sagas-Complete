using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour, IDraggable
{
    [SerializeField] private ImageVisualizer shadow;
    private Vector3 _initialScale = Vector3.zero;
    
    private void Awake()
    {
        _initialScale = transform.localScale;
    }
    
    public void Drag(Vector3 mousePosition)
    {
        transform.position = mousePosition;
    }
    
    private IEnumerator ChangeScale(Vector3 targetScale)
    {
        float duration = 0.2f; // Duration of the scale change, for example, 0.5 seconds
        float elapsed = 0f;

        Vector3 startScale = transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale; // Ensure target scale is set precisely
    }
    
    public void Resize(Vector3 targetScale)
    {
        StartCoroutine(ChangeScale(targetScale));
    }
    
    public void ResizeToInitial()
    {
        StartCoroutine(ChangeScale(_initialScale));
    }
    
    public void SetShadowTransparency(float value)
    {
        if (shadow == null || shadow.gameObject.activeSelf == false || shadow.transform.parent.gameObject.activeSelf == false) return;
        shadow.ManuallyChangeAlpha(value);
    }
}