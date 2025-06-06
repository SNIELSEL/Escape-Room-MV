using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [NonSerialized]
    public bool gameStarted;

    [NonSerialized]
    public bool gameEnded;

    public Image fadeInImage;
    public Volume postProccesing;
    private Vignette vignette;

    public int gameTimeMinutes;

    public float gameTimeSeconds;

    public TextMeshProUGUI startButtonText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;

    public void Update()
    {
        if (gameTimeMinutes <= 0 && gameTimeSeconds <= 0)
        {
            gameEnded = true;

            if (postProccesing.profile.TryGet(out vignette))
            {
                Debug.Log("GameOver");
                vignette.intensity.value += Time.deltaTime * 0.8f;
                vignette.intensity.value = Mathf.Clamp01(vignette.intensity.value);
                fadeInImage.color += new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b,Time.deltaTime * 0.8f);
            }

            if(fadeInImage.color.a >= 1)
            {
                Debug.Log("GameOver Text");
                gameOverText.color += new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, Time.deltaTime * 0.8f);
            }

            if (gameOverText.color.a >= 1)
            {
                Invoke("StartGame", 5f);
            }
        }

        if (gameTimeSeconds <= 0 && !gameEnded)
        {
            gameTimeMinutes--;
            gameTimeSeconds = 60;
        }

        if (gameStarted && !gameEnded)
        {
            gameTimeSeconds -= Time.deltaTime;
            timerText.text = ($"{gameTimeMinutes.ToString()}.{gameTimeSeconds.ToString("F0")}");
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            startButtonText.text = "Restart";
            gameStarted = true;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application quit");
    }
}
