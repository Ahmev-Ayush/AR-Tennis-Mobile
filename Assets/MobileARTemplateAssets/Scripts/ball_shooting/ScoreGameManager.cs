using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class ScoreGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText; // Label to display current score
    public TextMeshProUGUI missedText; // Label to display missed shots and remaining chances
    public TextMeshProUGUI highScoreText; // Optional label to display the all-time high score 

    [Header("Game Stats")]
    public int score = 0;
    public int missed = 0;
    public int maxMissed = 5; // Max misses before game over
    int highScore = 0;

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
            GameOver(); // Reset the game when the player has missed too many times
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
            missedText.color = Color.red; // Warning color for last chance
        }
        else
        {
            missedText.color = Color.white; // Normal color
        }
    }

    void GameOver()
    {
        // Simple reset: reload the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
