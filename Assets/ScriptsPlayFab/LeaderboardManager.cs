using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class LeaderBoardManager : MonoBehaviour
{
    public void SendLeaderboard(int score)
    {
        // check if the player is logged in before trying to send a score
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogError("Player is not logged in. Cannot submit score to leaderboard.");
            return;
        }
        
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore", 
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnScoreSubmit, OnError);

    }

    private void OnScoreSubmit(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully submitted score!");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error submitting score: " + error.GenerateErrorReport());
    }   

}
