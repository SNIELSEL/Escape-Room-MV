using UnityEngine;
using UnityEngine.Video;

public class VideoClipPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public Material videoMaterial; // Material for the cube faces
    public RenderTexture renderTexture; // RenderTexture for the video

    void Start()
    {
        // Ensure VideoPlayer is set up
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        // Configure VideoPlayer
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        // Assign RenderTexture to the material
        if (videoMaterial != null)
        {
            videoMaterial.mainTexture = renderTexture;
        }

        // Optional: Set the video clip (can also be set in Inspector)
        // videoPlayer.clip = Resources.Load<VideoClip>("YourVideoClip");

        // Start playing the video
        videoPlayer.Play();
    }
}
