using UnityEngine;
using UnityEngine.UI;

public class StartUIInBallShootingScene : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject welcomePanel;
    public GameObject adjustPanel;
    public GameObject TutorialPanel;
    public GameObject TutorialPanel2;
    public GameObject LeaderboardPanel;
    public GameObject GameOverPanel;
    public Button buttonToContinue;


    void Start()
    {
        // When the app starts, show Welcome and hide Adjust
        welcomePanel.SetActive(true);
        adjustPanel.SetActive(false);
        TutorialPanel.SetActive(false);
        TutorialPanel2.SetActive(false);
        LeaderboardPanel.SetActive(false);
        buttonToContinue.gameObject.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    // Called by the 'Start' button on the Welcome Panel
    public void StartSetup()
    {
        welcomePanel.SetActive(false);
        // adjustPanel.SetActive(true);
        TutorialPanel.SetActive(true);
        TutorialPanel2.SetActive(true);
        buttonToContinue.gameObject.SetActive(true);
    }

    public void ShowAdjustmentPanel()
    {
        TutorialPanel.SetActive(false);
        TutorialPanel2.SetActive(false);
        adjustPanel.SetActive(true);
    }

    public void ShowLeaderboardPanel()
    {
        adjustPanel.SetActive(false);
        LeaderboardPanel.SetActive(true);
    }

    public void CloseLBPanel()
    {
        adjustPanel.SetActive(true);
        LeaderboardPanel.SetActive(false);
    }

    // Called by the 'Done' button on the Adjust Panel
    public void FinishAndLock()
    {
        adjustPanel.SetActive(false);

        // The UI is now gone, and the user can play!
    }
}