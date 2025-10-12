using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject nextdoor;
    public bool door_on = true;
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
       if (collision.gameObject.CompareTag("Door") && door_on)
       {
           transform.position = nextdoor.transform.position;
       }
    }
}
