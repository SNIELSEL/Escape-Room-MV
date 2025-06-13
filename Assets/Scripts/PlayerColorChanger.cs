using UnityEngine;

public class PlayerColorChanger : MonoBehaviour
{
    [SerializeField]
    private string hexColorCode;
    [SerializeField]
    private Material colorChanger;
    public float colorChangeAmount = 0.1f; // Amount to change color per enter
    public string paintMaterialName = "PaintMaterial";

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
        {
            //Debug.LogError($"No Renderer found on {gameObject.name}");
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
        float hexColorStepSize = colorChangeAmount / 255f;
        ContainerColor container = other.GetComponent<ContainerColor>();
        if (container != null)
        {
            if (ColorUtility.TryParseHtmlString(container.hexColor, out Color containerColor))
            {
                Color playerColor = colorChanger.color;

                if (containerColor == Color.black)
                {
                    playerColor.r = Mathf.Max(playerColor.r - hexColorStepSize, 0f);
                    playerColor.g = Mathf.Max(playerColor.g - hexColorStepSize, 0f);
                    playerColor.b = Mathf.Max(playerColor.b - hexColorStepSize, 0f);
                }
                else
                {
                    playerColor.r = Mathf.Min(playerColor.r + hexColorStepSize * containerColor.r, 1f);
                    playerColor.g = Mathf.Min(playerColor.g + hexColorStepSize * containerColor.g, 1f);
                    playerColor.b = Mathf.Min(playerColor.b + hexColorStepSize * containerColor.b, 1f);
                }

                hexColorCode = ColorUtility.ToHtmlStringRGB(playerColor);
                //Debug.Log($"Emission Hex Color: #{hexColorCode}");

                colorChanger.color = playerColor;
            }
            // else
            // {
            //     Debug.LogWarning("Invalid hex color: " + container.hexColor);
            // }
        }
    }
}