using UnityEngine;
using System.Collections;

public class LavaManager : MonoBehaviour
{
    public static LavaManager Instance;

    public float baseRiseSpeed = 1f;
    private float currentRiseSpeed;
    public float distanceFactor = 0.05f;
    public float safeOffset = 5f;

    private Transform player;
    private bool rising = false;
    private bool slowing = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentRiseSpeed = baseRiseSpeed;
        transform.position = new Vector3(player.position.x, player.position.y - 20f, 0); // start offscreen
    }

    void Update()
    {
        if (!rising || player == null) return;

        // Calculate distance-based rising speed
        float distance = Mathf.Abs(player.position.y - transform.position.y);
        float riseAmount = currentRiseSpeed + (distance * distanceFactor);

        // Move up over time
        transform.Translate(Vector2.up * riseAmount * Time.deltaTime);
    }

    public void StartLavaRise()
    {
        rising = true;
        Debug.Log("Lava rising started!");
    }

    public void StopLavaTemporarily()
    {
        StartCoroutine(PauseThenRise());
    }

    private IEnumerator PauseThenRise()
    {
        rising = false;
        yield return new WaitForSeconds(2f);
        rising = true;
    }

    public void ResetLavaBelowPlayer(Vector3 playerPos)
    {
        transform.position = new Vector3(playerPos.x, playerPos.y - safeOffset, transform.position.z);
        StopLavaTemporarily();
    }

    public void SlowLava()
    {
        if (slowing) return;
        StartCoroutine(SlowLavaCoroutine());
    }

    private IEnumerator SlowLavaCoroutine()
    {
        slowing = true;
        float oldSpeed = currentRiseSpeed;
        currentRiseSpeed = baseRiseSpeed / 2f; // half speed
        Debug.Log("Lava slowed!");
        yield return new WaitForSeconds(3f);
        currentRiseSpeed = oldSpeed;
        slowing = false;
        Debug.Log("Lava speed restored!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(1);
                ResetLavaBelowPlayer(playerController.transform.position);
            }
        }
    }
}
