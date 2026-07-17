using UnityEngine;

public class EnemyMoveBehavior : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool chasePlayer = true;
    [SerializeField] private bool flipOnBumpWithOthers = false;
    [SerializeField] private bool chaseFaster = true;
    [SerializeField] private bool randomlyFlips = false;
    [SerializeField] private float[] randomFlipIntervals = { 5, 10 };

    [Header("General Attributes")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float flipCooldown = 0.15f;

    [Header("Optional Attributes")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float detectionRadius = 5f;

    [Header("Dector Attachments")]
    [SerializeField] private Transform groundCheckOffset;
    [SerializeField] private float checkGroundRadius = 0.5f;
    [SerializeField] private Transform wallCheckOffset;
    [SerializeField] private float checkWallRadius = 0.25f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask entityLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask objectsLayer;

    [Header("Dev Options")]
    [SerializeField] private bool showDetections;

    private EnemyStats stats;
    private EnemyAttackBehavior attack;
    private Animator animator;
    private Rigidbody2D rb;

    private Transform currentTarget;

    private GameManager gameManager;

    private bool facingRight;
    private bool walkAnimation;
    private bool chaseAnimation;

    private int moveDirection = 1;

    private float flipDetectionFalloff;
    private float randomFlipCountdown;

    [HideInInspector] public bool isChasing;
    [HideInInspector] public bool playerSuccessHit;

    [HideInInspector] public bool walkDisabledOverride = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attack = GetComponent<EnemyAttackBehavior>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyStats>();

        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        GetRandomDirection();

        ApplyFacingVisual();
    }

    private void GetRandomDirection()
    {
        facingRight = ((int)Random.Range(0, 2) == 1);
        moveDirection = facingRight ? 1 : -1;
    }

    private void FixedUpdate()
    {
        if (stats.isDead || gameManager.stopAllMovementsOverride)
        {
            return;
        }

        if (walkDisabledOverride)
        {
            return;
        }

        if (stats.isGettingAttacked || playerSuccessHit)
        {
            return;
        }

        HandleFlipDetectionFalloff();
        GetPlayerReferenceForChasing();
        GetMovementBehavior();
    }

    private void Update()
    {
        SetAnimations();
        HandleRandomFlip();
    }

    #region Movement Behaviors
    private void GetMovementBehavior()
    {
        if (isChasing)
        {
            ChasingBehavior();
        }
        else
        {
            PatrolBehavior();
        }

        SetFacing(moveDirection);
    }

    private void ChasingBehavior()
    {
        float trueSpeed =
            (chaseFaster ? chaseSpeed : patrolSpeed) *
            stats.moveSpeedEffectsMultiplier;

        float dirX = currentTarget.position.x - transform.position.x;

        int newDirection = dirX >= 0f ? 1 : -1;

        moveDirection = newDirection;

        HandleMainMove(newDirection, trueSpeed);
    }

    private void PatrolBehavior()
    {
        CheckForObjectsInFront();
        CheckForGroundEdge();

        float trueSpeed =
            patrolSpeed *
            stats.moveSpeedEffectsMultiplier;

        HandleMainMove(moveDirection, trueSpeed);
    }

    private void HandleMainMove(int targetDirection, float speed)
    {
        if (attack.hasRangedTarget)
        {
            return;
        }

        rb.linearVelocity = new Vector2(targetDirection * speed, rb.linearVelocity.y);
    }
    #endregion

    #region Detections
    private void CheckForObjectsInFront()
    {
        bool wallCheck = Physics2D.OverlapCircle(wallCheckOffset.position, checkWallRadius, objectsLayer);

        if (wallCheck)
        {
            TryFlip();
        }
    }

    private void CheckForGroundEdge()
    {
        bool groundCheck = Physics2D.OverlapCircle(groundCheckOffset.position, checkGroundRadius, groundLayer);

        if (!groundCheck)
        {
            TryFlip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!flipOnBumpWithOthers)
        {
            return;
        }

        if (LayersDetected(collision.gameObject.layer, entityLayer))
        {
            if (isChasing)
            {
                return;
            }

            TryFlip();
        }
    }

    private bool LayersDetected(int layer, LayerMask mask)
    {
        return mask == (mask | (1 << layer));
    }

    private void GetPlayerReferenceForChasing()
    {
        currentTarget = chasePlayer ? DetectPlayer() : null;

        isChasing = (currentTarget != null);
    }

    private Transform DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        return hit != null ? hit.transform : null;
    }
    #endregion

    #region Direction Flipping
    private void HandleFlipDetectionFalloff()
    {
        if (flipDetectionFalloff > 0f)
        {
            flipDetectionFalloff -= Time.fixedDeltaTime;
        }
    }

    private void TryFlip()
    {
        if (flipDetectionFalloff > 0f)
        {
            return;
        }

        moveDirection *= -1;
        flipDetectionFalloff = flipCooldown;

        SetFacing(moveDirection);
    }

    private void SetFacing(int direction)
    {
        if (direction == 0)
        {
            return;
        }

        bool shouldFaceRight = direction > 0;

        if (facingRight == shouldFaceRight)
        {
            return;
        }

        facingRight = shouldFaceRight;

        ApplyFacingVisual();
    }

    private void ApplyFacingVisual()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1f : -1f);

        transform.localScale = scale;
    }

    public void FaceDirection(int direction)
    {
        if (direction == 0)
        {
            return;
        }

        moveDirection = direction;
        SetFacing(direction);
    }
    #endregion

    #region Random Flipping
    private void HandleRandomFlip()
    {
        if (!randomlyFlips)
        {
            return;
        }

        randomFlipCountdown -= Time.deltaTime;
        randomFlipCountdown = Mathf.Clamp(randomFlipCountdown, 0, Mathf.Infinity);

        InitiateRandomFlip();
    }

    private void InitiateRandomFlip()
    {
        if (randomFlipCountdown <= 0)
        {
            randomFlipCountdown = Random.Range(randomFlipIntervals[0], randomFlipIntervals[1]);

            bool decision = ((int)Random.Range(0, 2) == 1);

            if (decision)
            {
                TryFlip();
            }
        }
    }
    #endregion

    private void SetAnimations()
    {
        walkAnimation =
            !(stats.isDead || attack.hasRangedTarget || stats.isGettingAttacked) &&
            !walkDisabledOverride &&
            !gameManager.stopAllMovementsOverride;

        chaseAnimation =
            isChasing &&
            !attack.hasRangedTarget &&
            !walkDisabledOverride &&
            gameManager.stopAllMovementsOverride;

        animator.SetBool("Walk", walkAnimation);
        animator.SetBool("Chase", chaseAnimation);
    }

    private void OnDrawGizmosSelected()
    {
        if (showDetections)
        {
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(groundCheckOffset.position, checkGroundRadius);
            Gizmos.DrawWireSphere(wallCheckOffset.position, checkGroundRadius);
        }
    }
}