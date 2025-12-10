using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject nextdoor;
    public Vector3 doorpos;
    public AudioSource opendoorSound;
    
    ///public bool door_on = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     opendoorSound = GameObject.Find("Door_Sound").GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        ///doorpos = nextdoor.transform.position;
       if (collision.gameObject.CompareTag("Door"))
       {
           opendoorSound.Play();
           // AudioSource.PlayClipAtPoint(opendoorSound, transform.position);
            transform.position = doorpos;
       }
    }
}
