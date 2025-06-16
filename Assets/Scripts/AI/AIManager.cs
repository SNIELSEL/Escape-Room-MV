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

    [Header("WinPostProcessing")]
    public Image fadeInImage;
    public Volume postProccesing;
    private Vignette vignette;

    [Header("UI")]
    public UIManager UImanager;
    [SerializeField] public GameObject canvas;


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
            //play voice clip that player needs to return to the control room

            canvas.gameObject.SetActive(true);

            //logig for the override
        }
    }

    public void Update()
    {
        allPuzzlesCompleted();

        if (UImanager.gameTimeMinutes >= 0 && UImanager.gameTimeSeconds >= 0)
        {
            if (allPuzzlesCompleted())
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
        SceneManager.LoadScene("GameScene");
    }
}
