using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    [Header("Completed Puzzles")]

    public bool sculptingPuzzleComplete;
    public bool letterPuzzleComplete;
    public bool videoPuzzleComplete;
    public bool paintingPuzzleComplete;
    public bool codeValid;

    [Header("WinPostProcessing")]

    public Image fadeInImage;
    public Volume postProccesing;
    private Vignette vignette;

    [Header("UI")]

    public UIManager UImanager;
    private string[] codes = new string[4];

    public TextMeshProUGUI codeValidationText;
    public TextMeshProUGUI deactivationCode;
    public Button deactivationButton;

    public bool allPuzzlesCompleted()
    {
        if(sculptingPuzzleComplete && letterPuzzleComplete && videoPuzzleComplete && paintingPuzzleComplete) 
        { 
            return true;
        }

        else
        {
            return false;
        }
    }

    public void CheckPuzzleStates()
    {
        if (allPuzzlesCompleted())
        {
            deactivationButton.interactable = true;
            codeValidationText.text = "Code Valid";
        }

        else
        {
            codes[0] = "_";
            codes[1] = "_";
            codes[2] = "_";
            codes[3] = "_";
        }


        if (letterPuzzleComplete)
        {
            codes[0] = "8";
        }

        if (sculptingPuzzleComplete)
        {
            codes[1] = "0";
        }

        if (paintingPuzzleComplete)
        {
            codes[2] = "3";
        }

        if (videoPuzzleComplete)
        {
            codes[3] = "1";
        }

        deactivationCode.text = $"{codes[0]} {codes[1]} {codes[2]} {codes[3]}";
    }

    public void Update()
    {
        CheckPuzzleStates();

        if (UImanager.gameTimeMinutes >= 0 && UImanager.gameTimeSeconds >= 0)
        {
            if (codeValid)
            {
                UImanager.gameEnded = true;

                if (postProccesing.profile.TryGet(out vignette))
                {
                    vignette.intensity.value += Time.deltaTime * 0.8f;
                    vignette.intensity.value = Mathf.Clamp01(vignette.intensity.value);
                    fadeInImage.color += new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, Time.deltaTime * 0.8f);
                }

                if (fadeInImage.color.a >= 1)
                {
                    float alpha = UImanager.winText.color.a;
                    alpha += Time.deltaTime * 0.8f;
                    UImanager.winText.color = new Color(UImanager.winText.color.r, UImanager.winText.color.g, UImanager.winText.color.b, alpha);
                }

                if (UImanager.winText.color.a >= 1)
                {
                    Invoke("WinGame", 5f);
                }
            }
        }
    }

    public void WinGame()
    {
        codeValid = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
