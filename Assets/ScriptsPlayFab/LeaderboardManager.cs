using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    public GameObject DataPrefab; // Prefab for displaying leaderboard entries
    public Transform parent;      // Parent transform for leaderboard entries
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

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, OnError);
    }

    private void OnScoreSubmit(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully submitted score!");
    }

    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        Debug.Log("Top 10 Leaderboard: ");
        // Clear existing leaderboard entries
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        foreach (var entry in result.Leaderboard)
        {

            GameObject newEntry = Instantiate(DataPrefab, parent);
            Text[] texts = newEntry.GetComponentsInChildren<Text>();
            texts[0].text = (entry.Position + 1).ToString(); // Rank
            texts[1].text = entry.PlayFabId;                 // Player ID
            texts[2].text = entry.StatValue.ToString();      // Score
            Debug.Log(string.Format("Rank: {0}, PlayFabId: {1}, Score: {2}", entry.Position, entry.PlayFabId, entry.StatValue));
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error submitting score: " + error.GenerateErrorReport());
    }   

}
