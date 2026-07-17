using System.Collections;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float flashEffectDuration = 0.2f;
    [SerializeField] private float gettingAttackedDuration = 1f;

    [Header("Attachments")]
    [SerializeField] private DamageFlash damageFlash;

    private Animator animator;
    private EnemyStats stats;
    private UIIndicators ui;

    private Rigidbody2D rb;

    private Coroutine dot;

    private EnemyAudioManager enemyAudio;
    private RobotAudioManager robotAudio;

    private float gettingAttackedFalloff;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        ui = GetComponent<UIIndicators>();

        robotAudio = GetComponent<RobotAudioManager>();
    }

    private void Update()
    {
        HandleGettingAttackedFalloff();
    }

    #region Damage Doors
    public bool ReceiveDamage(float damageAmount, Vector2 sourcePosition, float knockbackForce = 0, float knockbackVertical = 0)
    {
        bool hit = !stats.isDead;        

        if (!hit)
        {
            return false;
        }

        gettingAttackedFalloff = gettingAttackedDuration;

        stats.ModifyHealth(-damageAmount);

        ui.ShowDamageNumberIndicator(damageAmount, GameInstantiables.Instance.normalDamageIndicator);
        ui.ShowHitEffect(GameInstantiables.Instance.playerHitEffect, sourcePosition);
        animator.SetTrigger("Damage");
        
        HandleKnockback(sourcePosition, knockbackForce, knockbackVertical);

        if (robotAudio != null)
        {
            robotAudio.PlayHit();
        }

        return hit;
    }

    public void ReceiveDamageOvertime(float dotDamage, int dotLength)
    {
        animator.SetTrigger("Damage");

        if (dot != null)
        {
            StopCoroutine(dot);
            dot = null;
        }

        dot = StartCoroutine(HandleDamageOvertime(dotDamage, dotLength));
    }

    private IEnumerator HandleDamageOvertime(float dotDamage, int dotLength)
    {
        do
        {
            Debug.Log("Dot: " + dotLength);
            stats.ModifyHealth(-dotDamage);
            ui.ShowDamageNumberIndicator(dotDamage, GameInstantiables.Instance.dotDamageIndicator);

            dotLength--;

            yield return new WaitForSeconds(1);
        }
        while (dotLength > 0);

        yield return null;

        dot = null;
    }

    private void HandleKnockback(Vector2 sourcePosition, float knockbackForce, float knockbackUpwardForce)
    {
        float dirX = Mathf.Sign(transform.position.x - sourcePosition.x);

        if (dirX == 0)
        {
            dirX = 1;
        }        

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(dirX * knockbackForce, knockbackUpwardForce), ForceMode2D.Impulse);
    }

    private void HandleGettingAttackedFalloff()
    {
        gettingAttackedFalloff -= Time.deltaTime;
        gettingAttackedFalloff = Mathf.Clamp(gettingAttackedFalloff, 0, gettingAttackedDuration);

        stats.isGettingAttacked = (gettingAttackedFalloff > 0);
    }

    private IEnumerator FlashEffect(float duration)
    {
        // Flash white

        yield return new WaitForSeconds(duration);

        // Flash back
    }
    #endregion
}