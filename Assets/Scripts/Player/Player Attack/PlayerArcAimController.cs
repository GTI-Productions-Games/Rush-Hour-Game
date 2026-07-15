using System.Collections;
using UnityEngine;

public class PlayerArcAimController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject arcProjectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform aimPivot;

    [Header("Aiming Range")]
    [SerializeField] private float minAngle = 30f;
    [SerializeField] private float maxAngle = 70f;
    [SerializeField] private float aimSpeed = 90f;

    [Header("Throw Settings")]
    [SerializeField] private float launchSpeed = 15f;

    private PlayerMoveController moveController;

    private float currentAngle;
    private int sweepDirection = 1;
    private bool isAiming = false;
    private Coroutine aimingRoutine;

    private void Awake()
    {
        moveController = GetComponent<PlayerMoveController>();
    }

    public void StartAiming()
    {
        if (isAiming)
        {
            return;
        }

        isAiming = true;
        currentAngle = minAngle;
        sweepDirection = 1;

        if (aimPivot != null)
        {
            aimPivot.gameObject.SetActive(true);
        }

        aimingRoutine = StartCoroutine(AimingLoop());
    }

    private IEnumerator AimingLoop()
    {
        while (isAiming)
        {
            currentAngle += sweepDirection * aimSpeed * Time.deltaTime;

            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                sweepDirection = -1;
            }
            else if (currentAngle <= minAngle)
            {
                currentAngle = minAngle;
                sweepDirection = 1;
            }

            UpdateAimVisual(currentAngle);

            yield return null;
        }
    }

    private void UpdateAimVisual(float angle)
    {
        if (aimPivot == null)
        {
            return;
        }

        aimPivot.position = firePoint.position;

        bool facingRight = moveController == null || moveController.IsFacingRight;
        float displayAngle = facingRight ? angle : 180f - angle;

        aimPivot.rotation = Quaternion.Euler(0f, 0f, displayAngle);
    }

    public void FireProjectile()
    {
        if (!isAiming)
        {
            return;
        }

        isAiming = false;

        if (aimingRoutine != null)
        {
            StopCoroutine(aimingRoutine);
            aimingRoutine = null;
        }

        if (aimPivot != null)
        {
            aimPivot.gameObject.SetActive(false);
        }

        bool facingRight = moveController == null || moveController.IsFacingRight;

        Vector2 launchVelocity = CalculateVelocityFromAngle(currentAngle, launchSpeed, facingRight);

        GameObject projectile = Instantiate(arcProjectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        projectileRb.AddForce(launchVelocity * projectileRb.mass, ForceMode2D.Impulse);
    }

    private Vector2 CalculateVelocityFromAngle(float angleDegrees, float speed, bool facingRight)
    {
        float angleRad = angleDegrees * Mathf.Deg2Rad;
        float horizontalSign = facingRight ? 1f : -1f;

        float vx = Mathf.Cos(angleRad) * speed * horizontalSign;
        float vy = Mathf.Sin(angleRad) * speed;

        return new Vector2(vx, vy);
    }
}