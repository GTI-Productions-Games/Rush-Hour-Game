using System.Collections;
using UnityEngine;

public class EnemyAttackBehavior : MonoBehaviour
{
    public enum AttackType
    {
        Melee,
        RangedLinear,
        RangedProjectile,
        Override
    }

    [Header("Attributes")]
    [SerializeField] private AttackType attackType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float[] attackCooldownMutliplierRange = { 0.7f, 1.3f };    
    [SerializeField] private float instantiateDelay = 0;
    [SerializeField] private LayerMask playerLayer;

    [Header("Detectors Config")]
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private float detectionRediusSecond = 0f;

    [Header("Optional Attributes (Ranged Projectile)")]
    [SerializeField] private float arcApexHeight = 3f;

    [Header("Self Attachments (Ranged)")]
    [SerializeField] private Transform firePoint;

    [Header("External Attachments")]    
    [SerializeField] private GameObject linearProjectilePrefab;
    [SerializeField] private GameObject arcProjectilePrefab;

    [Header("Dev Options")]
    [SerializeField] private bool showDetections;

    private Animator animator;
    private EnemyStats stats;

    private Transform target;

    private float cooldown;

    public bool hasRangedTarget => (target != null && attackType != AttackType.Melee);
    
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    private void Update()
    {
        if (stats.isGettingAttacked || stats.isDead)
        {
            return;
        }

        HandleAttackCooldown();
        GetAttackBehavior();        
    }

    private void HandleAttackCooldown()
    {
        cooldown -= Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, Mathf.Infinity);
    }

    public Transform DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        return hit != null ? hit.transform : null;
    }

    public bool DetectPlayerSecondary()
    {
        return Physics2D.OverlapCircle(transform.position, detectionRediusSecond, playerLayer);        
    }

    private void GetAttackBehavior()
    {
        if (cooldown > 0)
        {
            return;
        }

        switch (attackType)
        {
            case AttackType.Melee:
                target = DetectPlayer();
                AttackMelee();
                break;

            case AttackType.RangedLinear:
                target = DetectPlayer();
                FireLinear();
                break;

            case AttackType.RangedProjectile:
                target = DetectPlayer();
                FireProjectile();
                break;            
        }
    }

    private void AttackMelee()
    {
        if (target == null)
        {
            return;
        }

        animator.SetTrigger("Attack");
        cooldown = GetAttackCooldown();
    }

    private void FireLinear()
    {
        if (target == null)
        {
            return;
        }

        animator.SetTrigger("Attack");
        cooldown = GetAttackCooldown();

        StartCoroutine(FireLinearSequence());        
    }

    private IEnumerator FireLinearSequence()
    {
        yield return new WaitForSeconds(instantiateDelay);

        LinearAttackMove bullet =
            Instantiate(linearProjectilePrefab, firePoint.position, firePoint.rotation).
            GetComponent<LinearAttackMove>();

        bullet.Initialize(Mathf.Sign(transform.localScale.x));         
    }

    private void FireProjectile()
    {
        if (target == null)
        {
            return;
        }

        animator.SetTrigger("Attack");
        cooldown = GetAttackCooldown();

        StartCoroutine(FireProjectileSequence());
    }

    private IEnumerator FireProjectileSequence()
    {
        yield return new WaitForSeconds(instantiateDelay);

        GameObject projectile = Instantiate(arcProjectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        float gravity = Physics2D.gravity.y * projectileRb.gravityScale;

        Vector2 launchVelocity = CalculateArcLaunchVelocity(firePoint.position, target.position, arcApexHeight, gravity);

        projectileRb.AddForce(launchVelocity * projectileRb.mass, ForceMode2D.Impulse);
    }

    private float GetAttackCooldown()
    {
        return attackCooldown * Random.Range(attackCooldownMutliplierRange[0], attackCooldownMutliplierRange[1]);
    }
    #region Arc Trajectory Calculations
    private Vector2 CalculateArcLaunchVelocity(Vector2 start, Vector2 end, float apexHeight, float gravity)
    {
        float gravityMagnitude = Mathf.Abs(gravity);

        if (gravityMagnitude <= 0.0001f)
        {
            gravityMagnitude = 0.0001f;
        }            

        float deltaX = end.x - start.x;
        float deltaY = end.y - start.y;
        
        float safeApexHeight = Mathf.Max(apexHeight, deltaY + 0.1f);

        float timeToApex = Mathf.Sqrt(2f * safeApexHeight / gravityMagnitude);
        float timeFromApexToTarget = Mathf.Sqrt(2f * (safeApexHeight - deltaY) / gravityMagnitude);
        float totalFlightTime = timeToApex + timeFromApexToTarget;

        float velocityY = gravityMagnitude * timeToApex;
        float velocityX = deltaX / totalFlightTime;

        return new Vector2(velocityX, velocityY);
    }
    #endregion


    private void OnDrawGizmosSelected()
    {
        if (showDetections)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = Color.azure;
            Gizmos.DrawWireSphere(transform.position, detectionRediusSecond);
        }        
    }
}