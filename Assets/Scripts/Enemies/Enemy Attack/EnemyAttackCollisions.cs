using UnityEngine;

public class EnemyAttackCollisions : MonoBehaviour
{
    [Header("Self Attachments")]
    [SerializeField] private Transform hitOrigin;

    private AttackStats stats;

    private void Awake()
    {
        stats = GetComponent<AttackStats>();

        if (hitOrigin == null)
        {
            hitOrigin = transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCollisions player = collision.GetComponent<PlayerCollisions>();

        if (player != null)
        {
            HitEnemy(player);
        }
    }

    private void HitEnemy(PlayerCollisions player)
    {
        float damage = stats.baseDamage * Random.Range(stats.baseDamageScale[0], stats.baseDamageScale[1]);
        float knockbackUpwardForce = stats.knockbackPower * stats.knockbackUpwardMultiplier;

        try
        {
            if (stats.hasDamage)
            {
                player.ReceiveDamage(Mathf.Round(damage), hitOrigin.position, stats.knockbackPower, knockbackUpwardForce);
            }            
            
            if (stats.hasDot)
            {
                float dotDamage = damage * stats.dotScaleFactor;
                int dotLength = stats.baseDotLength;

                player.ReceiveDamageOvertime(Mathf.Round(dotDamage), dotLength);
            }

            if (stats.hasHealth)
            {
                stats.ManageHealth(-1);
            }
        }
        catch { }
    }
}