using UnityEngine;

// Top-down camera that follows the player.
// Set Camera rotation to (90, 0, 0) and assign this script.
// Use Projection = Orthographic or Perspective with high Y height.
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float height = 17f;
    public float smoothSpeed = 8f;

    [Header("Arena bounds (match BoundaryKeeper)")]
    public float xLimit = 12f;
    public float zLimit = 12f;

    void LateUpdate()
    {
        if (target == null) return;

        float x = Mathf.Clamp(target.position.x, -xLimit, xLimit);
        float z = Mathf.Clamp(target.position.z, -zLimit, zLimit);
        Vector3 desired = new Vector3(x, height, z);
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
