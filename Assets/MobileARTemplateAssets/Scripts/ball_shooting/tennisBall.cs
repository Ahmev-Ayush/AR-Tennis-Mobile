using UnityEngine;

public class tennisBall : MonoBehaviour
{
    public bool hasBeenHitByPlayer = false;

    // This is called when the racket hits the ball
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerRacket"))
        {
            hasBeenHitByPlayer = true;
            Debug.Log("Ball hit by player!");
            if(AudioManagerScript.Instance != null)
            {
                AudioManagerScript.Instance.Play("RacketHitBall");
            }
        }
    }
}