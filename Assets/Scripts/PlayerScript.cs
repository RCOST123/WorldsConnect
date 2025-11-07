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

        if (moveInput != 0f)
            facingDirection = Mathf.Sign(moveInput);

        // Release wall grab if pressing away
        if (isTouchingWall)
        {
            bool movingAway = (wallDirection == 1 && moveInput < 0f) || (wallDirection == -1 && moveInput > 0f);
            if (movingAway)
            {
                isTouchingWall = false;
                StopWallGrab();
            }
        }

        // Ignore input into wall
        if (isTouchingWall)
        {
            bool pressingIntoWall = (wallDirection == 1 && moveInput > 0f) || (wallDirection == -1 && moveInput < 0f);
            if (pressingIntoWall)
                moveInput = 0f;
        }

        // Horizontal movement
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

        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && hasDash && !isDashing)
        {
            float dashDir = (Mathf.Approximately(moveInput, 0f)) ? facingDirection : Mathf.Sign(moveInput);
            StartDash(dashDir);
        }

        // Handle wall grab / slide
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
            {
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
        if (other.CompareTag("Key"))
        {
            keysCollected++;
            Debug.Log("Key collected! Total keys: " + keysCollected);
            Destroy(other.gameObject);
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
        ///AudioSource.PlayClipAtPoint(woodSound, transform.position);
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
        // Handle ground/platform contacts and also side-as-wall logic
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("UnsafePlatform"))
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    float impactSpeed = Mathf.Abs(lastYVelocity);
                    if (!isGrounded && impactSpeed > fallDamageThreshold)
                    {
                        TakeDamage(1, false);
                        Debug.Log("Impact speed: " + impactSpeed.ToString("F1"));
                    }

                    isGrounded = true;
                    extraJumpAvailable = true;

                    // Store platform position as respawn point, but ONLY if it's a safe platform
                    if (collision.gameObject.CompareTag("Platform") && !collision.gameObject.CompareTag("UnsafePlatform"))
                    {
                        lastPlatformPosition = transform.position;
                        hasValidRespawnPoint = true;

                        Debug.Log("Respawn point saved at: " + lastPlatformPosition);
                    }
                    else if (collision.gameObject.CompareTag("UnsafePlatform"))
                    {
                        Debug.Log("Landed on unsafe platform - no respawn point saved");
                    }

                    rb.gravityScale = initialGravityScale;
                    break;
                }

                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    isTouchingWall = true;
                    wallDirection = (contact.normal.x > 0f) ? -1 : 1;
                }
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1, true); // enemy damage
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;

            if (collision.contacts.Length > 0)
            {
                var n = collision.contacts[0].normal.x;
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

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Platform"))
        {
            isTouchingWall = false;
            StopWallGrab();
        }
    }

    // Damage handling
    public void TakeDamage(int amount, bool fromEnemy)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
        Debug.Log("Damage taken! Hearts left: " + currentHearts + " (enemy damage: " + fromEnemy + ")");

        if (currentHearts <= 0)
        {
            Die();
        }
        else
        {
            RespawnAtLastPlatform();
        }
    }

    private void RespawnAtLastPlatform()
    {
        if (hasValidRespawnPoint)
        {
            transform.position = lastPlatformPosition;
            rb.linearVelocity = Vector2.zero;

            isGrounded = false;
            isTouchingWall = false;
            isWallGrabbing = false;
            isDashing = false;
            wallJumpActive = false;
            rb.gravityScale = initialGravityScale;

            Debug.Log("Respawned at last safe platform: " + lastPlatformPosition);
        }
        else
        {
            Debug.Log("No valid respawn point saved.");
        }
    }

    private void Die()
    {
        Debug.Log("Game Over!");
        // Add restart or game over logic here
    }

    private void LoseHeart(int amount)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
        Debug.Log("Hearts left: " + currentHearts);
    }
}