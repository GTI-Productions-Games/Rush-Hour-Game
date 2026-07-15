using System.Collections;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float hitAnimationLength;

    [Header("Player Attachments")]
    [SerializeField] private Animator animator;

    [HideInInspector] public bool isHit;

    private PlayerStats stats;
    private PlayerItemStats itemStats;
    private PlayerUIManager playerUI;
    private UIIndicators ui;
    private Rigidbody2D rb;

    private Coroutine dot;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        itemStats = GetComponent<PlayerItemStats>();
        rb = GetComponent<Rigidbody2D>();
        ui = GetComponent<UIIndicators>();
        playerUI = GetComponent<PlayerUIManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InstantDeath"))
        {
            InstantDeath();
        }
    }

    #region Attack Receive
    private void InstantDeath()
    {
        animator = GetComponent<Animator>();
        stats.playerHealth = 0;
        stats.isPlayerDead = true;
        animator.SetBool("Death", true);
    }

    public void ReceiveDamage(float damage, Vector2 sourcePosition, float knockbackForce = 0, float knockbackUpwardForce = 0)
    {
        Debug.Log("Received");
        if (stats.isPlayerDead || isHit)
        {
            return;
        }        

        stats.ModifyHealth(-damage);
        
        ui.ShowDamageNumberIndicator(damage, GameInstantiables.Instance.enemyDamageIndicator);
        ui.ShowHitEffect(GameInstantiables.Instance.enemyHitEffect, sourcePosition);

        HandleKnockback(sourcePosition, knockbackForce, knockbackUpwardForce);
        StartCoroutine(HitSequence());
    }

    public void ReceiveDamageOvertime(float dotDamage, int dotLength)
    {
        Debug.Log("Dot Started 1");
        if (dot != null)
        {
            StopCoroutine(dot);
            dot = null;
        }

        Debug.Log("Dot Started 2");
        dot = StartCoroutine(HandleDamageOvertime(dotDamage, dotLength));
    }

    private IEnumerator HandleDamageOvertime(float dotDamage, int dotLength)
    {
        Debug.Log("Dot Started 3");
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

        Debug.Log("Dot FInished");
        dot = null;
    }

    private IEnumerator HitSequence()
    {
        isHit = true;
        
        animator.SetTrigger("Hit");

        StartCoroutine(FlashEffect(hitAnimationLength / 2));

        yield return new WaitForSeconds(hitAnimationLength / 2);

        isHit = false;
    }

    private IEnumerator FlashEffect(float duration)
    {
        // Flash white

        yield return new WaitForSeconds(duration);

        // Flash back
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
    #endregion

    #region
    public void ReceiveItem(Items item, int amountToAdd)
    {
        itemStats.ReceiveItem(item, amountToAdd, true, true);
    }
    #endregion
}