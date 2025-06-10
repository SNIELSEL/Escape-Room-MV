using UnityEngine;

public class AIManager : MonoBehaviour
{
    public bool sculptingPuzzleComplete;
    public bool letterPuzzleComplete;
    public bool videoPuzzleComplete;
    public bool paintingPuzzleComplete;


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
        }
    }


    public void WinGame()
    {
        //logig for the win screen and sounds
    }
}
