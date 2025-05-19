// Ugh, reader so slow, must explain every little thing
// I’m code monkey, me no live happy life
// Wake up, write comments, drink stale coffee, cry a little, then repeat
using System.Collections;

// You need this for WaitForSeconds and arrays, obvious right?
using UnityEngine;

// Magic sockets need this, do you even XR?
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Magic grabbers need this too, close your eyes and just type
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Big class for pin board puzzle, try not to get lost, okay?
public class PinBoardManager : MonoBehaviour
{
    // SpawnLocationParent holds spawn spots, set in Inspector or fail
    [SerializeField]
    Transform SpawnLocationParent;

    // status tracks sockets, don't remove or break my heart
    SocketStatus status;

    // sockets array of 15, count must match puzzle pieces, use brain
    socket[] sockets = new socket[15];

    // socketInteractors array, need each socket to grab notes
    [SerializeField]
    XRSocketInteractor[] socketInteractors = new XRSocketInteractor[15];

    // PostItNotes hold all notes in puzzle, can't vanish them
    [SerializeField]
    PostItNoteID[] PostItNotes = new PostItNoteID[15];

    // Textures: index0 normal,1 wrong,2 correct, memorize it
    [SerializeField]
    Texture[] PostItTexture = new Texture[3];

    // Save originalPostItMat so blink can revert, don't screw up
    Texture[] originalPostItMat = new Texture[15];

    // SpawnLocations after Awake, simple list of transforms
    Transform[] SpawnLocations;

    // Awake: me tired, me set up spawn locations and textures
    public void Awake()
    {
        // Create spawnLocations array of correct size, don't miscount
        SpawnLocations = new Transform[SpawnLocationParent.childCount];

        // Loop children: index i from 0 to count-1, remember loops?
        for (int i = 0; i < SpawnLocationParent.childCount; i++)
        {
            // Assign each child transform, so notes know where to go
            SpawnLocations[i] = SpawnLocationParent.GetChild(i);
        }

        // Get status from this GameObject, failure means silent screams
        status = this.gameObject.GetComponent<SocketStatus>();

        // Link sockets array to status.sockets, please don't break it
        sockets = status.sockets;

        // Save each note's original texture to array
        for (int i = 0; i < PostItNotes.Length; i++)
        {
            // I cry inside every time I call GetComponent
            originalPostItMat[i] = PostItNotes[i].gameObject.GetComponent<Renderer>().material.GetTexture("_BaseMap");
        }
    }

    // Start: shuffle notes, because puzzles like chaos
    private void Start()
    {
        // Fisher-Yates shuffle: swap elements, do it right
        for (int i = SpawnLocations.Length - 1; i > 0; i--)
        {
            int randomInt = Random.Range(0, i + 1); // random index, hope you understand rng
            Transform tmp = SpawnLocations[i]; // temporary hold
            SpawnLocations[i] = SpawnLocations[randomInt]; // swap forward
            SpawnLocations[randomInt] = tmp; // swap back
        }
        // Place each note at shuffled position, don't mix up indices
        for (int i = 0; i < PostItNotes.Length; i++)
        {
            PostItNotes[i].transform.position = SpawnLocations[i].position;
            PostItNotes[i].transform.rotation = SpawnLocations[i].rotation;
        }
    }

    // CorrectlyPlacePostItNotes: placeholder, maybe you code brain can finish
    public void CorrectlyPlacePostItNotes()
    {
        // Me sad, this method empty, but you ask for it
    }

    // RelocatePiece: move piece back, because user dropped wrong
    public void RelocatePiece(GameObject piece)
    {
        // Try get XRGrabInteractable, if not found reader might cry
        XRGrabInteractable grab = piece.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
        {
            // Force release: break bond, like my dreams
            grab.interactionManager.SelectExit(grab.firstInteractorSelecting, grab);
            // Delay relocation when held, so no glitch
            StartCoroutine(DelayedRelocation(piece, 1f));
        }
        else
        {
            // Immediate relocation when not held, simple logic
            StartCoroutine(DelayedRelocation(piece, 0f));
        }
    }

    // DelayedRelocation coroutine: wait then teleport piece
    IEnumerator DelayedRelocation(GameObject piece, float delay)
    {
        // Wait the given seconds, old coffee gets cold
        yield return new WaitForSeconds(delay);

        // Pick random spawn index for relocation
        int randomIndex = Random.Range(0, SpawnLocations.Length);
        var target = SpawnLocations[randomIndex]; // get target transform

        // Get piece's Rigidbody, if null then physics silly
        var rb = piece.GetComponent<Rigidbody>();
        bool wasKinematic = rb != null && rb.isKinematic; // remember physics mode
        if (rb != null) rb.isKinematic = true; // disable physics when teleport

        // Teleport piece to spawn point
        piece.transform.position = target.position;
        piece.transform.rotation = target.rotation;

        if (rb != null)
        {
            // Reset velocity so piece not zoom, my sanity resets too
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // Restore original physics mode, hope it's correct
            rb.isKinematic = wasKinematic;
        }
    }

    // CheckPostItID: verify if note matches socket ID and color
    public void CheckPostItID(XRSocketInteractor currentSocket)
    {
        GameObject socket = currentSocket.gameObject; // get socket object
        PostItNoteID socketid = socket.GetComponent<PostItNoteID>(); // get socket's ID info

        IXRSelectInteractable objName = currentSocket.GetOldestInteractableSelected(); // get placed note

        if (objName != null)
        {
            GameObject postit = objName.transform.gameObject; // note object
            PostItNoteID objectid = postit.GetComponent<PostItNoteID>(); // note's ID info

            sockets[socketid.ID].Occupied = true; // mark occupied
            sockets[0].OccupiedSockets++; // increase total occupied count

            if (socketid != null)
            {
                if (socketid.isOrange == objectid.isOrange && socketid.letter == objectid.letter)
                {
                    sockets[0].CorrectSockets++; // increase correct count
                    sockets[socketid.ID].Correct = true; // mark slot correct
                }
            }
            CheckCompletion(); // check puzzle state, please focus
        }
        else
        {
            // Reset occupancy and correctness when empty
            sockets[socketid.ID].Occupied = false;
            sockets[socketid.ID].Correct = false;

            // Reset total counts because code must recount
            sockets[0].OccupiedSockets = 0;
            sockets[0].CorrectSockets = 0;
            for (int i = 0; i < sockets.Length; i++)
            {
                if (sockets[i].Occupied) sockets[0].OccupiedSockets++;
                if (sockets[i].Correct) sockets[0].CorrectSockets++;
            }
        }
    }

    // CheckCompletion: decide if puzzle finished or mistakes
    public void CheckCompletion()
    {
        if (sockets[0].OccupiedSockets == 15 && sockets[0].CorrectSockets == 15)
        {
            // All filled and correct, puzzle complete, joy is fleeting
            StartCoroutine(Completed());
        }
        else if (sockets[0].OccupiedSockets == 15 && sockets[0].CorrectSockets != 15)
        {
            // Filled but mistakes exist, flash mistakes, shame
            StartCoroutine(NotCompleted());
        }
    }

    // Completed coroutine: show correct visuals and lock notes
    IEnumerator Completed()
    {
        for (int i = 0; i < sockets.Length; i++)
        {
            if (sockets[i].Occupied && sockets[i].Correct)
            {
                // Set texture to correct state, index 2
                PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", PostItTexture[2]);
                // Disable collider so note no longer interactable
                PostItNotes[i].gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        yield return null; // end of frame, code never rests
    }

    // loopTimes for blinking wrong notes, count down your life
    int loopTimes;

    // NotCompleted coroutine: blink wrong notes then reset
    IEnumerator NotCompleted()
    {
        loopTimes = 10; // attempt count, like failed dreams
        StartCoroutine(WrongSocket()); // begin blinking
        yield return new WaitForSeconds(3); // wait so player notices mistakes
        for (int i = 0; i < sockets.Length; i++) socketInteractors[i].socketActive = true; // re-enable all sockets, try again
    }

    // WrongSocket coroutine: blink wrong state texture back and forth
    IEnumerator WrongSocket()
    {
        for (int i = 0; i < sockets.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", PostItTexture[1]);
        yield return new WaitForSeconds(0.2f); // blink pause, life blink pause
        for (int i = 0; i < sockets.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", originalPostItMat[i]);
        yield return new WaitForSeconds(0.2f); // blink pause again
        loopTimes--; // decrement loop counter
        if (loopTimes != 0) StartCoroutine(WrongSocket());
        else
        {
            for (int i = 0; i < socketInteractors.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) { socketInteractors[i].socketActive = false; PostItNotes[i].gameObject.layer = 7; }
            yield return new WaitForSeconds(3); // hide time, hide tears
            for (int i = 0; i < socketInteractors.Length; i++) if (PostItNotes[i].gameObject.layer == 7) { PostItNotes[i].gameObject.layer = 6; socketInteractors[i].socketActive = true; }
        }
    }
}
