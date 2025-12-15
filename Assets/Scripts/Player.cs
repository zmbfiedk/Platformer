using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static event Action<int> OnCoinPickup;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int health;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxExtraJumps = 1; // double jump
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.15f;

    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Squash & Stretch")]
    [SerializeField] private float squashX = 1.2f;
    [SerializeField] private float squashY = 0.8f;
    [SerializeField] private float squashTime = 0.1f;

    [Header("UI")]
    [SerializeField] private Image healthImage;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    private bool isPaused = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private bool isGrounded;
    private int extraJumpsLeft;

    private float jumpBufferCounter;
    private float coyoteCounter;

    private Vector3 originalScale;
    private bool isSquashing = false;
    private bool isInvincible = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        health = maxHealth;
        extraJumpsLeft = maxExtraJumps;

        originalScale = transform.localScale;
    }

    private void Update()
    {
        HandleMovement();
        HandleJumpInput();
        HandleAnimations();
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void FixedUpdate()
    {
        CheckSurroundings();
        HandleJump();
    }

    // ---------------- CHECKS ----------------

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            extraJumpsLeft = maxExtraJumps;

            // Stop squash when grounded
            if (isSquashing)
            {
                StopAllCoroutines();
                transform.localScale = originalScale;
                isSquashing = false;
            }
        }
        else
        {
            coyoteCounter -= Time.fixedDeltaTime;
        }
    }

    // ---------------- MOVEMENT ----------------

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        if (x != 0)
            sprite.flipX = x < 0;
    }

    // ---------------- JUMP ----------------

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    private void HandleJump()
    {
        if (jumpBufferCounter <= 0) return;

        if (isGrounded || coyoteCounter > 0)
        {
            DoJump();
        }
        else if (extraJumpsLeft > 0)
        {
            extraJumpsLeft--;
            DoJump();
        }
    }

    private void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpBufferCounter = 0;
        coyoteCounter = 0;

        if (!isSquashing)
            Squash(0.8f, 1.2f);
    }

    // ---------------- SQUASH ----------------

    private void Squash(float x, float y)
    {
        StopAllCoroutines();
        StartCoroutine(SquashRoutine(x, y));
    }

    private IEnumerator SquashRoutine(float x, float y)
    {
        isSquashing = true;
        transform.localScale = new Vector3(originalScale.x * x, originalScale.y * y, 1);
        yield return new WaitForSeconds(squashTime);
        if (!isGrounded)
            transform.localScale = originalScale;
        isSquashing = false;
    }

    // ---------------- ANIMATIONS ----------------

    private void HandleAnimations()
    {
        
        animator.SetBool("Run", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        animator.SetBool("Jump", rb.linearVelocity.y > 0.1f && !isGrounded);
        animator.SetBool("Fall", rb.linearVelocity.y < -0.1f && !isGrounded);
    }

    // ---------------- HEALTH ----------------

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        health -= amount;
        if (health <= 0) Die();
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    // ---------------- POWERUP METHODS ----------------

    public void SetInvincible(float duration)
    {
        StartCoroutine(InvincibleRoutine(duration));
    }

    private IEnumerator InvincibleRoutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    public void AddExtraJump(int amount)
    {
        maxExtraJumps += amount;
        extraJumpsLeft = maxExtraJumps;
    }

    // ---------------- UI ----------------

    private void UpdateUI()
    {
        if (healthImage)
            healthImage.fillAmount = (float)health / maxHealth;
    }

    // ---------------- PAUSE MENU ----------------

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    //---------------- GIZMOZ ----------------
    private void OnDrawGizmosSelected()
    {
        if (transform == null) return;
        Gizmos.color = Color.red;
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
