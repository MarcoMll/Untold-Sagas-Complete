using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MouseInteractionController))]
public class KeywordHoverDetector : MonoBehaviour
{
    private MouseInteractionController _mouseInteractionController;
    private Coroutine _checkMouseHoverCoroutine;
    private Vector3 _lastMousePosition;
    private const float CheckInterval = 0.1f;

    private void Awake()
    {
        _mouseInteractionController = GetComponent<MouseInteractionController>();
    }

    private void OnEnable()
    {
        _checkMouseHoverCoroutine = StartCoroutine(CheckMouseHoverRoutine());
    }

    private void OnDisable()
    {
        if (_checkMouseHoverCoroutine != null)
        {
            StopCoroutine(_checkMouseHoverCoroutine);
            _checkMouseHoverCoroutine = null;
        }
    }

    private IEnumerator CheckMouseHoverRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CheckInterval);
            CheckMouseHover();
        }
    }

    private void CheckMouseHover()
    {
        if (Vector3.Distance(Input.mousePosition, _lastMousePosition) < 1f)
        {
            //Debug.Log($"Returned because the mouse cursor is not moving; mouse movement: {Vector3.Distance(Input.mousePosition, _lastMousePosition)}");
            return;
        }

        _lastMousePosition = Input.mousePosition;

        var results = _mouseInteractionController.GetAllObjectsUnderMouse();
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out IWordHoverable wordHoverable))
            {
                var hoveredWord = wordHoverable.GetHoveredWord(Input.mousePosition);
                InformationWindowController.Instance.FindKeyword(hoveredWord);

                Debug.Log($"Current hovered word: {hoveredWord}");
                return;
            }
        }

        InformationWindowController.Instance.FindKeyword(string.Empty);
        //Debug.Log("IWordHoverable not found!");
    }
}