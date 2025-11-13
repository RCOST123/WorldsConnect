using UnityEngine;
using System.Collections;

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

    [Header("Key Display")]
    public GameObject keyPrefab;
    public float keyStartOffsetX = 8f;   // top right
    public float keyStartOffsetY = 4f;
    public float keySpacing = 1f;
    public float keyScale = 0.5f;
    

    private float minY;
    private float maxY;
    private float minX;
    private float maxX;

    public PlayerController playerController;
    private GameObject[] heartObjects;
    private GameObject[] keyObjects;

    private int lastHeartCount = -1;
    private int lastKeyCount = -1;

    void Start()
    {
        CameraBounds bounds = FindFirstObjectByType<CameraBounds>();
        if (bounds != null)
        {
            minY = bounds.minY;
            maxY = bounds.maxY;
            minX = bounds.minX;
            maxX = bounds.maxX;
        }
        else
        {
            minY = 0f;
            maxY = Mathf.Infinity;
            minX = -5f;
            maxX = 50f;
        }

        if (player != null)
            playerController = player.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("CameraFollow: PlayerController not found!");
            return;
        }

        InitializeHearts();
        InitializeKeys();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            float clampedX = Mathf.Clamp(player.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(player.position.y, minY, maxY);
            Vector3 targetPos = new Vector3(clampedX, clampedY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }

        UpdateHeartDisplay();
        UpdateKeyDisplay();
    }

    // -------------------- HEARTS -------------------- //
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

    public void UpdateHeartDisplay()
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
                if (heart != null) Destroy(heart);
        }

        InitializeHearts();
    }

    // -------------------- KEYS -------------------- //

    void InitializeKeys()
    {
        if (keyPrefab == null)
        {
            Debug.LogError("Key prefab not assigned!");
            return;
        }

        int maxKeys = playerController.maxKeys; // define in PlayerController
        keyObjects = new GameObject[maxKeys];

        for (int i = 0; i < maxKeys; i++)
        {
            Vector3 localPos = new Vector3(
                keyStartOffsetX - (i * keySpacing),
                keyStartOffsetY,
                10f
            );

            keyObjects[i] = Instantiate(keyPrefab, Vector3.zero, Quaternion.identity);
            keyObjects[i].transform.SetParent(transform);
            keyObjects[i].transform.localPosition = localPos;
            keyObjects[i].transform.localScale = Vector3.one * keyScale;
            keyObjects[i].name = "Key_" + (i + 1);

            // Make the key initially gray (uncollected)
            SetKeyColor(keyObjects[i], Color.gray);
        }

        lastKeyCount = -1; // force first-time update
    }

    public void UpdateKeyDisplay()
    {
        if (playerController == null || keyObjects == null) return;

        int currentKeys = playerController.GetCurrentKeys(); // implement in PlayerController
        int maxKeys = keyObjects.Length;

        for (int i = 0; i < maxKeys; i++)
        {
            if (keyObjects[i] == null) continue;

            // If this key just got collected, play animation
            if (i < currentKeys && i >= lastKeyCount)
            {
                StopAllCoroutines(); // stop previous animations
                StartCoroutine(FadeKeyColor(keyObjects[i], Color.gray, Color.white, 0.3f, true));
            }
            else if (i >= currentKeys)
            {
                SetKeyColor(keyObjects[i], Color.gray);
                keyObjects[i].transform.localScale = Vector3.one * keyScale;
            }
        }

        lastKeyCount = currentKeys;
    }

    IEnumerator FadeKeyColor(GameObject keyObj, Color fromColor, Color toColor, float duration, bool pop)
    {
        SpriteRenderer sr = keyObj.GetComponent<SpriteRenderer>();
        UnityEngine.UI.Image img = keyObj.GetComponent<UnityEngine.UI.Image>();

        float elapsed = 0f;
        Vector3 baseScale = Vector3.one * keyScale;
        Vector3 targetScale = pop ? baseScale * 1.2f : baseScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Color newColor = Color.Lerp(fromColor, toColor, t);

            if (sr != null) sr.color = newColor;
            if (img != null) img.color = newColor;

            if (pop)
                keyObj.transform.localScale = Vector3.Lerp(targetScale, baseScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (sr != null) sr.color = toColor;
        if (img != null) img.color = toColor;
        keyObj.transform.localScale = baseScale;
    }

    void SetKeyColor(GameObject keyObj, Color color)
    {
        SpriteRenderer sr = keyObj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color;
            return;
        }

        UnityEngine.UI.Image img = keyObj.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.color = color;
    }

    public void RefreshKeyDisplay()
    {
        if (keyObjects != null)
        {
            foreach (GameObject key in keyObjects)
                if (key != null) Destroy(key);
        }

        InitializeKeys();
    }


}
