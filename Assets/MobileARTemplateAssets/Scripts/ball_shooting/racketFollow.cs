using UnityEngine;

public class racketFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform arCamera; // Reference to the AR camera

    [Header("Settings from Image")]
    public Vector3 posOffset = new Vector3(0f, -0.133f, 0.23f); // Offset to position the racket in front of the camera
    private Vector3 rotOffset = new Vector3(0f, 90f, -100);

    [Header("prefabScaleScript")]
    public prefabScalerForBallShooting prefabScaleScript; // Reference to the prefabScalerForBallShooting script
    Vector3 a = new Vector3(0f, -0.133f, 0.23f);
    Vector3 b = new Vector3(0f, -0.6f, 1.2f);



    // update vs lateupdate
    void FixedUpdate()
    {
        if(arCamera == null)
        {
            return;
        }

        if (prefabScaleScript == null)
        {
            return;
        }

        
        float currentScaleFactor = prefabScaleScript.scaleFactor;

        // Linearly interpolate between the original and maximum offset based on the current scale factor
        posOffset = a + ( (b-a) * (currentScaleFactor-1.0f) / 5.0f );

        // 1. POSITION LOGIC
        // We calculate the target position by starting at the camera 
        // and moving along its local axes by your specific offset values.
        Vector3 targetPosition = arCamera.position 
            + arCamera.right * posOffset.x 
            + arCamera.up * posOffset.y 
            + arCamera.forward * posOffset.z;

        transform.position = targetPosition;

        // 2. ROTATION LOGIC
        // We take the camera's current orientation and multiply it by 
        // the racket's local rotation offset to keep the angle consistent
        transform.rotation = arCamera.rotation * Quaternion.Euler(rotOffset);


        // <________________________Ignore for now_______________________________>
        // 1. Calculate how far along the path we are (0.0 to 1.0)
        // This assumes your scale goes from 1.0 to 6.0
        // float t = (prefabScaleScript.scaleFactor - 1.0f) / 5.0f;

        // // 2. Clamp the value so it doesn't go below 0 or above 1
        // // This prevents the racket from going behind the camera or too far away
        // t = Mathf.Clamp01(t);

        // // 3. Linearly Interpolate
        // posOffset = Vector3.Lerp(a, b, t);

    }
}
