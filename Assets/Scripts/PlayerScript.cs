using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashForce = 15f;
    public float wallJumpHorizontalPush = 5f;
    public string losingscreen = "Lose_Scene";

    [Header("Fall Damage Settings")]
    public float fallDamageThreshold = 15f;

    [Header("Wall Settings")]
    public float wallGrabDuration = 0.2f;
    [Tooltip("Slide gravity = initialGravityScale * wallSlideGravity")]
    public float wallSlideGravity = 0.5f;

    [Header("Dash Settings")]
    public float dashDuration = 0.2f;

    [Header("Water Balloon Ability")]
    public GameObject waterBalloonPrefab;
    private bool hasWaterBalloon;

    [Header("Player Health")]
    public int maxHearts = 3;
    private int currentHearts;

    [Header("Collectables")]
    private int keysCollected = 0;

    [Header("Respawn Settings")]
    private Vector3 lastPlatformPosition;
    private bool hasValidRespawnPoint;

    [Header("Sounds")]
    public AudioClip woodSound;
    public AudioClip wingSound;
    public AudioClip heartSound;
    public AudioClip keySound;
    public AudioClip deathsound;

    [Header("Key settings")]
    public int maxKeys = 3;
    private int currentKeys = 0;

    public int GetCurrentKeys() => currentKeys;

    public void AddKey()
    {
        if (currentKeys < maxKeys)
            currentKeys++;
        Debug.LogError("addkey works!");
    }

    public PlayerController playerController;

    // Components
    private Rigidbody2D rb;
    private float initialGravityScale;
    public CameraFollow cameraFollow;

    // Ground detection
    private bool isGrounded;
    private float lastYVelocity;

    // Wall detection
    private bool isTouchingWall;
    private int wallDirection;

    // Wall grab state
    private bool isWallGrabbing;
    private float wallGrabTimer;

    // Skills
    private bool hasExtraJump;
    private bool extraJumpAvailable;
    private bool hasDash;
    private bool hasClaws;

    // Dash state
    private bool isDashing;
    private float dashEndTime;

    // Movement control
    private float facingDirection = 1f;
    private bool wallJumpActive;
    private float wallJumpEndTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialGravityScale = rb.gravityScale;
        currentHearts = maxHearts;
        keysCollected = 0;
    }

    void Update()
    {
        if (rb == null) return;

        lastYVelocity = rb.linearVelocity.y;

        if (isDashing)
        {
            if (Time.time >= dashEndTime)
                EndDash();
            return;
        }

        if (wallJumpActive)
        {
            if (Time.time >= wallJumpEndTime)
                wallJumpActive = false;
            else
                return;
        }

        if (hasWaterBalloon && Input.GetKeyDown(KeyCode.E))
            ThrowWaterBalloon();

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0f)
            facingDirection = Mathf.Sign(moveInput);

        if (isTouchingWall)
        {
            bool movingAway = (wallDirection == 1 && moveInput < 0f) || (wallDirection == -1 && moveInput > 0f);
            if (movingAway)
            {
                isTouchingWall = false;
                StopWallGrab();
            }
        }

        if (isTouchingWall)
        {
            bool pressingIntoWall = (wallDirection == 1 && moveInput > 0f) || (wallDirection == -1 && moveInput < 0f);
            if (pressingIntoWall)
                moveInput = 0f;
        }

        if (!isWallGrabbing)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump")||Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            /////////////ISSUE AREA FOR JUMP BREAK, FIX ASAP
            Debug.Log("Jump Pressed");
            if (isGrounded)
            {
                Jump();
                AudioSource.PlayClipAtPoint(woodSound, transform.position);
                Debug.Log("SHOULD JUMP1");
            }
            //else if ((hasClaws && isTouchingWall) && (isGrounded)|| (hasClaws && isTouchingWall))
            //{
              //  WallJump();
                //Debug.Log("SHOULD WALLJUMP9");
           // }
            //Debug.Log("Wall Jump Pressed");
            else if ((isWallGrabbing) || (hasClaws && isTouchingWall))
            {
                Debug.Log("Wall Jump Pre");
                if (hasClaws && isTouchingWall)
                {
                   WallJump();
                    Debug.Log("SHOULD WALLJUMP1"); 
                }
            }
          //  else if (isTouchingWall && !hasClaws)
            //{
              ///  Jump();
                //AudioSource.PlayClipAtPoint(woodSound, transform.position);
                //Debug.Log("SHOULD JUMP2");
           // }
            else if (hasExtraJump && extraJumpAvailable)
            {
                Jump();
                AudioSource.PlayClipAtPoint(wingSound, transform.position);
                extraJumpAvailable = false;
                Debug.Log("SHOULD JUMP3");
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && hasDash && !isDashing)
        {
            float dashDir = (Mathf.Approximately(moveInput, 0f)) ? facingDirection : Mathf.Sign(moveInput);
            StartDash(dashDir);
        }

        HandleWallGrab();
    }

    private void HandleWallGrab()
    {
        if (!hasClaws || !isTouchingWall || isGrounded || isDashing)
        {
            if (isWallGrabbing)
                StopWallGrab();
            return;
        }

        if (!isWallGrabbing)
        {
            isWallGrabbing = true;
            wallGrabTimer = wallGrabDuration;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            extraJumpAvailable = true;
        }

        if (isWallGrabbing)
        {
            wallGrabTimer -= Time.deltaTime;
            if (wallGrabTimer <= 0f)
                rb.gravityScale = initialGravityScale * wallSlideGravity;
            else
                rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            keysCollected++;
            Debug.Log("Key collected! Total keys: " + keysCollected);
            AudioSource.PlayClipAtPoint(keySound, transform.position);
            Destroy(other.gameObject);
            AddKey();
        }
    }

    public int GetKeyCount() => keysCollected;
    public int GetCurrentHearts() => currentHearts;
    public void ResetKeys() => keysCollected = 0;

    private void StopWallGrab()
    {
        isWallGrabbing = false;
        rb.gravityScale = initialGravityScale;
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        Debug.Log("Not Grounded1");
    }

    private void WallJump()
    {
        float pushDirection = -wallDirection;
        rb.linearVelocity = new Vector2(pushDirection * wallJumpHorizontalPush, jumpForce);
        isWallGrabbing = false;
        isTouchingWall = false;
        rb.gravityScale = initialGravityScale;
        extraJumpAvailable = true;
        isGrounded = false;
        Debug.Log("Not GROUNDED2");
        wallJumpActive = true;
        wallJumpEndTime = Time.time + 0.15f;
    }

    private void StartDash(float dirSign)
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(dirSign * dashForce, 0f);
    }

    private void EndDash()
    {
        isDashing = false;
        rb.gravityScale = initialGravityScale;
    }

    private void ThrowWaterBalloon()
    {
        if (waterBalloonPrefab == null) return;
        Vector3 spawnPos = transform.position + Vector3.down * 0.5f;
        GameObject balloon = Instantiate(waterBalloonPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rbBalloon = balloon.GetComponent<Rigidbody2D>();
        if (rbBalloon != null)
            rbBalloon.linearVelocity = new Vector2(0, -10f);
        Debug.Log("Water balloon thrown!");
    }

    public void UnlockSkill(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.Wings: hasExtraJump = true; extraJumpAvailable = true; break;
            case SkillType.Claws: hasClaws = true; break;
            case SkillType.Dash: hasDash = true; break;
            case SkillType.WaterBalloon: hasWaterBalloon = true; break;
        }
        Debug.Log(skill + " unlocked!");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform") || collision.collider.CompareTag("Ground") || collision.collider.CompareTag("UnsafePlatform"))
        {
            bool foundGround = false;
            Debug.Log("no ground found1.1");
            if (collision.collider.CompareTag("Platform"))
            {
                Debug.Log("collide platform");
            }
            else if (collision.collider.CompareTag("Ground"))
            {
                Debug.Log("collide ground");
            }
            else if (collision.collider.CompareTag("UnsafePlatform"))
            {
                Debug.Log("collide unsafe platform");
            }

            foreach (var contact in collision.contacts)
            {
                // grounded
                if (contact.normal.y > 0.5f)
                {
                    ///moved from here
                    float impactSpeed = Mathf.Abs(lastYVelocity);
                    if (!isGrounded && impactSpeed > fallDamageThreshold)
                        TakeDamage(1);

                    if (collision.gameObject.CompareTag("Platform"))
                    {
                        lastPlatformPosition = transform.position;
                        hasValidRespawnPoint = true;
                    }
                    foundGround = true;
                    isGrounded = true;
                    Debug.Log("is grounded1");
                    extraJumpAvailable = true;
                    rb.gravityScale = initialGravityScale;
                    if ((collision.collider.CompareTag("Wall")&& (collision.collider.CompareTag("Ground")))
                    || (collision.collider.CompareTag("Wall")&& (collision.collider.CompareTag("Platform"))
                    || ((collision.collider.CompareTag("Wall") && collision.collider.CompareTag("UnsafePlatform")))))
                    {
                        isTouchingWall = true;
                        isGrounded = true;
                        Debug.Log("is grounded7");
                        Debug.Log("is touchingwall7");
                    }
                    //if (Input.GetButtonDown("Jump"))
                   /// {
                   /// Jump();
                    ///AudioSource.PlayClipAtPoint(woodSound, transform.position);
                    ///Debug.Log("SHOULD JUMP2.5");
                    ///}
                }
                /////////TRYING SOMETHING
                ///else
                ///{
                   /// if (Input.GetButtonDown("Jump"))
                    //{
                    //Jump();
                    //AudioSource.PlayClipAtPoint(woodSound, transform.position);
                    //Debug.Log("SHOULD JUMP2.2");
                    //}
                //}

                // Side contact
                if (Mathf.Abs(contact.normal.x) > 0.5f && contact.normal.y < 0.2f)
                {
                    bool pushingIntoWall = Mathf.Abs(rb.linearVelocity.x) < 0.1f;

                    if (pushingIntoWall)
                    {
                        isTouchingWall = true;
                        wallDirection = (contact.normal.x > 0f) ? -1 : 1;
                    }
                    else
                    {
                        // Player is still moving
                        isTouchingWall = false;
                    }
                }
            }

            if (foundGround)
                return;
        }
        else if (collision.collider.CompareTag("Wall"))
            {
                ///trying something
                Debug.Log("collide wall");
                isTouchingWall = true;
            }

        // Enemy Collision
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            return;
        }

        // Real wal collisions
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
            if (collision.contacts.Length > 0)
            {
                float n = collision.contacts[0].normal.x;
                wallDirection = (n > 0f) ? -1 : 1;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
       // if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("UnsafePlatform"))
         //   isGrounded = false;
           // Debug.Log("Not Grounded4");
            //Debug.Log("No longer in contact with " + collision.transform.name);

        if ((collision.gameObject.CompareTag("Wall")) || (collision.gameObject.CompareTag("Platform")) || (collision.collider.CompareTag("UnsafePlatform"))|| (collision.gameObject.CompareTag("Ground")))
        {
            isTouchingWall = false;
            isGrounded = true;
            Debug.Log("IS Grounded9");
            StopWallGrab();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
        AudioSource.PlayClipAtPoint(heartSound, transform.position);

        if (currentHearts <= 0)
        {
            cameraFollow.UpdateHeartDisplay();
            Die();
        }
        else
        {
            RespawnAtLastPlatform();
        }
    }

    void RespawnAtLastPlatform()
    {
        if (hasValidRespawnPoint)
        {
            transform.position = lastPlatformPosition;
            rb.linearVelocity = Vector2.zero;
            isGrounded = false;
            Debug.Log("not grounded5");
            isTouchingWall = false;
            isWallGrabbing = false;
            isDashing = false;
            wallJumpActive = false;
            rb.gravityScale = initialGravityScale;
        }
    }

    void Die()
    {
        Debug.Log("Game Over!");
        AudioSource.PlayClipAtPoint(deathsound, transform.position);
        Time.timeScale = 1f;
        Destroy(gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene(losingscreen);
    }

    void LoseHeart(int amount)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
    }
}