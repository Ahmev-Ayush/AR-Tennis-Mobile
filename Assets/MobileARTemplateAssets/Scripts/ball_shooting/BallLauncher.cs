using UnityEngine;

// Launches pooled balls toward a target direction at a fixed cadence.
public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;      // Prefab that must include a Rigidbody
    public Transform targetDirection;  // Aim transform; usually the player or camera
    public float shootForce = 15f;     // Impulse strength per shot
    public float fireRate = 3f;        // Seconds between shots
    public float destroyBelowY = -5f;  // Cleanup threshold for fallen balls

    void Start()
    {
        // Start firing after a short delay and keep repeating on the interval.
        InvokeRepeating("SpawnBall", 2f, fireRate);
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        // Grab the Rigidbody on this spawned instance so we can apply force to it (each clone has its own component).
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        // If the prefab was misconfigured and has no Rigidbody, fail fast to avoid null refs.
        if (rb == null)
        {
            Debug.LogError("Ball prefab is missing a Rigidbody", ball);
            return;
        }
        
        // Shoot toward the target with a slight upward arc for a lobbed trajectory.
        // Vector3 shotVector = (targetDirection.position - transform.position).normalized;
        Vector3 shotVector = new Vector3(0,0, -1); // Shoot straight forward in local space, ignoring target position for mobile AR template
        rb.AddForce((shotVector + Vector3.up * 0.2f) * shootForce, ForceMode.Impulse);

        // Start watching this instance; destroy it if it falls below the cleanup height.
        StartCoroutine(DestroyWhenBelow(ball));
    }

    // Per-ball watcher that destroys the instance once it drops below the Y threshold.
    private System.Collections.IEnumerator DestroyWhenBelow(GameObject ball)
    {
        while (ball != null)
        {
            if (ball.transform.position.y < destroyBelowY)
            {
                Destroy(ball);
                yield break;
            }

            yield return null; // Wait for next frame.
        }
    }
}