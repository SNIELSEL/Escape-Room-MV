using UnityEngine;

public class PlayerColorChanger : MonoBehaviour
{
    public string hexColorCode;
    [SerializeField] private Material colorChanger;
    private float colorStepSize = 25f;
    private float tolerance = 12.5f;
    private const float COLOR_SCALE = 255f;

    void Start()
    {
        Color playerColor = colorChanger.color;
        playerColor.r = 0;
        playerColor.g = 0;
        playerColor.b = 0;
        colorChanger.color = SnapToCleanHex(playerColor);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ContainerColor container))
        {
            if (ColorUtility.TryParseHtmlString(container.hexColor, out Color containerColor))
            {
                Color playerColor = colorChanger.color;
                float stepSize = colorStepSize / COLOR_SCALE;

                if (containerColor == Color.black)
                {
                    playerColor.r = Mathf.Max(playerColor.r - stepSize, 0f);
                    playerColor.g = Mathf.Max(playerColor.g - stepSize, 0f);
                    playerColor.b = Mathf.Max(playerColor.b - stepSize, 0f);
                }
                else
                {
                    playerColor.r = Mathf.Min(playerColor.r + stepSize * containerColor.r, 1f);
                    playerColor.g = Mathf.Min(playerColor.g + stepSize * containerColor.g, 1f);
                    playerColor.b = Mathf.Min(playerColor.b + stepSize * containerColor.b, 1f);
                }

                playerColor = SnapToCleanHex(playerColor);
                colorChanger.color = playerColor;
                hexColorCode = ColorUtility.ToHtmlStringRGB(playerColor);
                Debug.Log($"Snapped Hex Color: #{hexColorCode}");
            }
            else
            {
                Debug.LogWarning($"Invalid hex color: {container.hexColor}");
            }
        }
    }

    private Color SnapToCleanHex(Color color)
    {
        color.r = SnapChannel(color.r);
        color.g = SnapChannel(color.g);
        color.b = SnapChannel(color.b);
        return color;
    }

    private float SnapChannel(float channel)
    {
        float step = colorStepSize / COLOR_SCALE;
        float tol = tolerance / COLOR_SCALE;
        if (Mathf.Abs(channel - 1f) < tol) return 1f;
        if (Mathf.Abs(channel - 0f) < tol) return 0f;
        return Mathf.Round(channel / step) * step;
    }
}