using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LavaManager.Instance.StartLavaRise();
            Destroy(gameObject); // trigger once
        }
    }
}
