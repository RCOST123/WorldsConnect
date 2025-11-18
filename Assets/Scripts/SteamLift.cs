using UnityEngine;

public class SteamLift : MonoBehaviour
{
    public float liftForce = 15f; // tweak until it feels right
    public AudioClip windSound;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce(Vector2.up * liftForce * Time.deltaTime, ForceMode2D.Impulse);
                AudioSource.PlayClipAtPoint(windSound, transform.position);
            }
        }
    }
}
