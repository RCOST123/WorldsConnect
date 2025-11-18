using UnityEngine;

public class Wall_Activate : MonoBehaviour
{
    [Header("Sounds")]
    public PlayerController player;
    public AudioClip wallupSound;
    public AudioClip walldownSound;
    public GameObject wallchangingobject;
    public GameObject walluptrigger;
    public int minkeydown = 2;
    public int j = 1;

    void Start()
    {
        wallchangingobject.SetActive(false);
    }

    void Update()
    {
        if (player.GetKeyCount() >= minkeydown)
        {
            Debug.Log("Wall down");
            wallchangingobject.SetActive(false);
            if (j == 1)
            {
                AudioSource.PlayClipAtPoint(walldownSound, transform.position);
                j = j - 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("walluptrigger"))
        {
            AudioSource.PlayClipAtPoint(wallupSound, transform.position);
            wallchangingobject.SetActive(true);
            Destroy(walluptrigger);
        }
    }
}
