using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

// This script is replaced by PrefabScaleAndPlacementScript.cs, which combines prefab scaling and placement 
// functionality into a single script for better organization and efficiency.

public class prefabScalerForBallShooting : MonoBehaviour
{
    public ARRaycastManager raycastManager; // Reference to the ARRaycastManager for plane detection
    public ARPlaneManager planeManager; // Reference to the ARPlaneManager for managing detected planes

    // scaling factor with range from 0.1 to 10f
    [Header("Scaling Factor")]
    [Range(0.1f, 10f)]
    public float scaleFactor = 6f;
    public float scaleStep = 0.5f; // step size for scaling the prefabs

    [Header("Court Settings")]
    public GameObject CourtPrefab; // court Game Object to scale
    public Vector3 defaultScaleFactorCourt = new Vector3(2f, 1f, 1f); // default scale factor for the court

    // private Quaternion initialRotation; 
    // private Vector3 initialForward;
    // private Vector3 initialCameraPos;


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

    // on validate, update the scale of the court and racket prefabs
    public void OnValidate()
    {
        UpdateScale();
        // PositionObjectInFront();
    }

    void Start()
    {
        UpdateScale();

        // initialCameraPos = Camera.main.transform.position; // 1. Capture the position of the phone at the very start
        // initialForward   = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; // 2. Capture the direction (flattened so it stays level)
        // initialRotation  = Quaternion.LookRotation(initialForward); // 3. Store that rotation

        // PositionObjectInFront(); // Position the court in front of the user at the start
    }

    void Update()
    {

        if(isPlacing){
            return; // If the court is already placed, we don't need to raycast anymore
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame && ARModeController.IsPlacementAllowed){
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

            if(IsPointerOverUI(screenCenter)){
                return;
            }

            if (raycastManager.Raycast(screenCenter, s_hits, TrackableType.PlaneWithinPolygon)){
                Pose hitPose = s_hits[0].pose;

                PlaceCourtOnPlane(hitPose);
            }
        }
        else if(Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && ARModeController.IsPlacementAllowed){
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            if(IsPointerOverUI(screenCenter)){
                return;
            }
            if (raycastManager.Raycast(screenCenter, s_hits, TrackableType.PlaneWithinPolygon)){
                Pose hitPose = s_hits[0].pose;

                PlaceCourtOnPlane(hitPose);
            }
        }

    }

    void PlaceCourtOnPlane(Pose hitPose){
        CourtPrefab.transform.position = hitPose.position;

        Vector3 lookPos = Camera.main.transform.position - CourtPrefab.transform.position;
        lookPos.y = 0; // keep the court upright
        
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        CourtPrefab.transform.rotation = rotation;

        CourtPrefab.transform.position = hitPose.position - (lookPos.normalized * value_to_spawn_in_front);

        CourtPrefab.transform.Rotate(0, rotateCourt, 0); // Rotate the court to face the user

        isPlacing = true; // Set the flag to indicate that the court has been placed

        SetAllPlanesActive(false); // Hide all detected planes after placing the court
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
    

    // commented this function (PositionObjectInFront) as fixed by the raycast-based placement in Update() to spawn the court 
    // on detected planes instead of always in front of the user, which is more intuitive for mobile AR experiences. 
    // We can revisit this if we want a quick spawn option that ignores plane detection, 
    // but for now it seems better to keep the court grounded in the real world.

    // void PositionObjectInFront()
    // {
    //     // Position it 2 units in front of the camera
    //     Vector3 spawnPos = initialCameraPos + (initialForward * value_to_spawn_in_front);

    //     // Optional: Keep it at a specific height (e.g., ground level)
    //     spawnPos.y = -1.0f; 

    //     CourtPrefab.transform.position = spawnPos;

    //     // Make the object look at the user, but stay upright
    //     CourtPrefab.transform.position = spawnPos;
    //     CourtPrefab.transform.rotation = initialRotation; 
    //     CourtPrefab.transform.Rotate(0, rotateCourt, 0); // Rotate the court to face the user
    // }


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
