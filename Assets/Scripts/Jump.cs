using UnityEngine;

public class Jump : MonoBehaviour
{
    public string upKey = "up";
    public string downKey = "down";
    public string leftKey = "left";
    public string rightKey = "right";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check control keys
        if (Input.GetKey("up"))
        {
            transform.Translate(0, 7f * Time.deltaTime, 0);
        }
        else if (Input.GetKey("down"))
        {
            transform.Translate(0, -7f * Time.deltaTime, 0);
        }
        else if (Input.GetKey("left"))
        {
            transform.Translate(-6f * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("right"))
        {
            transform.Translate(6f * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("right") && Input.GetKey("up"))
        {
            transform.Translate(6f * Time.deltaTime, 7f * Time.deltaTime, 0);
        }
        else if (Input.GetKey("right") && (Input.GetKey("down")))
        {
            transform.Translate(6f * Time.deltaTime, -7f * Time.deltaTime, 0);
        }
        else if (Input.GetKey("left") && (Input.GetKey("up")))
        {
            transform.Translate(-6f * Time.deltaTime, 7f * Time.deltaTime, 0);
        }
        else if (Input.GetKey("left") && (Input.GetKey("down")))
        {
            transform.Translate(-6f * Time.deltaTime, -7f * Time.deltaTime, 0);
        }
    }
}
