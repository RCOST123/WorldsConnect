using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;

    [Header("Heart Display")]
    public GameObject heartPrefab;
    public float heartStartOffsetX = -8f;
    public float heartStartOffsetY = 4f;
    public float heartSpacing = 1f;
    public float heartScale = 0.5f;

    private float fixedX;
    private float minY;
    private float maxY;

    private PlayerController playerController;
    private GameObject[] heartObjects;
    private int lastHeartCount = -1;

    void Start()
    {
        fixedX = transform.position.x;

        CameraBounds bounds = FindFirstObjectByType<CameraBounds>();
        if (bounds != null)
        {
            minY = bounds.minY;
            maxY = bounds.maxY;
        }
        else
        {
            minY = 0f;
            maxY = Mathf.Infinity;
        }

        if (player != null)
            playerController = player.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("CameraFollow: PlayerController not found!");
            return;
        }

        InitializeHearts();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            float clampedY = Mathf.Clamp(player.position.y, minY, maxY);
            Vector3 targetPos = new Vector3(fixedX, clampedY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }

        UpdateHeartDisplay();
    }

    void InitializeHearts()
    {
        if (heartPrefab == null)
        {
            Debug.LogError("Heart prefab not assigned!");
            return;
        }

        int maxHearts = playerController.maxHearts;
        heartObjects = new GameObject[maxHearts];

        for (int i = 0; i < maxHearts; i++)
        {
            Vector3 localPos = new Vector3(
                heartStartOffsetX + (i * heartSpacing),
                heartStartOffsetY,
                10f
            );

            heartObjects[i] = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity);
            heartObjects[i].transform.SetParent(transform);
            heartObjects[i].transform.localPosition = localPos;
            heartObjects[i].transform.localScale = Vector3.one * heartScale;
            heartObjects[i].name = "Heart_" + (i + 1);
        }

        lastHeartCount = maxHearts;
    }

    void UpdateHeartDisplay()
    {
        if (playerController == null || heartObjects == null) return;

        int currentHearts = playerController.GetCurrentHearts();

        if (currentHearts != lastHeartCount)
        {
            for (int i = 0; i < heartObjects.Length; i++)
            {
                if (heartObjects[i] != null)
                    heartObjects[i].SetActive(i < currentHearts);
            }

            lastHeartCount = currentHearts;
        }
    }

    public void RefreshHeartDisplay()
    {
        if (heartObjects != null)
        {
            foreach (GameObject heart in heartObjects)
            {
                if (heart != null)
                    Destroy(heart);
            }
        }

        InitializeHearts();
    }
}