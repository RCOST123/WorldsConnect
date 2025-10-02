using UnityEngine;

public class Hearts : MonoBehaviour
{
    public int HeartsCount = 5;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;
    public GameObject player;
    void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                HeartsCount = HeartsCount - 1;
                if (HeartsCount == 4)
                {
                    Destroy(heart5);
                }
                else if (HeartsCount == 3)
                {
                    Destroy(heart4);
                }
                else if (HeartsCount == 2)
                {
                    Destroy(heart3);
                }
                else if (HeartsCount == 1)
                {
                    Destroy(heart2);
                }
                else if (HeartsCount == 0)
                {
                // Co-pilot wrote the next line
                Destroy(heart1);
                Destroy(player);
                }
            }
        }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
