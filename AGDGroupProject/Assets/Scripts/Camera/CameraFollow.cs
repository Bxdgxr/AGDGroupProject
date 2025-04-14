using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public BoxCollider2D mapBounds; // The collider that defines the border

    public float smoothTime = 0.2f; // Adjust for faster/slower smoothing

    private float halfHeight;
    private float halfWidth;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    private void LateUpdate()
    {
        if (target == null || mapBounds == null) return;

        Bounds bounds = mapBounds.bounds;

        // Desired position based on player
        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Clamp to bounds
        float clampedX = Mathf.Clamp(desiredPos.x, bounds.min.x + halfWidth, bounds.max.x - halfWidth);
        float clampedY = Mathf.Clamp(desiredPos.y, bounds.min.y + halfHeight, bounds.max.y - halfHeight);
        Vector3 clampedPos = new Vector3(clampedX, clampedY, transform.position.z);

        // Smoothly move camera
        transform.position = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, smoothTime);
    }
}
