using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    public AudioClip lavaSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LavaManager.Instance.StartLavaRise();
            AudioSource.PlayClipAtPoint(lavaSound, transform.position);
            Destroy(gameObject); // trigger once
        }
    }
}
