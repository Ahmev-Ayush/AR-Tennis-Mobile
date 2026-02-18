using UnityEngine;

public class ZoneDetection : MonoBehaviour
{
    public bool isScoreZone;
    public ScoreGameManager gameManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            tennisBall ball = other.GetComponent<tennisBall>();

            if(isScoreZone && ball.hasBeenHitByPlayer)
            {
                gameManager.AddPoint();
                Debug.Log("Ball entered score zone. Score: " + gameManager.score);
            }
            else if (!isScoreZone)
            {
                gameManager.BallMissed();
                Debug.Log("Ball entered miss zone. Missed: " + gameManager.missed);
            }

            Destroy(other.gameObject);
        }
    }
}
