using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("sockets")]

    public PuzzleSocket[] puzzleSockets;


    public bool puzzleComplete = false;


    private AIManager manager;


    void OnEnable()
    {
        foreach (var socket in puzzleSockets)
        {
            socket.OnSocketUpdated += CheckPuzzle;
        }
    }

    void OnDisable()
    {
        foreach (var socket in puzzleSockets)
        {
            socket.OnSocketUpdated -= CheckPuzzle;
        }
    }

    void CheckPuzzle()
    {
        puzzleComplete = true;


        foreach (var socket in puzzleSockets)
        {
            if (!socket.isCorrectlyFilled)
            {
                puzzleComplete = false;


                break;
            }
        }


        if (puzzleComplete)
        {
            manager.CheckPuzzleStates();
        }
    }
}
