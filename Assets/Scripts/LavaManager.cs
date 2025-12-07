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
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            currentRiseSpeed = baseRiseSpeed;
            
            // CHANGED: Use transform.position.x instead of player.position.x
            // This keeps the lava where you placed it in the editor horizontally
            transform.position = new Vector3(transform.position.x, player.position.y - 20f, 0); 
        }
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
        // CHANGED: We now use 'transform.position.x' (Lava's X) 
        // instead of 'playerPos.x' (Player's X).
        transform.position = new Vector3(transform.position.x, playerPos.y - safeOffset, transform.position.z);
        
        StopLavaTemporarily();
    }

    public void SlowLava()
    {
        if (slowing) return;
        StartCoroutine(SlowLavaRoutine());
    }

    private IEnumerator SlowLavaRoutine()
    {
        slowing = true;
        float originalSpeed = currentRiseSpeed;
        
        // Slow down
        currentRiseSpeed = baseRiseSpeed / 4f; 
        Debug.Log("Lava slowed!");
        
        yield return new WaitForSeconds(3f);
        
        // Restore speed
        currentRiseSpeed = baseRiseSpeed; 
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