using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform player; // Reference to the player character
    public Vector3 localOffsetFromPlayer; // The local offset from the player to the camera target
    public float cameraCollisionOffset = 0.5f; // Offset to prevent the camera target from being embedded in the wall
    public LayerMask levelStructureLayerMask;

    private void Start()
    {
        // Store the local offset based on the initial relative position
        localOffsetFromPlayer = player.InverseTransformPoint(transform.position);
    }

    private void Update()
    {
        // Calculate desired position based on the player's transformation and the local offset
        Vector3 desiredPosition = player.TransformPoint(localOffsetFromPlayer);

        Vector3 displacement = desiredPosition - player.position;

        Ray ray = new Ray(player.position, displacement);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, displacement.magnitude, levelStructureLayerMask))
        {
            Vector3 hitPoint = hit.point - ray.direction.normalized * cameraCollisionOffset;
            transform.position = hitPoint;
        }
        else
        {
            transform.position = desiredPosition;
        }
    }
}
