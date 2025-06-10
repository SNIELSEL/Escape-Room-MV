using UnityEngine;
using UnityEngine.Video;

public class VideoClipPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public Material videoMaterial; // Material face that plays the video
    public VideoClip videoClip; // Clip that you want to be played
    public RenderTexture renderTexture; // RenderTexture for the video
    private AudioSource audioSource;

    void Start()
    {
        // Get or add AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 1f;
        audioSource.spatialBlend = 1f;

        // Configure VideoPlayer
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        if (videoPlayer.clip == null)
        {
            videoPlayer.clip = videoClip;
        }

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.EnableAudioTrack(0, true);

        // Assign RenderTexture to material
        if (videoMaterial != null)
        {
            videoMaterial.mainTexture = renderTexture;
        }

        // Debug
        if (videoPlayer.clip != null)
        {
            Debug.Log($"Video: {videoPlayer.clip.name}, Duration: {videoPlayer.clip.length}, Audio Tracks: {videoPlayer.audioTrackCount}");
        }
        else
        {
            Debug.LogError("No video clip assigned.");
        }
    }

    public void PlayClip()
    {
        videoPlayer.Play();
    }

    public void StopClip()
    {
        videoPlayer.Stop();
    }
}