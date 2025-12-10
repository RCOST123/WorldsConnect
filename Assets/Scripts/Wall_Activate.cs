using UnityEngine;

public class Wall_Activate : MonoBehaviour
{
    [Header("Sounds")]
    public PlayerController player;
    public AudioSource wallupSound;
    public AudioSource walldownSound;
    public GameObject wallchangingobject;
    public GameObject walluptrigger;
    public int minkeydown = 2;
    public int j = 1;

    void Start()
    {
        wallchangingobject.SetActive(false);
        wallupSound = GameObject.Find("WallUp_Sound").GetComponent<AudioSource>();
        walldownSound = GameObject.Find("WallDown_Sound").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player.GetKeyCount() >= minkeydown)
        {
            Debug.Log("Wall down");
            wallchangingobject.SetActive(false);
            if (j == 1)
            {
                walldownSound.Play();
                //AudioSource.PlayClipAtPoint(walldownSound, transform.position);
                j = j - 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("walluptrigger"))
        {
            //AudioSource.PlayClipAtPoint(wallupSound, transform.position);
            wallupSound.Play();
            wallchangingobject.SetActive(true);
            Destroy(walluptrigger);
        }
    }
}
