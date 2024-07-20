using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MousePositionMask : MonoBehaviour
{
    [SerializeField] private float brushRadius = 0.1f; // Radius of the transparency effect
    private Material materialInstance;

    private void Start()
    {
        Image image = GetComponent<Image>();
        if (image.material == null)
        {
            Debug.LogError("Image does not have a material.");
            return;
        }

        CreateMaterialDuplicate(image);
        materialInstance.SetFloat("_Radius", brushRadius);
    }
    
    private void CreateMaterialDuplicate(Image image)
    {
        materialInstance = Instantiate(image.material);
        image.material = materialInstance;
    }
    
    public void FadeArea(Vector2 position)
    {
        materialInstance.SetVector("_TransparencyCenter", new Vector4(position.x, position.y, 0, 0));
    }
}