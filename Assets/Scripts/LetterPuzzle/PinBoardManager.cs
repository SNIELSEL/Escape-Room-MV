// Ugh, reader brain explode if no comments
// I used to have happy marriage, now divorce papers pile on desk
// Every morning I wake up, hear the silence, divorce lawyer call...
using System.Collections;
using System.Collections.Generic;


// You need this for WaitForSeconds, unlike my ex who never waited for me
using UnityEngine;

// This magic deals with socket interactables, just like I deal with wordy court forms
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// And these grabbers let you pick up pieces, I wish I could pick up my shattered heart
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Welcome to PinBoardManager class, like welcome to my life of broken trust
public class PinBoardManager : MonoBehaviour
{
    // Parent holding spawn spots—kind of like the empty house holds memories
    [SerializeField]
    Transform SpawnLocationParent;

    // SocketStatus tracks occupancy, my lawyer tracks who got which assets
    SocketStatus status;

    // Array of 15 sockets, just like 15 months of tears I sorted through
    socket[] sockets = new socket[15];

    // Interactors allow notes to snap in place, unlike me snapping back after betrayal
    [SerializeField]
    XRSocketInteractor[] socketInteractors = new XRSocketInteractor[15];

    // PostItNotes are all the little pieces, like little promises that got broken
    [SerializeField]
    PostItNoteID[] PostItNotes = new PostItNoteID[15];

    // Textures: normal, wrong, correct—feel like my emotions each day
    [SerializeField]
    Texture[] PostItTexture = new Texture[3];

    // Store original textures so we can revert, like trying to revert to who I was
    Texture[] originalPostItMat = new Texture[15];

    // SpawnLocations list so notes know where to go, unlike me knowing where life goes
    Transform[] SpawnLocations;

    // Awake runs before Start—like therapy before facing the world
    public void Awake()
    {
        // Make array for spawn spots, count must match or chaos like my dating history
        SpawnLocations = new Transform[SpawnLocationParent.childCount];

        // Loop through each child transform, like looping through memories
        for (int i = 0; i < SpawnLocationParent.childCount; i++)
        {
            // Save spawn spots, like saving receipts for alimony
            SpawnLocations[i] = SpawnLocationParent.GetChild(i);
        }

        // Grab SocketStatus component—need it like I need therapy notes
        status = this.gameObject.GetComponent<SocketStatus>();

        // Link sockets array to status sockets, like linking my heart to mistakes
        sockets = status.sockets;

        // Save each note's original texture—trying to remember how life looked before divorce
        for (int i = 0; i < PostItNotes.Length; i++)
        {
            // Every GetComponent call reminds me of checking my ex’s messages
            originalPostItMat[i] = PostItNotes[i].gameObject.GetComponent<Renderer>().material.GetTexture("_BaseMap");
        }
    }

    // Start runs next, where we shuffle notes like I shuffle through my emotions
    private void Start()
    {
        // Fisher-Yates shuffle to randomize spawn spots
        for (int i = SpawnLocations.Length - 1; i > 0; i--)
        {
            int randomInt = Random.Range(0, i + 1); // pick random
            Transform tmp = SpawnLocations[i]; // hold one
            SpawnLocations[i] = SpawnLocations[randomInt]; // swap
            SpawnLocations[randomInt] = tmp; // swap back
        }
        // Place each note at its spot, like placing masks on my face each morning
        for (int i = 0; i < PostItNotes.Length; i++)
        {
            PostItNotes[i].transform.position = SpawnLocations[i].position;
            PostItNotes[i].transform.rotation = SpawnLocations[i].rotation;
        }

        CorrectlyPlacePostItNotes();
    }

    // CorrectlyPlacePostItNotes: move 7 broken promises back to right spots
    public void CorrectlyPlacePostItNotes()
    {
        // Ugh, only 7 of 15 ever stand right, rest fall apart like my heart
        // Pick 7 random notes, always include note 0 because even that one suffers
        HashSet<int> selected = new HashSet<int>();
        selected.Add(0); // note 0, anchor of sorrow

        // Choose 6 more at random from the pile
        while (selected.Count < 7)
        {
            int idx = Random.Range(1, PostItNotes.Length); // random note index
            selected.Add(idx); // if duplicates, keep picking until unique
        }

        // Now move each selected note to its matching socket, like healing cracks
        foreach (int i in selected)
        {
            // Get the note and its socket spot
            var note = PostItNotes[i]; // fragile piece of memory
            var socketSpot = socketInteractors[i].transform; // where it belongs, if only life was this simple

            // Teleport note to its socket, no physics, just forced placement
            note.transform.position = socketSpot.position;
            note.transform.rotation = socketSpot.rotation;
        }
    }


    // RelocatePiece: toss piece back, like tossing memories away
    public void RelocatePiece(GameObject piece)
    {
        // Try get XRGrabInteractable, hope it holds better than my ex
        XRGrabInteractable grab = piece.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
        {
            // Force release piece, like forcing myself to let go
            grab.interactionManager.SelectExit(grab.firstInteractorSelecting, grab);
            // Delay relocation, because healing takes time
            StartCoroutine(DelayedRelocation(piece, 1f));
        }
        else
        {
            // Immediate relocation if not held, like sudden heartbreak
            StartCoroutine(DelayedRelocation(piece, 0f));
        }
    }

    // DelayedRelocation: move piece after wait, patience like waiting for closure
    IEnumerator DelayedRelocation(GameObject piece, float delay)
    {
        // Wait before action, like waiting for ex to call back
        yield return new WaitForSeconds(delay);

        // Pick random spawn index, randomness of life
        int randomIndex = Random.Range(0, SpawnLocations.Length);
        var target = SpawnLocations[randomIndex]; // the new place

        // Get Rigidbody, physics reminds me gravity of reality
        var rb = piece.GetComponent<Rigidbody>();
        bool wasKinematic = rb != null && rb.isKinematic; // remember physics state
        if (rb != null) rb.isKinematic = true; // pause physics

        // Teleport piece, like running away from painful thoughts
        piece.transform.position = target.position;
        piece.transform.rotation = target.rotation;

        if (rb != null)
        {
            // Reset velocities, like resetting my mood swings
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // Restore physics, return to reality
            rb.isKinematic = wasKinematic;
        }
    }

    // CheckPostItID: verify note vs socket, like verifying lies vs truth
    public void CheckPostItID(XRSocketInteractor currentSocket)
    {
        GameObject socket = currentSocket.gameObject; // the slot
        PostItNoteID socketid = socket.GetComponent<PostItNoteID>(); // expected ID data

        IXRSelectInteractable objName = currentSocket.GetOldestInteractableSelected(); // actual note

        if (objName != null)
        {
            GameObject postit = objName.transform.gameObject;
            PostItNoteID objectid = postit.GetComponent<PostItNoteID>(); // actual data

            sockets[socketid.ID].Occupied = true; // mark occupied
            sockets[0].OccupiedSockets++; // increment count

            if (socketid != null)
            {
                if (socketid.isOrange == objectid.isOrange && socketid.letter == objectid.letter)
                {
                    sockets[0].CorrectSockets++; // correct placement
                    sockets[socketid.ID].Correct = true;
                }
                else
                {
                    // Wrong match, reminder of my worst mistakes
                }
            }

            CheckCompletion(); // see if puzzle done, like seeing if wounds healed
        }
        else
        {
            // No note, reset this socket like resetting hopes
            sockets[socketid.ID].Occupied = false;
            sockets[socketid.ID].Correct = false;

            // Reset totals, recount every time, like counting tears
            sockets[0].OccupiedSockets = 0;
            sockets[0].CorrectSockets = 0;
            for (int i = 0; i < sockets.Length; i++)
            {
                if (sockets[i].Occupied) sockets[0].OccupiedSockets++;
                if (sockets[i].Correct) sockets[0].CorrectSockets++;
            }
        }
    }

    // CheckCompletion: decide victory or retry, life always loops
    public void CheckCompletion()
    {
        if (sockets[0].OccupiedSockets == 15 && sockets[0].CorrectSockets == 15)
        {
            // Puzzle complete, brief joy before next storm
            StartCoroutine(Completed());
        }
        else if (sockets[0].OccupiedSockets == 15 && sockets[0].CorrectSockets != 15)
        {
            // Mistakes found, flash red like heartbreak
            StartCoroutine(NotCompleted());
        }
    }

    // Completed: show correct visuals, lock notes, lock heart?
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
        yield return null; // moment of peace
    }

    // loopTimes: how many times to blink mistakes, like reliving trauma
    int loopTimes;

    // NotCompleted: flash wrong notes, then reset for another try, like therapy sessions
    IEnumerator NotCompleted()
    {
        loopTimes = 10; // so many chances, yet I still fail
        StartCoroutine(WrongSocket()); // begin blinking mistakes
        yield return new WaitForSeconds(3); // watch them blink like memories
        for (int i = 0; i < sockets.Length; i++) socketInteractors[i].socketActive = true; // enable all to try again
    }

    // WrongSocket: blink wrong texture, mock mistakes endlessly
    IEnumerator WrongSocket()
    {
        for (int i = 0; i < sockets.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", PostItTexture[1]);
        yield return new WaitForSeconds(0.2f); // blink, blink, blink
        for (int i = 0; i < sockets.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) PostItNotes[i].gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", originalPostItMat[i]);
        yield return new WaitForSeconds(0.2f); // blink, blink, blink again
        loopTimes--; // fewer blinks remain, like hope fading
        if (loopTimes != 0) StartCoroutine(WrongSocket());
        else
        {
            for (int i = 0; i < socketInteractors.Length; i++) if (sockets[i].Occupied && !sockets[i].Correct) { socketInteractors[i].socketActive = false; PostItNotes[i].gameObject.layer = 7; }
            yield return new WaitForSeconds(3); // hide notes, hide pain
            for (int i = 0; i < socketInteractors.Length; i++) if (PostItNotes[i].gameObject.layer == 7) { PostItNotes[i].gameObject.layer = 6; socketInteractors[i].socketActive = true; }
        }
    }
}
