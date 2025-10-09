using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Assign the player in the Inspector
    public float smoothSpeed = 5f;

    private float fixedX;
    private float minY;
    private float maxY;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player reference not set!");
            return;
        }

        fixedX = transform.position.x;

        // Try to find camera bounds in the scene
        CameraBounds bounds = FindObjectOfType<CameraBounds>();
        if (bounds != null)
        {
            minY = bounds.minY;
            maxY = bounds.maxY;
        }
        else
        {
            minY = 0f;
            maxY = Mathf.Infinity;
            Debug.LogWarning("CameraFollow: No CameraBounds found, using default Y limits.");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        float clampedY = Mathf.Clamp(player.position.y, minY, maxY);
        Vector3 targetPos = new Vector3(fixedX, clampedY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
