using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageZoom : MonoBehaviour, IScrollHandler
{
    [SerializeField] private float zoomSpeed = 0.2f;
    [SerializeField] private float maxZoom = 5f;
    [SerializeField] private float zoomCooldown = 0.1f;

    private float _smoothZoomTime = 0.2f;
    private float _lastZoomTime = 0f;

    private Vector3 _initialScale;
    private Vector3 _targetScale;
    private Vector2 _currentPivot;
    private Vector2 _targetPivot;

    private RectTransform _rectTransform;

    private Coroutine _zoomCoroutine;
    private Coroutine _pivotCoroutine;

    private bool _interactable = true;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        SetInitialValues();
    }

    private void SetInitialValues()
    {
        _initialScale = transform.localScale;
        _targetScale = _initialScale;
        _currentPivot = _rectTransform.pivot;
    }

    private IEnumerator SmoothZoom(Vector3 startScale, Vector3 endScale)
    {
        float timeElapsed = 0;

        while (timeElapsed < _smoothZoomTime)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, timeElapsed / _smoothZoomTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
    }

    private IEnumerator SmoothPivotChange(Vector2 startPivot, Vector2 endPivot)
    {
        float timeElapsed = 0;

        while (timeElapsed < _smoothZoomTime)
        {
            _rectTransform.pivot = Vector2.Lerp(startPivot, endPivot, timeElapsed / _smoothZoomTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _rectTransform.pivot = endPivot;
        _currentPivot = endPivot;
    }

    private Vector2 GetPositionRelativeToImage(Vector2 position, Camera camera = null)
    {
        Vector2 localPosition;

        Canvas canvas = _rectTransform.GetComponentInParent<Canvas>();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, position, camera, out localPosition))
        {
            // Correct for canvas scaling
            localPosition.x /= canvas.scaleFactor;
            localPosition.y /= canvas.scaleFactor;

            float normalizedX = Mathf.Clamp01((localPosition.x - _rectTransform.rect.xMin) / _rectTransform.rect.width);
            float normalizedY = Mathf.Clamp01((localPosition.y - _rectTransform.rect.yMin) / _rectTransform.rect.height);

            return new Vector2(normalizedX, normalizedY);
        }
        return Vector2.zero;
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(_initialScale, desiredScale);
        desiredScale = Vector3.Min(_initialScale * maxZoom, desiredScale);
        return desiredScale;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (Time.time - _lastZoomTime < zoomCooldown || !_interactable) return;
        _lastZoomTime = Time.time;

        if (_targetScale == _initialScale * maxZoom && eventData.scrollDelta.y > 0) return;

        Vector3 delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        Vector3 desiredScale = _targetScale + delta;

        _targetScale = ClampDesiredScale(desiredScale);

        _targetPivot = GetPositionRelativeToImage(Input.mousePosition);

        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        _zoomCoroutine = StartCoroutine(SmoothZoom(transform.localScale, _targetScale));

        if (_pivotCoroutine != null) StopCoroutine(_pivotCoroutine);
        _pivotCoroutine = StartCoroutine(SmoothPivotChange(_currentPivot, _targetPivot));
    }

    public void ZoomOnObject(Transform targetTransform, float zoomValue = 5, bool interactable = true)
    {
        SetInteractable(interactable);

        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTransform.position);
        _targetPivot = GetPositionRelativeToImage(screenPosition, Camera.main);

        Debug.Log("Object relative to image position: " + _targetPivot);
        Debug.Log("Mouse relative to image position: " + GetPositionRelativeToImage(Input.mousePosition));

        _targetScale = _initialScale * zoomValue; 

        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        _zoomCoroutine = StartCoroutine(SmoothZoom(transform.localScale, _targetScale));

        if (_pivotCoroutine != null) StopCoroutine(_pivotCoroutine);
        _pivotCoroutine = StartCoroutine(SmoothPivotChange(_currentPivot, _targetPivot));
    }

    public void SetInteractable(bool value)
    {
        _interactable = value;
    }
}