using UnityEngine;

[CreateAssetMenu(fileName = "BallShootingScriptableObjectScript", menuName = "Scriptable Objects/BallShootingScriptableObjectScript")]
public class BallShootingScriptableObjectScript : ScriptableObject
{
    public float shootForce;    // Impulse strength per shot
    public float fireRate;        // Seconds between shots
    public float destroyBelowY;  // Cleanup threshold for fallen balls
}
