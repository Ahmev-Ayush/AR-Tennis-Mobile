using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;     // Label to display current score
    public TextMeshProUGUI missedText;    // Label to display missed shots and remaining chances
    public TextMeshProUGUI highScoreText; // label to display the all-time high score 
    public TextMeshProUGUI FinalScoreText;  // label to display "Game Over" score on the Game Over panel 

    [Header("Game Stats")]
    public int score = 0;
    public int missed = 0;
    public int maxMissed = 3; // Max misses before game over
    int highScore = 0;
    public LeaderBoardManager leaderBoardManager;
    public BallLauncher ballLauncher; // Reference to the BallLauncher to stop shooting when game is over
    public StartUIInBallShootingScene startUIManager; // Reference to the StartUI manager to show Game Over panel

    void Start()
    {
        // Initialize scoreboard labels on scene start
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    public void AddPoint()
    {
        // Increase score on hit and reset the current miss streak
        score++;
        missed = 0; 
        // Check for new high score and save it if achieved
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    public void BallMissed()
    {
        // Track consecutive misses and trigger game reset if the cap is reached
        missed++;

        UpdateUI();

        if (missed >= maxMissed)
        {
            ballLauncher.StopShooting(); // Stop spawning new balls when game is over

            GameOver();                  // Reset the game when the player has missed too many times
        }
    }

    void UpdateUI()
    {
        // Reflect current score and misses in the HUD; warn when about to lose
        scoreText.text = "Score: " + score;
        missedText.text = "Missed: " + missed + "/" + maxMissed;
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
            highScoreText.color = (score >= highScore) ? Color.yellow : Color.white; // Highlight new high score
        }

        if(missed == maxMissed - 1)
        {
            missedText.color = Color.red;   // Warning color for last chance
        }
        else
        {
            missedText.color = Color.white; // Normal color
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
        leaderBoardManager.SendLeaderboard(score);                      // Send final score to leaderboard before resetting Scene

        startUIManager.GameOverPanel.SetActive(true);                   // Show the Game Over panel
        FinalScoreText.text = score.ToString();

        StartCoroutine(ResetSceneAfterDelay(3f));                       // Wait a 3 seconds before resetting the scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);     // Simple reset: reload the active scene
    }

    // for reset of high score, can be called from a UI button
    public void ResetHighScore()
    {
        // Wipe all PlayerPrefs (including high score) - use with caution!
        // PlayerPrefs.DeleteAll();

        //Delete the saved high score from PlayerPrefs and reset the local variable, then update the UI
        PlayerPrefs.DeleteKey("HighScore");

        PlayerPrefs.Save();
        highScore = 0;
        UpdateUI();

        Debug.Log("High score reset.");
    }

    public bool continueToGameOver()
    {
        return true;
    }

    private IEnumerator ResetSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
