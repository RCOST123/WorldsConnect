using UnityEngine;

public class MovetoDoorScript : MonoBehaviour
{
    public GameObject nextdoor;
    public Vector3 doorpos;
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
       if (collision.gameObject.CompareTag("Door"))
       {
            transform.position = doorpos;
       }
    }
}
