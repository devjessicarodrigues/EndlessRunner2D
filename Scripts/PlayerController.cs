using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    PlayerInputs inputActions;
    public float speed = 3f;
    SpriteRenderer sprite;
    public Animator animator;
    public Rigidbody2D rb;
    bool canJump = true;
    bool canAttack = true;
    public ParticleSystem dust;
    public GameManager gameManager;
    public bool isPaused;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip shotSound;

    [Header("Shot")]
    public GameObject shot;
    public float shotForce = 7f; 

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public TrailRenderer trailRenderer;

    [Header("Double Jump")]
    public int extraJumpsValue = 1;
    private int extraJumps;
    public float jumpForce = 5f;

    [Header("Fall Damage")]
    public float fallThreshold = -10f;

    [Header("Life")]
    private int life;
    private int maxHealth = 3;
    public Image heartOn;
    public Image heartOff;
    public Image heartOn2;
    public Image heartOff2;

    public ScoreManager scoreManager;

    void Awake()
    {
        inputActions = new PlayerInputs();
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue;
        life = maxHealth;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (isDashing)
        {
            dust.Stop();
            return;
        }
        if (isPaused) return;

        var moveInputs = inputActions.Player.Movement.ReadValue<Vector2>();
        transform.position += speed * Time.deltaTime * new Vector3(moveInputs.x, 0, 0);

        animator.SetBool("isWalking", moveInputs.x != 0);
        if (moveInputs.x != 0)
        {
            sprite.flipX = moveInputs.x < 0;
            CreateDust();

            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkSound;
                audioSource.Play();
            }
        }
        else
        {
            dust.Stop();
            audioSource.Stop();
        }

        canJump = Mathf.Abs(rb.velocity.y) <= 0.001f;
        if (canJump) extraJumps = extraJumpsValue;
        animator.SetBool("isJumping", !canJump);

        HandlerJumpAction();
        HandlerAttack();

        if (Input.GetKeyDown(KeyCode.LeftShift) || inputActions.Player.Dash.WasPressedThisFrame())  
        {
            if (canDash && !isDashing)
            {
                StartCoroutine(Dash());
            }
        }
        Fall();
    }

    void HandlerJumpAction()
    {
        var jumpPressed = inputActions.Player.Jump.WasPressedThisFrame();

        if (jumpPressed)
        {
            if (canJump)
            {
                rb.velocity = Vector2.up * jumpForce;

                audioSource.PlayOneShot(jumpSound);
            }
            else if (extraJumps > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumps--;
                CreateDust();

                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    void HandlerAttack()
    {
        var attackPressed = inputActions.Player.Attack.IsPressed();
        if (canAttack && attackPressed)
        {
            canAttack = false;
            animator.SetTrigger("Attack");

            audioSource.PlayOneShot(shotSound);
        }
    }

    public void ShotNewEgg()
    {
        var newShot = GameObject.Instantiate(shot);
        newShot.transform.position = transform.position;

        var isLookRight = !sprite.flipX;
        Vector2 shotDirection = shotForce * new Vector2(isLookRight ? -1 : 1, 0);
        newShot.GetComponent<Rigidbody2D>().AddForce(shotDirection, ForceMode2D.Impulse);
        newShot.GetComponent<SpriteRenderer>().flipY = !isLookRight;
        Destroy(newShot, 3f);
    }

    public void SetCanAttack()
    {
        canAttack = true;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        animator.SetBool("isDashing", true);
        trailRenderer.emitting = true;

        audioSource.PlayOneShot(dashSound);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashDirection = sprite.flipX ? -1f : 1f;
        rb.velocity = new Vector2(dashDirection * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
        trailRenderer.emitting = false;
        animator.SetBool("isDashing", false);

        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void Die()
    {
        animator.SetTrigger("Die");
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Damage()
    {
        life -= 1;

        if (life == 2)
        {
            heartOn2.enabled = true;
            heartOff2.enabled = false;
        }
        else
        {
            heartOn2.enabled = false;
            heartOff2.enabled = true;
        }
        if (life == 1)
        {
            heartOn2.enabled = true;
            heartOff2.enabled = false;

            heartOn.enabled = true;
            heartOff.enabled = false;
        }
        else
        {
            heartOn.enabled = false;
            heartOff.enabled = true;
        }
        if (life <= 0)
        {
            Die();
            Debug.Log("Game Over");
            StartCoroutine(ShowGameOverScreen());
        }
    }

    private IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(0.4f);
        if (scoreManager != null)
        {
            scoreManager.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Damage");
            Damage();
        }
    }

    private void Fall()
    {
        if (transform.position.y < fallThreshold)
        {
            life = 0;
            Damage();
        }
    }

    private void CreateDust()
    {
        if (!dust.isPlaying)
        {
            dust.Play();
        }
    }
    public void PauseMovement()
    {
        isPaused = true;
    }
    public void ResumeMovement()
    {
        isPaused = false;
    }
}
