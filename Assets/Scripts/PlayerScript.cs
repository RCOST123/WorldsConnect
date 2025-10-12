using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float player_y = 0f;
    private float updatedplayer_y = 0f;
    public string upKey = "up";
     public string leftKey = "left";
    public string rightKey = "right";

    [Header("Player Health")]
    public int maxHearts = 5;
    int currentHearts;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;
    public GameObject player;

    [Header("Sounds")]
    public AudioClip jumpSound;

    //Rigidbody2D rb;
    bool isGrounded;
    
    // Skills
    bool hasExtraJump;
    bool extraJumpAvailable;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        currentHearts = maxHearts;
    }

    void Update()
    {
        ///FIX THIS
        //float player_y;
        //player_y = transform.position.y;
        //Debug.Log($"Player y position: {player_y}");
        // Horizontal movement
        if (Input.GetKey("left"))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("right"))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }

        //float moveInput = Input.GetAxis("Horizontal");
        //rb.linearVelocity = new Vector2(moveInput * moveSpeed * Time.deltaTime, rb.linearVelocity.y);

        // Jumping
        if (Input.GetKey("up"))
        {
            Jump();
            //if (isGrounded)
            // {
            // Jump();
            //} deleted else on next line
            if (hasExtraJump && extraJumpAvailable)
            {
                Jump();
                Jump();
                //extraJumpAvailable = false;
            }
        }
    }

    void Jump()
    {
        transform.Translate(0, jumpForce * Time.deltaTime, 0);
        AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * Time.deltaTime);
    }

    // Called from SkillPickup when a skill is collected
    public void UnlockSkill(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.Wings:
                hasExtraJump = true;
                Debug.Log("Wings acquired — double jump unlocked.");
                break;

            case SkillType.Claws:
                Debug.Log("Claws acquired — wall climbing coming soon.");
                break;

            case SkillType.Dash:
                Debug.Log("Dash acquired — speed boost coming soon.");
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            ///NEED TO FIX THIS
            ///  isGrounded = true;
            //float player_y;
            //float updatedplayer_y;
            player_y = transform.position.y;
            Debug.Log($"Player y position: {player_y}");
            //if (player_y < 0f && updatedplayer_y < 0f)
            // {
            //   break;
            // }
            if (player_y < 0f && updatedplayer_y > 0f)
            {
                if (updatedplayer_y + player_y < -2f)
                {
                    LoseHeart();
                }
            } 
            else if (player_y > 0f && updatedplayer_y > 0f)
            {
                if (updatedplayer_y - player_y < -2f)
                {
                    LoseHeart();
                }
            } 
            updatedplayer_y = transform.position.y;
            Debug.Log($"Updated Player y position: {updatedplayer_y}");
         ///else
           // {
             //   isGrounded = false;
            //}
            //foreach (var contact in collision.contacts)
            //{
            //Debug.Log($"contact normal is {contact.normal.y}");
            //if (contact.normal.y > 1f)
            //{
            //if (collision.gameObject.CompareTag("Ground") && !isGrounded)
            //LoseHeart();
            //isGrounded = true;
            //extraJumpAvailable = true;
            //break;
            //}
            //}
       }
   }

    //void OnCollisionExit2D(Collision2D collision)
    //{
      //  if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        //    isGrounded = true;
            ///made change here
    //}

    void LoseHeart()
    {
        currentHearts = currentHearts - 1;
        if (currentHearts == 4)
        {
            Destroy(heart5);
        }
        else if (currentHearts == 3)
        {
            Destroy(heart4);
        }
        else if (currentHearts == 2)
        {
            Destroy(heart3);
        }
        else if (currentHearts == 1)
        {
            Destroy(heart2);
        }
        else if (currentHearts == 0)
        {
            Destroy(heart1);
            Destroy(player);
            Debug.Log("Player is out of hearts — Game Over.");
        }
        Debug.Log($"Player landed — hearts left: {currentHearts}");

    }
}
