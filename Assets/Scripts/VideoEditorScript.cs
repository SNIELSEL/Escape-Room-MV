using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class VideoClipEditorScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> sockets;
    [SerializeField]
    List<VideoClipPlayer> videoClipList = new();
    [SerializeField]
    List<VideoClipPlayer> correctList = new();
    public VideoClip wrongOrderClip;
    [SerializeField]
    private int currentClipIndex;
    [SerializeField]
    VideoPlayer BigScreenPlayer;
    public RenderTexture renderTexture;
    private AudioSource audioSource;
    public void CheckFullClipPlacement()
    {
        Debug.Log("Checking clips");
        videoClipList.Clear(); // Clear the list to repopulate with current socket states

        foreach (GameObject socket in sockets) // Assumes 'sockets' is a List<GameObject> defined in the class
        {
            Collider[] colliders = Physics.OverlapBox(socket.transform.position, new Vector3(0.5f, 0.5f, 0.5f));

            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    GameObject socketedObject = collider.gameObject;
                    VideoClipPlayer clipPlayer = socketedObject.GetComponent<VideoClipPlayer>();

                    if (clipPlayer != null && !videoClipList.Contains(clipPlayer))
                    {
                        videoClipList.Add(clipPlayer);
                        Debug.Log($"Added VideoClipPlayer from {socketedObject.name} to list");
                        break; // Exit after adding the first valid VideoClipPlayer for this socket
                    }
                }
            }
            else
            {
                Debug.LogWarning($"No objects found at socket {socket.name} position");
            }
        }

        Debug.Log($"VideoClipList count: {videoClipList.Count}");
        if (videoClipList.Count == 3)
        {
            PlayFullClip();
        }
    }

    private void PlayFullClip()
    {
        if (BigScreenPlayer == null)
        {
            BigScreenPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        BigScreenPlayer.playOnAwake = true;
        BigScreenPlayer.isLooping = false;
        BigScreenPlayer.renderMode = VideoRenderMode.RenderTexture;
        BigScreenPlayer.targetTexture = renderTexture;
        BigScreenPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        // Get or add AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 1f;
        audioSource.spatialBlend = 1f;
        BigScreenPlayer.SetTargetAudioSource(0, audioSource);
        BigScreenPlayer.EnableAudioTrack(0, true);

        int index = 0;
        bool correctOrder = false;
        foreach (VideoClipPlayer clipPlayer in videoClipList)
        {
            Debug.Log($"{clipPlayer.videoClip} and {correctList[index].videoClip}");
            if (clipPlayer.videoClip != correctList[index].videoClip)
            {
                correctOrder = false;
                BigScreenPlayer.clip = wrongOrderClip;
                break;
            }
            else
            {
                correctOrder = true;
                index++;
                continue;
            }

        }
        if (correctOrder)
        {
            BigScreenPlayer.clip = videoClipList[0].videoClip;
            BigScreenPlayer.loopPointReached += PlayNextClip;
        }
        else
        {
            BigScreenPlayer.Play();
        }
    }

    private void PlayNextClip(VideoPlayer vp)
    {
        currentClipIndex++;
        if (currentClipIndex > videoClipList.Count + 1)
        {
            BigScreenPlayer.loopPointReached -= PlayNextClip;
            return;
        }

        BigScreenPlayer.clip = videoClipList[currentClipIndex].videoClip;
        Debug.Log($"current clip playing {videoClipList[currentClipIndex]}");
        BigScreenPlayer.Play();
    }
}