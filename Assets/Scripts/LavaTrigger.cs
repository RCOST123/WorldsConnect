using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    public AudioSource lavaSound;

    void Start()
    {
        lavaSound = GameObject.Find("Lava_Sound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LavaManager.Instance.StartLavaRise();
            lavaSound.Play();
            //AudioSource.PlayClipAtPoint(lavaSound, transform.position);
            Destroy(gameObject); // trigger once
        }
    }
}
