using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PrefabScaleAndPlacementScript : MonoBehaviour
{
    public ARRaycastManager raycastManager; // Reference to the ARRaycastManager for plane detection
    public ARPlaneManager planeManager;     // Reference to the ARPlaneManager for managing detected planes

    // scaling factor with range from 0.1 to 10f
    [Header("Scaling Factor")]
    [Range(0.1f, 10f)]
    public float scaleFactor = 6f;
    public float scaleStep = 0.5f; // step size for scaling the prefabs

    [Header("Court Settings")]
    public GameObject CourtPrefab; // court Game Object to scale
    public Vector3 defaultScaleFactorCourt = new Vector3(2f, 1f, 0.75f); // default scale factor for the court

    [Header("Racket Settings")]
    public GameObject racketPrefab;               // racket prefab to scale
    public float defaultScaleFactorRacket = 0.4f; // default scale factor for the racket

    [Header("Ball Settings")]
    public GameObject ballPrefab;                   // ball prefab to scale
    public float defaultScaleFactorBall = 0.01167f; // default scale factor for the ball

    [Header("Court Rotation and Positioning")]
    public float rotateCourt = -90f;              // rotation angle the court to face
    public float value_to_spawn_in_front = 5f; // distance in front of the camera to spawn the court

    // Raycast hit results will be stored in this list
    public bool isPlacing = false;                                          // flag to indicate if the court is being placed
    private static List<ARRaycastHit> s_hits = new List<ARRaycastHit>();    // List to store raycast hits

    // Input Action for event-driven touches/clicks
    private InputAction pressAction;

    private void Awake()
    {
        // __________Create___ an input action that triggers on any pointer press (Touch or Mouse)
        pressAction = new InputAction("Tap", type: InputActionType.Button, binding: "<Pointer>/press");
        pressAction.performed += OnPointerPressed;
    }

    private void OnEnable()
    {
        pressAction.Enable();
    }

    private void OnDisable()
    {
        pressAction.Disable();
    }

    private void OnDestroy()
    {
        // Properly dispose of the input action to prevent memory leaks and null reference issues
        if (pressAction != null)
        {
            pressAction.performed -= OnPointerPressed;
            pressAction.Dispose();
        }
    }

    // on validate, update the scale of the court and racket prefabs
    public void OnValidate()
    {
        // Only update scale if we have valid prefab references
        if (CourtPrefab != null || racketPrefab != null || ballPrefab != null)
        {
            UpdateScale();
        }
    }

    void Start()
    {
        UpdateScale();
    }

    // Event-driven callback replacing Update()
    private void OnPointerPressed(InputAction.CallbackContext context)
    {
        if (isPlacing) return; // If the court is already placed, we don't need to raycast anymore

        // Check if AR Mode allows placement
        // Assuming ARModeController is a globally accessible class in your project
        // if (!ARModeController.IsPlacementAllowed) return;

        // Add null check for raycastManager
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager is not assigned!");
            return;
        }

        // Your original logic used the screen center for the raycast
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        // Optional: If you want to check if the actual touch was over UI, use the pointer position instead:
        // Vector2 touchPosition = Pointer.current.position.ReadValue();
        if (IsPointerOverUI(screenCenter))
        {
            return;
        }

        if (raycastManager.Raycast(screenCenter, s_hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_hits[0].pose;
            PlaceCourtOnPlane(hitPose);
        }
    }

    void PlaceCourtOnPlane(Pose hitPose)
    {
        if (CourtPrefab == null)
        {
            Debug.LogError("CourtPrefab is null! Cannot place court on plane.");
            return;
        }

        CourtPrefab.transform.position = hitPose.position;

        Vector3 lookPos = Camera.main.transform.position - CourtPrefab.transform.position;
        lookPos.y = 0; // keep the court upright
        
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        CourtPrefab.transform.rotation = rotation;

        CourtPrefab.transform.position = hitPose.position - (lookPos.normalized * value_to_spawn_in_front);

        CourtPrefab.transform.Rotate(0, rotateCourt, 0); // Rotate the court to face the user

        isPlacing = true; // Set the flag to indicate that the court has been placed

        SetAllPlanesActive(false); // Hide all detected planes after placing the court
        
        // Optimization: Disable the input action entirely since we only place the court once
        pressAction.Disable(); 
    }

    // Utility function to set ALL detected planes active or inactive
    void SetAllPlanesActive(bool value)
    {
        if (planeManager != null)
        {
            planeManager.enabled = false; // Disable the plane manager to stop detecting new planes

            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(value); // Set each existing plane active or inactive
            }

            raycastManager.enabled = false; // Disable the raycast manager to stop raycasting after placement
        }
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
    
    // Utility function to check if the touch position is over a UI element, 
    // to prevent placing the court when interacting with UI
    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}