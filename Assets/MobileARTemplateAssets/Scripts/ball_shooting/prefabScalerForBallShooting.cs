using UnityEngine;

public class prefabScalerForBallShooting : MonoBehaviour
{
    // scaling factor with range from 0.1 to 10f
    [Header("Scaling Factor")]
    [Range(0.1f, 10f)]
    public float scaleFactor = 6f;
    public float scaleStep = 0.5f; // step size for scaling the prefabs

    [Header("Court Settings")]
    public GameObject CourtPrefab; // court Game Object to scale
    public Vector3 defaultScaleFactorCourt = new Vector3(2f, 1f, 1f); // default scale factor for the court

    private Quaternion initialRotation; 
    private Vector3 initialForward;
    private Vector3 initialCameraPos;

    [Header("Racket Settings")]
    public GameObject racketPrefab; // racket prefab to scale
    public float defaultScaleFactorRacket = 0.4f; // default scale factor for the racket

    [Header("Ball Settings")]
    public GameObject ballPrefab;     // ball prefab to scale
    public float defaultScaleFactorBall = 0.01167f;     // default scale factor for the ball

    [Header("Court Rotation and Positioning")]
    public float rotateCourt = 90f; // rotation angle the court to face
    public float value_to_spawn_in_front = 6.0f; // distance in front of the camera to spawn the court

    // on validate, update the scale of the court and racket prefabs
    public void OnValidate()
    {
        UpdateScale();
        PositionObjectInFront();
    }

    void Start()
    {
        UpdateScale();

        initialCameraPos = Camera.main.transform.position; // 1. Capture the position of the phone at the very start
        initialForward   = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; // 2. Capture the direction (flattened so it stays level)
        initialRotation  = Quaternion.LookRotation(initialForward); // 3. Store that rotation

        PositionObjectInFront(); // Position the court in front of the user at the start
    }


    private void UpdateScale()
    {
        if (CourtPrefab != null)
        {
            CourtPrefab.transform.localScale = defaultScaleFactorCourt * scaleFactor;
        }

        if (racketPrefab != null)
        {
            racketPrefab.transform.localScale = new Vector3(defaultScaleFactorRacket * scaleFactor, defaultScaleFactorRacket * scaleFactor, defaultScaleFactorRacket * scaleFactor);
        }
        if (ballPrefab != null)
        {
            ballPrefab.transform.localScale = new Vector3(defaultScaleFactorBall * scaleFactor, defaultScaleFactorBall * scaleFactor, defaultScaleFactorBall * scaleFactor);
        }
    }

    // scale up the prefabs by increasing the scale factor (with a maximum of 10)
    public void ScaleUp()
    {
        scaleFactor = Mathf.Min(10f, scaleFactor + scaleStep); // prevent scaling above 10
        UpdateScale();
    }

    // scale down the prefabs by decreasing the scale factor
    public void ScaleDown()
    {
        scaleFactor = Mathf.Max(0.1f, scaleFactor - scaleStep); // prevent scaling below 0.1
        UpdateScale();
    }
    void PositionObjectInFront()
    {
        // Position it 2 units in front of the camera
        Vector3 spawnPos = initialCameraPos + (initialForward * value_to_spawn_in_front);

        // Optional: Keep it at a specific height (e.g., ground level)
        spawnPos.y = -1.0f; 

        CourtPrefab.transform.position = spawnPos;

        // Make the object look at the user, but stay upright
        CourtPrefab.transform.position = spawnPos;
        CourtPrefab.transform.rotation = initialRotation; 
        CourtPrefab.transform.Rotate(0, rotateCourt, 0); // Rotate the court to face the user
    }

}
