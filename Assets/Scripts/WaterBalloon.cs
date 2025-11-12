using UnityEngine;

public class WaterBalloon : MonoBehaviour
{
    public float lifetime = 3f; // auto-destroy timer
    public GameObject splashEffect; // prefab for splash particle
    public AudioClip splashSound; // optional sound
    private AudioSource audioSource;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Sound here =>
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only destroy if it hits a valid object
        if (other.CompareTag("Platform") || 
            other.CompareTag("Lava") || 
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
        }

        // Play sound =>
        

        // Destroy self (delay slightly if sound exists)
        Destroy(gameObject, splashSound ? splashSound.length * 0.5f : 0f);
    }
}
