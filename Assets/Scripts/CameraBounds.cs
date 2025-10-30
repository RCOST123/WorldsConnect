using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [Header("Camera Y Limits")]
    public float minY = 0f;
    public float maxY = Mathf.Infinity;
    public float minX = -3f;
    public float maxX = 50f;
}
