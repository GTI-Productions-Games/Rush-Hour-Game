using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Attributes")]
    public float health = 200;
    public float maxHealth = 200;
    public bool isDead = false;
    public float deathDestroyDelay = 2;

    [Header("Modifier Attributes")]
    public float moveSpeedEffectsMultiplier = 1;

    [Header("States")]
    public bool isGettingAttacked = false;

    private Animator animator;
    private LootDropManager loot;

    private bool deathSequence = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        loot = GetComponent<LootDropManager>();
    }

    private void Start()
    {
        SetInitialAttributes();
    }

    private void SetInitialAttributes()
    {
        maxHealth = health;
    }

    #region Health Managers
    public void ModifyHealth(float amountToAdd)
    {
        health += amountToAdd;
        health = Mathf.Clamp(health, 0, maxHealth);

        CheckForDeath();
    }

    private void CheckForDeath()
    {
        isDead = (health <= 0);
        
        if (isDead)
        {
            if (deathSequence)
            {
                return;
            }

            deathSequence = true;
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        animator.SetBool("Death", true);        

        yield return new WaitForSeconds(deathDestroyDelay);

        if (loot != null)
        {
            yield return StartCoroutine(loot.DropCoinsSequence());
        }

        Destroy(gameObject);
    }

    #endregion
}