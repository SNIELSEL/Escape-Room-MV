using UnityEngine;
using UnityEngine.SceneManagement;

public class AIManager : MonoBehaviour
{
    public bool sculptingPuzzleComplete;
    public bool letterPuzzleComplete;
    public bool videoPuzzleComplete;
    public bool paintingPuzzleComplete;


    [Header("UI")]

    [SerializeField] public GameObject canvas;


    public bool allPuzzlesCompleted()
    {
        if(sculptingPuzzleComplete && letterPuzzleComplete && videoPuzzleComplete) 
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

            //canvas.enabled = true;

            //logig for the override
        }
    }


    public void WinGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
