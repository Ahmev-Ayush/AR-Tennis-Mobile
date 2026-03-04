using UnityEngine;

// Launches pooled balls toward a target direction at a fixed cadence.
public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;      // Prefab that must include a Rigidbody
    private Transform targetDirection; // Aim transform; usually the player or camera
    public float shootForce = 2.8f;    // Impulse strength per shot
    public float fireRate = 3f;        // Seconds between shots
    public float destroyBelowY = -5f;  // Cleanup threshold for fallen balls

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // Prevent screen dimming for mobile AR template
    }

    // Start function changed to BeginGame so that called from Start Button
    public void BeginGame()
    {
        // Start firing after a short delay and keep repeating on the interval.
        InvokeRepeating("SpawnBall", 2f, fireRate);
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        // Grab the Rigidbody on this above spawned instance so we can apply force to it (each clone has its own component).
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        // If the prefab was misconfigured and has no Rigidbody, fail fast to avoid null refs.
        if (rb == null)
        {
            Debug.LogError("Ball prefab is missing a Rigidbody", ball);
            return;
        }
        
        // Shoot toward the target with a slight upward arc for a lobbed trajectory.
        // Vector3 shotVector = (targetDirection.position - transform.position).normalized;
        // Vector3 shotVector = new Vector3(0, 0.4f, -1f); // Shoot straight forward in local space, ignoring target position for mobile AR template

        // custom shooting in a range (between two direction vectors)
        Vector3 a = new Vector3(0.5f,  1.8f, -9f).normalized;
        Vector3 b = new Vector3(-0.5f, 1.8f, -9f).normalized;

        Vector3 shortVector = Vector3.Lerp(a, b, Random.value); // interpolate between a and b

        // Add a small random upward component to the shot for variety, and apply the impulse force.
        shootForce = Random.Range(2.7f, 2.9f);                                   // randomize shoot force a bit for variety
        Debug.Log($"Shooting ball with force {shootForce} toward {shortVector}");

        // adding a small upward component to the shot for variety, and apply the impulse force.
        rb.AddForce((shortVector + Vector3.up * 0.2f) * shootForce, ForceMode.Impulse);

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