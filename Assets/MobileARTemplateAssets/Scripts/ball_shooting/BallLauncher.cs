using UnityEngine;
using TMPro;

// Launches pooled balls toward a target direction at a fixed cadence.
public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;                  // Prefab that must include a Rigidbody
    public Transform  targetDirection;             // Aim transform; usually the player or camera
    private Vector3   directionToCamera;           // Cached normalized direction to the camera for shooting
    private bool      directionRegistered = false; // Flag to ensure we only calculate direction once

    // public float shootForce    = 2.8f;          // Impulse strength per shot
    // public float fireRate      = 3f;            // Seconds between shots
    // public float destroyBelowY = -5f;           // Cleanup threshold for fallen balls
    
    // private bool _isStanding     = false;          // Track player posture; default is sitting, toggle with button press
    public float maxSpreadAngleY = 4f;             // Maximum angle for random spread in degrees
    public float maxSpreadAngleX = 12f;            // Maximum angle for random spread in degrees

    public BallShootingScriptableObjectScript DataContainer; // ScriptableObject for configurable parameters

    public TextMeshProUGUI debugText; // Optional UI element for displaying debug info in real-time

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // Prevent screen dimming for mobile AR template
    }

    void Update()
    {
        UpdateDebugDisplay();
    }
    // Start function changed to BeginGame so that called from Start Button
    public void BeginGame()
    {
        if(!directionRegistered)
        {
            directionToCamera = (targetDirection.position - transform.position).normalized;
            directionRegistered = true;
        }
        

        // Start firing after a short delay and keep repeating on the interval.
        InvokeRepeating("SpawnBallNew", 2f, DataContainer.fireRate);
    }

    // Default posture is sitting (12f). Press the button to toggle to standing (14f) and back.
/*
    public void TogglePlayerPosture()
    {
        _isStanding = !_isStanding;
        //              =  Standing (1.6 metres from ground level) : // Sitting (0.5 metres from ground level)
        maxSpreadAngleX = _isStanding ? Random.Range(11.5f, 12.5f) : Random.Range(2.5f, 3.2f); // vertical spread
        maxSpreadAngleY = _isStanding ? Random.Range( 3.5f,  3.7f) : Random.Range(2f, 3f);     // horizontal spread
        Debug.Log($"Player posture: {(_isStanding ? "Standing" : "Sitting")} — maxSpreadAngleX = {maxSpreadAngleX}");
    }

    // Spawns a single ball and applies an impulse force toward the target direction with some random spread.
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

        // custom shooting in a range 
        // Vector3 directionToCamera = (targetDirection.position - transform.position).normalized;

        Vector3 rightAxis = - Vector3.Cross(Vector3.up, directionToCamera).normalized;
        Vector3 upAxis    = Vector3.Cross(directionToCamera, rightAxis).normalized; // if ball launcher points straight up then use this

        Quaternion pitch  = Quaternion.AngleAxis(Random.Range(maxSpreadAngleX-1.2f, maxSpreadAngleX+0.5f), rightAxis); // Random pitch
        Quaternion yaw    = Quaternion.AngleAxis(Random.Range(-maxSpreadAngleY, maxSpreadAngleY), upAxis); // Random yaw

        Vector3 finalShotDirection = yaw * pitch * directionToCamera;
        Debug.Log($"Calculated shot direction: {finalShotDirection} with pitch {pitch.eulerAngles.x} and yaw {yaw.eulerAngles.y}");

        // Vector3 shortVector = Vector3.Lerp(a, b, Random.value); // interpolate between a and b

        // Add a small random upward component to the shot for variety, and apply the impulse force.
        DataContainer.shootForce = Random.Range(2.7f, 2.9f);                              // randomize shoot force a bit for variety
        Debug.Log($"Shooting ball with force {DataContainer.shootForce} toward {finalShotDirection}");

        // adding a small upward component to the shot for variety, and apply the impulse force.
        rb.AddForce((finalShotDirection + Vector3.up * 0.2f) * DataContainer.shootForce, ForceMode.Impulse);

        // Start watching this instance; destroy it if it falls below the cleanup height.
        StartCoroutine(DestroyWhenBelow(ball));
    }
*/
    void SpawnBallNew()
    {
        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        // Grab the Rigidbody on this above spawned instance so we can apply force to it (each clone has its own component).
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Ball prefab is missing a Rigidbody", ball);
            return;
        }

        float     h       = transform.position.y;          // height of the ball launcher
        Vector3 CameraPos = targetDirection.position;      // position of camera
        float     x0      = CameraPos.y;                   // height of the camera
        // ---------------------------------
        float     vd = x0 - h;                             // vertical distance between detected plane and camera
        x0 = x0 - vd; // adjust camera height to be relative to the plane, not the world origin
        // ---------------------------------
        float     g       = Physics.gravity.y;             // gravity (negative value)
        Vector2 camHorizontal = new Vector2(CameraPos.x, CameraPos.z);                    // horizontal position of the camera
        Vector2 objHorizontal = new Vector2(transform.position.x, transform.position.z);  // horizontal position of the ball launcher

        float distance     = Vector2.Distance(camHorizontal, objHorizontal);  // horizontal distance to camera
        float angle        = Random.Range(29.9f, 30.1f) * Mathf.Deg2Rad;      // random launch angle in radians
        Debug.Log($"Calculated distance to camera: {distance}, vertical dist (plane→cam): {vd:F2}m, height {h}, using launch angle: {angle * Mathf.Rad2Deg} degrees");

        float dinominatorLength = 2 * (x0 - h - Mathf.Tan(angle) * distance) * Mathf.Pow(Mathf.Cos(angle), 2);
        float velocitySquared   = g * Mathf.Pow(distance, 2) / dinominatorLength;         // derived from projectile motion equations

        if (velocitySquared <= 0)
        {
            Debug.LogError("Calculated non-positive velocity squared, cannot shoot ball.");
            return;
        }

        float velocity = Mathf.Sqrt(velocitySquared);
        float shootForce = velocity * rb.mass;                                  // F = m * v for an instantaneous impulse

        Vector3 shotDirection = (CameraPos - transform.position).normalized;    // Direction from launcher to camera
        Vector3 rightAxis = - Vector3.Cross(Vector3.up, directionToCamera).normalized;
        Vector3 upAxis    =   Vector3.Cross(directionToCamera, rightAxis).normalized; 

        Quaternion pitch  = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, rightAxis);      // Random pitch
        Quaternion yaw    = Quaternion.AngleAxis(Random.Range(-3f, 3f),  upAxis);         // Random yaw


        Vector3 finalShotDirection = yaw * pitch * shotDirection;

        rb.AddForce(finalShotDirection * shootForce, ForceMode.Impulse); // Apply the calculated impulse force in the direction of the camera


        StartCoroutine(DestroyWhenBelow(ball));                         // Start watching this instance; destroy it if it falls below the cleanup height.

    }

    public void UpdateDebugDisplay()
    {
        if (debugText == null) return;

        float   h             = transform.position.y;
        Vector3 cameraPos     = targetDirection.position;
        float   x0            = cameraPos.y;
        float   g             = Physics.gravity.y;
        Vector2 camHorizontal = new Vector2(cameraPos.x, cameraPos.z);
        Vector2 objHorizontal = new Vector2(transform.position.x, transform.position.z);

        float distance        = Vector2.Distance(camHorizontal, objHorizontal);
        float verticalDist    = x0 - h;                                        // vertical distance between detected plane and camera
        float angle           = 30f * Mathf.Deg2Rad;
        x0 = x0 + verticalDist; // adjust camera height to be relative to the plane, not the world origin

        float denominator     = 2 * (x0 - h - Mathf.Tan(angle) * distance) * Mathf.Pow(Mathf.Cos(angle), 2);
        float velocitySquared = (denominator != 0) ? g * Mathf.Pow(distance, 2) / denominator : 0f;
        float v0              = (velocitySquared > 0) ? Mathf.Sqrt(velocitySquared) : float.NaN;

        Vector3    shotDirection = (cameraPos - transform.position).normalized;
        Vector3    rightAxis     = -Vector3.Cross(Vector3.up, shotDirection).normalized;
        Vector3    upAxis        =  Vector3.Cross(shotDirection, rightAxis).normalized;
        Quaternion pitch         =  Quaternion.AngleAxis(angle * Mathf.Rad2Deg, rightAxis);
        Vector3    finalDir      =  pitch * shotDirection;

        string report = "<color=yellow>--- AR REALTIME DEBUG ---</color>\n";
        report += $"Horizontal Dist: {distance:F2}m\n";
        report += $"Launcher Y (h): {h:F2}m\n";
        report += $"Camera Y (x0): {x0:F2}m\n";
        report += $"Plane→Cam Vertical: {verticalDist:F2}m\n";
        report += $"Calculated V0: {v0:F2} m/s\n";
        report += $"Launch Vector: {finalDir:F2}\n";
        report += $"Angle: {angle * Mathf.Rad2Deg:F2}°\n";
        report += $"verticalDist: {verticalDist:F2}m\n";
        debugText.text = report;
    }

    // Per-ball watcher that destroys the instance once it drops below the Y threshold.
    private System.Collections.IEnumerator DestroyWhenBelow(GameObject ball)
    {
        while (ball != null)
        {
            if (ball.transform.position.y < DataContainer.destroyBelowY)
            {
                Destroy(ball);
                yield break;
            }

            yield return null; // Wait for next frame.
        }
    }
}