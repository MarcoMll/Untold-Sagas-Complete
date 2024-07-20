using UnityEngine;

public class MouseRelativeBackground : MonoBehaviour
{
    [SerializeField] private float movementMultiplier = 0.05f;
    private Vector2 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        Vector2 mousePositionNormalized = (mousePosition / screenSize) * 2f - Vector2.one;

        transform.position = new Vector3(_initialPosition.x + mousePositionNormalized.x * movementMultiplier,
            _initialPosition.y + mousePositionNormalized.y * movementMultiplier,
            transform.position.z);
    }
}