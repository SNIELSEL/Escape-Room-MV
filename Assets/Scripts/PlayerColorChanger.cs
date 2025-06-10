using UnityEngine;

public class PlayerColorChanger : MonoBehaviour
{
    [SerializeField]
    private Material colorChanger;
    public float colorChangeAmount = 0.1f; // Amount to change color per enter
    public string paintMaterialName = "PaintMaterial";

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError($"No Renderer found on {gameObject.name}");
            return;
        }

        foreach (Material mat in rend.materials)
        {
            if (mat.name == paintMaterialName || mat.name == paintMaterialName + " (Instance)")
            {
                colorChanger = mat;
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ContainerColor container = other.GetComponent<ContainerColor>();
        if (container != null)
        {
            if (ColorUtility.TryParseHtmlString(container.hexColor, out Color containerColor))
            {
                Color playerColor = colorChanger.color;

                if (containerColor == Color.black)
                {
                    // Decrease all channels for black container
                    playerColor.r = Mathf.Max(playerColor.r - colorChangeAmount, 0f);
                    playerColor.g = Mathf.Max(playerColor.g - colorChangeAmount, 0f);
                    playerColor.b = Mathf.Max(playerColor.b - colorChangeAmount, 0f);
                }
                else
                {
                    // Increase channels based on container color
                    playerColor.r = Mathf.Min(playerColor.r + colorChangeAmount * containerColor.r, 1f);
                    playerColor.g = Mathf.Min(playerColor.g + colorChangeAmount * containerColor.g, 1f);
                    playerColor.b = Mathf.Min(playerColor.b + colorChangeAmount * containerColor.b, 1f);
                }

                colorChanger.color = playerColor;
            }
            else
            {
                Debug.LogWarning("Invalid hex color: " + container.hexColor);
            }
        }
    }
}