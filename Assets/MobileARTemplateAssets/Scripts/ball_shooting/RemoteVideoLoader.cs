using UnityEngine;
using UnityEngine.Video;
using PlayFab;
using PlayFab.ClientModels;

[System.Serializable] // This class is used to structure video settings if you choose to send them as a JSON string in a single Title Data key
public class VideoConfig
{
    public string videoUrl;
    public bool loopVideo;
    public float playbackSpeed;
}

public class RemoteVideoLoader : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            LoadVideoSettings();
        }
        else   
        {
            Debug.Log("Waiting for PlayFab login...");
            PlayFabLogin.OnLoginSuccessEvent += LoadVideoSettings;
            // The += means: "When the event fires, run my LoadVideoSettings function
        }
    }

    void OnDestroy()
    {
        // Always unsubscribe to prevent memory leaks
        PlayFabLogin.OnLoginSuccessEvent -= LoadVideoSettings;
    }

    // This function can be called to load video settings from PlayFab Title Data
    public void LoadVideoSettings()
    {
        // Unsubscribe if we were subscribed, to avoid multiple calls if logic changes
        PlayFabLogin.OnLoginSuccessEvent -= LoadVideoSettings;

        Debug.Log("Fetching Video Settings from PlayFab...");

        var request = new GetTitleDataRequest();
        PlayFabClientAPI.GetTitleData(request, OnDataReceived, OnError);
    }

    void OnDataReceived(GetTitleDataResult result)
    {
        if (result.Data != null)
        {
            VideoConfig config = new VideoConfig();
            
            // 1. Try to get individual keys (Title Data Key-Value pairs)
            if (result.Data.ContainsKey("videoUrl"))
                config.videoUrl = result.Data["videoUrl"];
            
            if (result.Data.ContainsKey("loopVideo"))
                bool.TryParse(result.Data["loopVideo"], out config.loopVideo);
            
            if (result.Data.ContainsKey("playbackSpeed"))
                float.TryParse(result.Data["playbackSpeed"], out config.playbackSpeed);

            // 2. Fallback: If you decide to use a JSON string in a key like "MyVideoSettings"
            // if (result.Data.ContainsKey("MyVideoSettings"))
            // {
            //     config = JsonUtility.FromJson<VideoConfig>(result.Data["MyVideoSettings"]);
            // }

            if (!string.IsNullOrEmpty(config.videoUrl))
            {
                videoPlayer.url = config.videoUrl;
                videoPlayer.isLooping = config.loopVideo;
                videoPlayer.playbackSpeed = config.playbackSpeed;
                videoPlayer.Play();
                Debug.Log("Video Loaded from PlayFab: " + config.videoUrl);
            }
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("PlayFab Error: " + error.GenerateErrorReport());
    }
}