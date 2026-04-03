using UnityEngine;

public class OpenURLScript : MonoBehaviour
{
    // You can change this URL in the Unity Inspector
    public string youtubeUrl = "https://youtube.com/shorts/fJcO6g45dEA?si=7nQTvK62VG_bekoc"; // short URL

    public void OpenYoutubeVideo()
    {
        if (!string.IsNullOrEmpty(youtubeUrl))
        {
            Application.OpenURL(youtubeUrl);
        }
        else
        {
            Debug.LogWarning("YouTube URL is empty!");
        }
    }
}