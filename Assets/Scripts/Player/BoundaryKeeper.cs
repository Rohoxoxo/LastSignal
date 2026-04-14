using UnityEngine;

// Attach to Player — clamps position within the arena bounds.
// Set xLimit and zLimit to match your arena's half-extents.
public class BoundaryKeeper : MonoBehaviour
{
    public float xLimit = 10f;
    public float zLimit = 10f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        pos.z = Mathf.Clamp(pos.z, -zLimit, zLimit);
        pos.y = 0f;
        transform.position = pos;
    }
}
