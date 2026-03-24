using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;
using TMPro;

public class PlayFabLogin : MonoBehaviour
{
    public static event Action OnLoginSuccessEvent;

    public static event Action<string> OnNameUpdateSuccessEvent;

    public TMP_InputField displayNameInput;

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "42"; // title id, replace with title id from PlayFab Game Manager
        }
        #if UNITY_ANDROID && !UNITY_EDITOR // runs only on android devices
            var request = new LoginWithAndroidDeviceIDRequest{ AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true};
            PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
        #else // runs on editor or windows device
            // var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true};
            var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        #endif
    }

    private void OnLoginSuccess(LoginResult result)
    {
        // Debug.Log("Congratulations, you made your first successful API call!");
        // Debug.Log("You can use the PlayFab ID " + result.PlayFabId + " to send your first PlayStream event, or to get data from the Player Data API.");
        // Debug.Log("request: " + SystemInfo.deviceUniqueIdentifier);
        
        // Notify other scripts that we are logged in
        OnLoginSuccessEvent?.Invoke();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        // Debug.LogWarning("Something went wrong with your first API call.  :(");
        // Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    // public void SetPlayerDisplayName(string displayName)
    // {
    //     var request = new UpdateUserTitleDisplayNameRequest { DisplayName = displayName };

    //     PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateSuccess, OnDisplayNameUpdateFailure);
    // }

    void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult result)
    {
        // Debug.Log("Player display name successfully updated to: " + result.DisplayName);
        OnNameUpdateSuccessEvent?.Invoke(result.DisplayName); // Notify other scripts that the display name has been updated successfully
    }

    void OnDisplayNameUpdateFailure(PlayFabError error)
    {
        // Debug.LogWarning("Something went wrong with updating the player's display name.  :(");
        // Debug.LogError("Here's some debug information:");
        // Debug.LogError(error.GenerateErrorReport());
    }

    public void submitNameButton()
    {
        if(string.IsNullOrEmpty(displayNameInput.text))
        {
            // Debug.LogWarning("Display name cannot be empty.");
            return;
        }
        else if(displayNameInput.text.Length < 3 || displayNameInput.text.Length > 20)
        {
            // Debug.LogWarning("Display name must be between 3 and 20 characters.");
            return;
        }

        var request = new UpdateUserTitleDisplayNameRequest{ DisplayName = displayNameInput.text };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateSuccess, OnDisplayNameUpdateFailure);
    }

}