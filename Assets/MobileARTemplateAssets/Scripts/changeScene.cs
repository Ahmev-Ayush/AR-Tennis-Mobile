using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("Scene_Dragon");
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("carScene");
    }

    public void LoadDefaultScene()
    {
        if (Application.CanStreamedLevelBeLoaded("ScenePrime"))
        {
            SceneManager.LoadScene("ScenePrime");
        }
        else
        {
            exitApp();
        }
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene("ImageTrackingScene");
    }

    public void LoadScene4()
    {
        SceneManager.LoadScene("PointCloudScene");
    }

    public void LoadScene5()
    {
        SceneManager.LoadScene("BallShootingGameScene");
    }

    public void LoadScene6()
    {
        SceneManager.LoadScene("6DaysScene");
    }
    
    public void exitApp()
    {
        Application.Quit();
    }

    // restart scene function for testing purposes
    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
