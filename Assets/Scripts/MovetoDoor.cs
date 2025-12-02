using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovetoDoorScript : MonoBehaviour
{
    //public GameObject nextdoor;
    //public Vector3 doorpos;
    public string nextsceneName = "level";
    public AudioClip successSound;
    ///public bool door_on = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            AudioSource.PlayClipAtPoint(successSound, transform.position);
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextsceneName);
       }
    }
}
