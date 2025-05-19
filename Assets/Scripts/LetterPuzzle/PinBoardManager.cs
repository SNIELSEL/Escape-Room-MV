using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PinBoardManager : MonoBehaviour
{
    [SerializeField]
    Transform SpawnLocationParent;

    SocketStatus status;

    socket[] sockets = new socket[15];

    [SerializeField]
    XRSocketInteractor[] socketInteractors = new XRSocketInteractor[15];

    [SerializeField]
    PostItNoteID[] PostItNotes = new PostItNoteID[15];

    [SerializeField]
    Texture[] PostItTexture = new Texture[3];

    Texture[] originalPostItMat = new Texture[15];

    Transform[] SpawnLocations;

    public void Awake()
    {
        SpawnLocations = new Transform[SpawnLocationParent.childCount];

        for (int i = 0; i < SpawnLocationParent.childCount; i++)
        {
            SpawnLocations[i] = SpawnLocationParent.GetChild(i);
        }

        status = this.gameObject.GetComponent<SocketStatus>();

        sockets = status.sockets;

        for (int i = 0; i < PostItNotes.Length; i++)
        {
            originalPostItMat[i] = PostItNotes[i].gameObject.GetComponent<Renderer>().material.GetTexture("_BaseMap");
        }
    }

    private void Start()
    {
        for (int i = SpawnLocations.Length - 1; i > 0; i--)
        {
            int randomInt = Random.Range(0, i + 1);
            Transform tmp = SpawnLocations[i];
            SpawnLocations[i] = SpawnLocations[randomInt];
            SpawnLocations[randomInt] = tmp;
        }
        for (int i = 0; i < PostItNotes.Length; i++)
        {
            PostItNotes[i].transform.position = SpawnLocations[i].position;
            PostItNotes[i].transform.rotation = SpawnLocations[i].rotation;
        }

        CorrectlyPlacePostItNotes();
    }

    public void CorrectlyPlacePostItNotes()
    {
        HashSet<int> selected = new HashSet<int>();
        selected.Add(0);

        while (selected.Count < 7)
        {
            int idx = Random.Range(1, PostItNotes.Length);
            selected.Add(idx);
        }

        foreach (int i in selected)
        {
            var note = PostItNotes[i];
            var socketSpot = socketInteractors[i].transform;

            note.transform.position = socketSpot.position;
            note.transform.rotation = new Quaternion(0,90,0,0);
        }
    }

    public void RelocatePiece(GameObject piece)
    {
        XRGrabInteractable grab = piece.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
        {
            grab.interactionManager.SelectExit(grab.firstInteractorSelecting, grab);
            StartCoroutine(DelayedRelocation(piece, 1f));
        }
        else
        {
            StartCoroutine(DelayedRelocation(piece, 0f));
        }
    }

    IEnumerator DelayedRelocation(GameObject piece, float delay)
    {
        yield return new WaitForSeconds(delay);

        int randomIndex = Random.Range(0, SpawnLocations.Length);
        var target = SpawnLocations[randomIndex];

        var rb = piece.GetComponent<Rigidbody>();
        bool wasKinematic = rb != null && rb.isKinematic;
        if (rb != null) rb.isKinematic = true;

        piece.transform.position = target.position;
        piece.transform.rotation = target.rotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = wasKinematic;
        }
    }

    public void CheckPostItID(XRSocketInteractor currentSocket)
    {
        GameObject socket = currentSocket.gameObject;
        PostItNoteID socketid = socket.GetComponent<PostItNoteID>();

        IXRSelectInteractable objName = currentSocket.GetOldestInteractableSelected();

        if (objName != null)
        {
            GameObject postit = objName.transform.gameObject;
            PostItNoteID objectid = postit.GetComponent<PostItNoteID>();

            sockets[socketid.ID].Occupied = true;
            sockets[0].OccupiedSockets++;

            if (socketid != null)
            {
                if (socketid.isOrange == objectid.isOrange && socketid.letter == objectid.letter)
                {
                    sockets[0].CorrectSockets++;
                    sockets[socketid.ID].Correct = true;
                }
                else
                {
                }
            }

            CheckCompletion();
        }
        else
        {
            sockets[socketid.ID].Occupied = false;
            sockets[socketid.ID].Correct = false;

            sockets[0].OccupiedSockets = 0;
            sockets[0].CorrectSockets = 0;
            for (int i = 0; i < sockets.Length; i++)
            {
                if (sockets[i].Occupied) sockets[0].OccupiedSockets++;
                if (sockets[i].Correct) sockets[0].CorrectSockets++;
            }
        }
    }
    public void CheckCompletion()
    {
        if (sockets[0].OccupiedSockets == 15 && sockets[0].CorrectSockets == 15)
        {
            StartCoroutine(Completed());
        }
        else if (sockets[0].OccupiedSockets == 15 && sockets[0].CorrectSockets != 15)
        {
            StartCoroutine(NotCompleted());
        }
    }

    IEnumerator Completed()
    {
        for (int i = 0; i < sockets.Length; i++)
        {
            if (sockets[i].Occupied && sockets[i].Correct)
            {
                PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", PostItTexture[2]);
                PostItNotes[i].gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        yield return null;
    }

    int loopTimes;

    IEnumerator NotCompleted()
    {
        loopTimes = 10; 
        StartCoroutine(WrongSocket()); 
        yield return new WaitForSeconds(3);
        for (int i = 0; i < sockets.Length; i++) socketInteractors[i].socketActive = true;
    }

    IEnumerator WrongSocket()
    {
        for (int i = 0; i < sockets.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", PostItTexture[1]);
        yield return new WaitForSeconds(0.2f); 
        for (int i = 0; i < sockets.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", originalPostItMat[i]);
        yield return new WaitForSeconds(0.2f);
        loopTimes--;
        if (loopTimes != 0) StartCoroutine(WrongSocket());
        else
        {
            for (int i = 0; i < socketInteractors.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) { socketInteractors[i].socketActive = false; PostItNotes[i].gameObject.layer = 7; }
            yield return new WaitForSeconds(3);
            for (int i = 0; i < socketInteractors.Length; i++) if (PostItNotes[i].gameObject.layer == 7) { PostItNotes[i].gameObject.layer = 6; socketInteractors[i].socketActive = true; }
        }
    }
}
