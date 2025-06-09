using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PuzzleSocket : MonoBehaviour
{
    public XRSocketInteractor socket;
    public int requiredPieceID;
    public bool isCorrectlyFilled = false;


    public System.Action OnSocketUpdated;

    void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntered);
        socket.selectExited.AddListener(OnSelectExited);
    }


    void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntered);
        socket.selectExited.RemoveListener(OnSelectExited);
    }


    void OnSelectEntered(SelectEnterEventArgs args)
    {
        UpdateSocketState();
    }


    void OnSelectExited(SelectExitEventArgs args)
    {
        UpdateSocketState();
    }


    void UpdateSocketState()
    {
        var selected = socket.GetOldestInteractableSelected();

        var selectedComponent = selected as Component;


        if (selectedComponent != null)
        {
            PieceID currentPiece = selectedComponent.GetComponent<PieceID>();

            isCorrectlyFilled = currentPiece != null && currentPiece.pieceIndex == requiredPieceID;
        }

        else
        {
            isCorrectlyFilled = false;
        }


        OnSocketUpdated?.Invoke();
    }
}
