using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovetoDoorScript : MonoBehaviour
{
    //public GameObject nextdoor;
    //public Vector3 doorpos;
    public string nextsceneName = "level";
    //Co-pilot wrote the next line
    public AudioSource winaudio; 
   //public AudioClip successSound;
    ///public bool door_on = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
    //Co-pilot wrote the next line
    winaudio = GameObject.Find("Win_Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ///doorpos = nextdoor.transform.position;
       if (collision.gameObject.CompareTag("UnlockedDoor"))
       {
            //transform.position = doorpos;
            //AudioSource.PlayClipAtPoint(successSound, transform.position);
            winaudio.Play();
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextsceneName);
       }
    }
}
