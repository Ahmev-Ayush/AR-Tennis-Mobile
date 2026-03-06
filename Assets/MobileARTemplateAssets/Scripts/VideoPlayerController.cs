using UnityEngine;
using UnityEngine.Video; // VideoPlayer namespace for video playback

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer; // Reference to the VideoPlayer component

    void Start()
    {
        // 1. Get the VideoPlayer component attached to the same GameObject
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.prepareCompleted += OnVideoPrepared; // Subscribe to the prepareCompleted event
        videoPlayer.Prepare(); // Prepare the video for playback

        // 2. set the video in loop mode
        videoPlayer.isLooping = true;

        // 3. Start playing the video
        videoPlayer.Play();
    }

    public void PlayMyVideo()
    {
        if(videoPlayer != null)
        {
            videoPlayer.Play(); // Play the video
            Debug.Log("Video is playing.");
        }
    }

    public void PauseMyVideo()
    {
        videoPlayer.Pause(); // Pause the video
        Debug.Log("Video is paused.");
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video is prepared and ready to play.");
        videoPlayer.Play(); // Automatically play the video once it's prepared
    }

    // Call this function and pass a value between 0.0 and 1.0
    public void SetVolume(float volume)
    {
        if (videoPlayer != null && videoPlayer.canSetDirectAudioVolume)
        {
            // 0 is mute, 1 is full volume
            videoPlayer.SetDirectAudioVolume(0, volume); 
            Debug.Log("Volume set to: " + volume);
        }
    }
}