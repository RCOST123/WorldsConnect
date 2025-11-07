using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashForce = 15f;
    public float wallJumpHorizontalPush = 5f;

    [Header("Fall Damage Settings")]
    public float fallDamageThreshold = 15f;

    [Header("Wall Settings")]
    public float wallGrabDuration = 0.2f;
    [Tooltip("Slide gravity = initialGravityScale * wallSlideGravity")]
    public float wallSlideGravity = 0.5f;

    [Header("Dash Settings")]
    public float dashDuration = 0.2f;

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

    // Components
    private Rigidbody2D rb;
    private float initialGravityScale;
    /// Co-pilot wrote the next line
    public CameraFollow cameraFollow;

    // Ground detection
    private bool isGrounded;
    private float lastYVelocity;

    // Wall detection
    private bool isTouchingWall;
    private int wallDirection; // -1 = left wall, +1 = right wall

    // Wall grab state
    private bool isWallGrabbing;
    private float wallGrabTimer;

    // Skills
    private bool hasExtraJump;
    private bool extraJumpAvailable;
    private bool hasDash;
    private bool canDash;
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


        // Get vertical velocity for impact
        lastYVelocity = rb.linearVelocity.y;

        // If we're currently dashing, check end and skip normal control while dashing
        if (isDashing)
        {
            if (Time.time >= dashEndTime)
                EndDash();
            return;
        }

        // If wall jump is active, wait for it to finish before allowing input control
        if (wallJumpActive)
        {
            if (Time.time >= wallJumpEndTime)
                wallJumpActive = false;
            else
                return; 
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        // Update facing direction
        if (moveInput != 0f)
            facingDirection = Mathf.Sign(moveInput);

        // If touching a wall and player is pressing away from it, release the wall
        if (isTouchingWall)
        {
            bool movingAway = (wallDirection == 1 && moveInput < 0f) || (wallDirection == -1 && moveInput > 0f);
            if (movingAway)
            {
                isTouchingWall = false;
                StopWallGrab();
            }
        }

        // If touching a wall, ignore input
        if (isTouchingWall)
        {
            bool pressingIntoWall = (wallDirection == 1 && moveInput > 0f) || (wallDirection == -1 && moveInput < 0f);
            if (pressingIntoWall)
                moveInput = 0f;
        }

        // Normal horizontal movement
        if (!isWallGrabbing)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                AudioSource.PlayClipAtPoint(woodSound, transform.position);
            }
            else if (isWallGrabbing || (hasClaws && isTouchingWall))
            {
                if (hasClaws && isTouchingWall)
                    WallJump();
            }
            else if (hasExtraJump && extraJumpAvailable)
            {
                Jump();
                AudioSource.PlayClipAtPoint(wingSound, transform.position);
                extraJumpAvailable = false;
            }
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && hasDash && !isDashing)
        {
            float dashDir = (Mathf.Approximately(moveInput, 0f)) ? facingDirection : Mathf.Sign(moveInput);
            StartDash(dashDir);
        }

        // Handle wall grabbing / sliding
        HandleWallGrab();
    }

    private void HandleWallGrab()
    {
        // Conditions to start/maintain wall grab - have claws, touching wall, not grounded, not dashing
        if (!hasClaws || !isTouchingWall || isGrounded || isDashing)
        {
            if (isWallGrabbing)
                StopWallGrab();
            return;
        }

        // Start grabbing if not already
        if (!isWallGrabbing)
        {
            isWallGrabbing = true;
            wallGrabTimer = wallGrabDuration;

            // Stick to wall
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;

            // Reset extra jump
            extraJumpAvailable = true;
        }

        // If still grabbing, count
        if (isWallGrabbing)
        {
            wallGrabTimer -= Time.deltaTime;

            if (wallGrabTimer <= 0f)
            {
                // Start sliding - reduced gravity
                rb.gravityScale = initialGravityScale * wallSlideGravity;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Key collection
        if (other.CompareTag("Key"))
        {
            keysCollected++;
            Debug.Log("Key collected! Total keys: " + keysCollected);
            Destroy(other.gameObject);
        }
    }

    // Keycount
    public int GetKeyCount()
    {
        return keysCollected;
    }

    // Gethearts
    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    // Method to reset keys (useful for level restart)
    public void ResetKeys()
    {
        keysCollected = 0;
    }

    private void StopWallGrab()
    {
        isWallGrabbing = false;
        rb.gravityScale = initialGravityScale;
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        ///AudioSource.PlayClipAtPoint(woodSound, transform.position);
    }

    private void WallJump()
    {
        // If wall is on right (wallDirection = 1), push left (negative)
        // If wall is on left (wallDirection = -1), push right (positive)
        float pushDirection = -wallDirection;
        rb.linearVelocity = new Vector2(pushDirection * wallJumpHorizontalPush, jumpForce);

        // Reset wall states
        isWallGrabbing = false;
        isTouchingWall = false;

        // Restore gravity
        rb.gravityScale = initialGravityScale;

        // Give a jump reset
        extraJumpAvailable = true;
        isGrounded = false;

        wallJumpActive = true;
        wallJumpEndTime = Time.time + 0.15f;
    }

    private void StartDash(float dirSign)
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;

        // Remove gravity for a clean horizontal dash
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(dirSign * dashForce, 0f);
    }

    private void EndDash()
    {
        isDashing = false;
        rb.gravityScale = initialGravityScale;
    }

    public void UnlockSkill(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.Wings:
                hasExtraJump = true;
                extraJumpAvailable = true;
                break;
            case SkillType.Claws:
                hasClaws = true;
                break;
            case SkillType.Dash:
                hasDash = true;
                break;
        }
        Debug.Log(skill + " unlocked!");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle ground/platform contacts
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("UnsafePlatform"))
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    // Fall damage check
                    float impactSpeed = Mathf.Abs(lastYVelocity);
                    if (!isGrounded && impactSpeed > fallDamageThreshold)
                    {
                        TakeDamage(1);
                        Debug.Log("Impact speed: " + impactSpeed.ToString("F1"));
                    }

                    isGrounded = true;
                    extraJumpAvailable = true;

                    // Store platform position as respawn point
                    if (collision.gameObject.CompareTag("Platform"))
                    {
                        lastPlatformPosition = transform.position;
                        hasValidRespawnPoint = true;
                        Debug.Log("Respawn point saved at: " + lastPlatformPosition);
                    }

                    // Restore gravity
                    rb.gravityScale = initialGravityScale;
                    break;
                }
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }

        // Handle wall contacts
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;

            if (collision.contacts.Length > 0)
            {
                var n = collision.contacts[0].normal.x;
                // If normal.x > 0 -> wall is to the left, set wallDirection = -1
                wallDirection = (n > 0f) ? -1 : 1;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
            StopWallGrab();
        }
    }

    // all dmg triggers
    public void TakeDamage(int amount)
    {

        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
        Debug.Log("Damage taken! Hearts left: " + currentHearts);

        if (currentHearts <= 0)
        {
           ///Co-pilot wrote the next line
           cameraFollow.UpdateHeartDisplay();
            Die();
        }
        else
        {
            // Respawn at last platform
            RespawnAtLastPlatform();
        }
    }

    private void RespawnAtLastPlatform()
    {
        if (hasValidRespawnPoint)
        {
            // Teleport to last platform position
            transform.position = lastPlatformPosition;
            
            // Reset velocity
            rb.linearVelocity = Vector2.zero;
            
            // Reset states
            isGrounded = false;
            isTouchingWall = false;
            isWallGrabbing = false;
            isDashing = false;
            wallJumpActive = false;
            
            // Restore gravity
            rb.gravityScale = initialGravityScale;
            
        }
    }

    private void Die()
    {
        Debug.Log("Game Over!");
        Destroy(gameObject);
        // Add death logic here (reload scene, show game over screen, etc.)
    }

    private void LoseHeart(int amount)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
        Debug.Log("Hearts left: " + currentHearts);
    }
}
