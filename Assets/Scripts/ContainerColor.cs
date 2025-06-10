using UnityEngine;
using System.Linq; // Added to enable Select method

public class ContainerColor : MonoBehaviour
{
    public string hexColor; // Set in Inspector, e.g., "#FF0000" for red
    [SerializeField]
    private Material verfMaterial;
    public string paintMaterialName = "PaintMaterial"; // Set in Inspector to match paint material name

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError($"No Renderer found on {gameObject.name}");
            return;
        }

        // Find the paint material by name
        foreach (Material mat in rend.materials)
        {
            if (mat.name == paintMaterialName || mat.name == paintMaterialName + " (Instance)")
            {
                verfMaterial = mat;
                break;
            }
        }

        if (verfMaterial == null)
        {
            Debug.LogError($"Material '{paintMaterialName}' not found on {gameObject.name}. Available materials: {string.Join(", ", rend.materials.Select(m => m.name))}");
            return;
        }

        // Apply hexColor to verfMaterial
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            verfMaterial.color = newColor;
        }
        else
        {
            Debug.LogWarning($"Invalid hex color: {hexColor} on {gameObject.name}");
        }
    }
}