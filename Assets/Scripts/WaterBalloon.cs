using UnityEngine;

public class WaterBalloon : MonoBehaviour
{
    public float lifetime = 3f; // auto-destroy timer
    public GameObject splashEffect; // prefab for splash particle
    public AudioSource splashSound; // optional sound
    // private AudioSource audioSource; // Uncomment if you add an AudioSource component

    void Start()
    {
        splashSound = GameObject.Find("Water_Sound").GetComponent<AudioSource>();
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if we hit the Lava specifically
        if (other.CompareTag("Lava"))
        {
            // Call the slow function on the Singleton instance
            if (LavaManager.Instance != null)
            {
                LavaManager.Instance.SlowLava();
            }
            Burst();
        }
        // 2. Check other objects
        else if (other.CompareTag("Platform") || 
                 other.CompareTag("UnsafePlatform") || 
                 other.CompareTag("Enemy"))
        {
            Burst();
        }
    }

    void Burst()
    {
        // Spawn splash visual
        if (splashEffect)
        {
            Instantiate(splashEffect, transform.position, Quaternion.identity);
            splashSound.Play();
        }

        // Play sound logic would go here
        
        // Destroy self
        Destroy(gameObject); 
    }
}