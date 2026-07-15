using UnityEngine;

public class PlayerAttackCollisions : MonoBehaviour
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
        EnemyCollisions enemy = collision.GetComponent<EnemyCollisions>();

        if (enemy != null)
        {
            HitEnemy(enemy);
        }
    }

    private void HitEnemy(EnemyCollisions enemy)
    {
        float damage = stats.baseDamage * Random.Range(stats.baseDamageScale[0], stats.baseDamageScale[1]);
        float knockbackVertical = stats.knockbackPower * stats.knockbackUpwardMultiplier;
        try
        {            
            bool hit = enemy.ReceiveDamage(Mathf.Round(damage), hitOrigin.position, stats.knockbackPower, knockbackVertical);

            if (stats.hasDot)
            {
                float dotDamage = damage * stats.dotScaleFactor;
                int dotLength = stats.baseDotLength;

                enemy.ReceiveDamageOvertime(Mathf.Round(dotDamage), dotLength);
            }            

            if (stats.hasHealth)
            {
                stats.ManageHealth(-1);
            }
        }
        catch 
        {
        }
    }
}