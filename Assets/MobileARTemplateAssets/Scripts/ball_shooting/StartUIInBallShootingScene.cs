using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class StartUIInBallShootingScene : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject welcomePanel;
    public GameObject adjustPanel;
    public GameObject TutorialPanel;
    public GameObject TutorialPanel2;
    public Button buttonToContinue;


    void Start()
    {
        // When the app starts, show Welcome and hide Adjust
        welcomePanel.SetActive(true);
        adjustPanel.SetActive(false);
        TutorialPanel.SetActive(false);
        TutorialPanel2.SetActive(false);
        buttonToContinue.gameObject.SetActive(false);
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

    // Called by the 'Done' button on the Adjust Panel
    public void FinishAndLock()
    {
        adjustPanel.SetActive(false);

        // The UI is now gone, and the user can play!
    }
}