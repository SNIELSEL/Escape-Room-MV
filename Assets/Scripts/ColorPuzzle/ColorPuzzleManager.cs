using System.Linq;
using UnityEngine;

public class MaterialColorManager : MonoBehaviour
{
    [SerializeField] private AIManager puzzleManager;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private PlayerColorChanger playerColorChanger;
    [SerializeField] [Range(1f, 50f)] private float stepSize = 25f; // Adjustable step size (1-50 range)
    [SerializeField] [Range(0f, 25f)] private float tolerance = 12.5f; // Default to half of stepSize, adjustable (0-25 range)

    [SerializeField] private string targetDeletionHex = "#272B6A";
    [SerializeField] private string targetCollegeHex = "#F6821F";

    private string selectedProperty { get; set; }

    private void Start()
    {
        SelectRandomPropertyAndSetColor();
    }

    private void SelectRandomPropertyAndSetColor()
    {
        selectedProperty = "_CollegeColor";
        SetRandomColor(selectedProperty);
        //Debug.Log($"Initial color set: {selectedProperty} to {GetCurrentHexColor(selectedProperty)}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision with{collision.gameObject}");
        if (collision.gameObject.GetComponent<PlayerColorChanger>() == playerColorChanger || collision.gameObject.tag == "playerKwast")
        {
            SetNewColor(selectedProperty, playerColorChanger.hexColorCode);
            Debug.Log($"Collision update: {selectedProperty} changed to {GetCurrentHexColor(selectedProperty)}");
            CheckPuzzleComplete();
        }
    }

    private void SetRandomColor(string property)
    {
        string hexColor = GenerateRandomHexColor();
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            targetMaterial.SetColor(property, newColor);
        }
        else
        {
            Debug.LogError($"Invalid hex color generated: {hexColor}");
        }
    }

    private void SetNewColor(string property, string hexColor)
    {
        if (ColorUtility.TryParseHtmlString("#" + hexColor, out Color newColor))
        {
            targetMaterial.SetColor(property, newColor);
        }
        else
        {
            Debug.LogError($"Invalid hex color provided: {hexColor}");
        }
    }

    public void CheckPuzzleComplete()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned!");
            return;
        }

        Color currentColor = targetMaterial.GetColor(selectedProperty);
        string targetHex = selectedProperty == "_DeletionColor" ? targetDeletionHex : targetCollegeHex;

        if (ColorUtility.TryParseHtmlString(targetHex, out Color targetColor))
        {
            if (AreColorsWithinTolerance(currentColor, targetColor, tolerance / 255f)) // Convert to 0-1 range
            {
                //Debug.Log($"Puzzle complete! {selectedProperty} matches {targetHex}");
                OnPuzzleComplete();
            }
            else
            {
                Debug.Log($"Puzzle not complete. {selectedProperty} is {ColorUtility.ToHtmlStringRGB(currentColor)}, target is {targetHex}");
            }
        }
        else
        {
            Debug.LogWarning("Failed to parse target hex color.");
        }
    }

    private bool AreColorsWithinTolerance(Color a, Color b, float tolerance)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
                Mathf.Abs(a.g - b.g) < tolerance &&
                Mathf.Abs(a.b - b.b) < tolerance;
    }

    private string GenerateRandomHexColor()
    {
        string chars = "0123456789ABCDEF";
        return "#" + new string(Enumerable.Range(0, 6)
            .Select(_ => chars[Random.Range(0, chars.Length)])
            .ToArray());
    }

    private string GetCurrentHexColor(string property)
    {
        Color color = targetMaterial.GetColor(property);
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    private void OnPuzzleComplete()
    {
        puzzleManager.paintingPuzzleComplete = true;
        puzzleManager.CheckPuzzleStates();
    }
}