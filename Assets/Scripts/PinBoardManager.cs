using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PinBoardManager : MonoBehaviour
{
    public socket[] sockets = new socket[15];
    public void CheckPostItID(XRSocketInteractor currentSocket)
    {
        IXRSelectInteractable objName = currentSocket.GetOldestInteractableSelected();
        if (objName != null)
        {
            GameObject socket = currentSocket.gameObject;
            GameObject postit = objName.transform.gameObject;

            PostItNoteID socketid = socket.GetComponent<PostItNoteID>();
            PostItNoteID objectid = postit.GetComponent<PostItNoteID>();

            sockets[socketid.socketID].Occupied = true;

            if (socketid != null)
            {
                if (socketid.isOrange == objectid.isOrange && socketid.letter == objectid.letter)
                {
                    sockets[socketid.socketID].Correct = true;
                    //Debug.Log(objName.transform.name + " in socket of " + currentSocket.transform.name);
                }
            }
        }
    }

    public void RemovePostItID(XRSocketInteractor currentSocket)
    {
        IXRSelectInteractable objName = currentSocket.GetOldestInteractableSelected();
        if (objName == null)
        {
            GameObject socket = currentSocket.gameObject;

            PostItNoteID socketid = socket.GetComponent<PostItNoteID>();

            sockets[socketid.socketID].Occupied = false;

            sockets[socketid.socketID].Correct = false;
        }
    }
}
[System.Serializable]
public struct socket
{
    public bool Correct;
    public bool Occupied;
}