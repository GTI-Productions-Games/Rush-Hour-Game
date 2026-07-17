using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float sprintMultiplier = 2;
    [SerializeField] private float moveSmooth = 10;
    [SerializeField] private float staminaConsume = 7;
    [SerializeField] private float staminaRegen = 20;

    [Header("Ground Check Config")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attachments")]
    [SerializeField] private GameObject mainSprite;
    [SerializeField] private Animator animator;

    [Header("Dev")]
    [SerializeField] private bool seeGroundCheck;

    private ControllerInput input;
    private Rigidbody2D rb;
    private PlayerAttackController attack;
    private PlayerStats stats;
    private PlayerCollisions collisions;

    private GameManager gameManager;

    private float moveDirection;
    private bool isRunning;
    private bool jumpTrigger;
    private bool isGrounded;

    public bool IsFacingRight => mainSprite.transform.localScale.x > 0;

    private void Awake()
    {
        attack = GetComponent<PlayerAttackController>();
        stats = GetComponent<PlayerStats>();
        collisions = GetComponent<PlayerCollisions>();
        rb = GetComponent<Rigidbody2D>();

        gameManager = FindAnyObjectByType<GameManager>();

        InitializeInput();
    }

    #region Inpit Init
    private void InitializeInput()
    {
        input = new ControllerInput();
        input.Enable();

        InitializeMovement();
    }

    private void InitializeMovement()
    {
        input.Move.Walk.performed += ctx =>
        {
            moveDirection = ctx.ReadValue<float>();
        };

        input.Move.Walk.canceled += ctx =>
        {
            moveDirection = 0;
        };

        input.Move.Run.performed += ctx =>
        {
            isRunning = true;
        };

        input.Move.Run.canceled += ctx =>
        {
            isRunning = false;
        };

        input.Move.Jump.performed += ctx =>
        {
            jumpTrigger = true;
        };
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    #endregion

    private void Update()
    {
        UpdateGroundedState();
        GetMovementAnimation();
        ConsumeStamina();

        if (gameManager.stopAllMovementsOverride)
        {
            jumpTrigger = false;
        }
    }

    private void FixedUpdate()
    {
        if (stats.isPlayerDead || gameManager.stopAllMovementsOverride)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (collisions.isHit)
        {
            return;
        }

        MovePlayer();
        HandleJump();
    }

    private void MovePlayer()
    {
        float trueSpeed = 0;

        if (isRunning && stats.playerStamina >= 5)
        {            
            trueSpeed = moveSpeed * sprintMultiplier;            
        }
        else
        {
            trueSpeed = moveSpeed;            
        }

        Vector2 targetVelocity = new Vector2(moveDirection * trueSpeed, rb.linearVelocity.y);

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, moveSmooth * Time.fixedDeltaTime);
    }

    private void ConsumeStamina()
    {
        if (isRunning)
        {
            stats.playerStamina -= Time.deltaTime * staminaConsume;            
        }
        else
        {
            stats.playerStamina += Time.deltaTime * staminaRegen;
        }

        stats.playerStamina = Mathf.Clamp(stats.playerStamina, 0, stats.playerMaxStamina);
    }

    private void HandleJump()
    {
        if (!jumpTrigger)
        {
            return;
        }

        jumpTrigger = false;

        if (!isGrounded)
        {
            return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    private void UpdateGroundedState()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void GetMovementAnimation()
    {
        bool canAnimateMovement = !stats.isPlayerDead && !gameManager.stopAllMovementsOverride;

        if (!canAnimateMovement)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            return;
        }

        bool isWalking = Mathf.Abs(moveDirection) > 0.01f;
        bool isRunningNow = isWalking && isRunning && stats.playerStamina >= 5;

        animator.SetBool("Walk", isWalking);
        animator.SetBool("Run", isRunningNow);

        if (!isWalking)
        {
            return;
        }

        Vector3 scale = mainSprite.transform.localScale;

        if (moveDirection > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else if (moveDirection < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        mainSprite.transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (seeGroundCheck)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }               
    }
}