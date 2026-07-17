using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playerHealth = 500;
    public float playerStamina = 100;
    public bool isPlayerDead = false;
    public bool acquiredJeep = false;

    [HideInInspector] public float playerMaxHealth;
    [HideInInspector] public float playerMaxStamina;

    private Animator animator;

    private bool deathTriggered;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        GetInitialStats();
    }

    private void GetInitialStats()
    {
        playerMaxHealth = playerHealth;
        playerMaxStamina = playerStamina;
    }

    #region Health Managers
    public void ModifyHealth(float amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);

        CheckForDeath();
    }

    private void CheckForDeath()
    {
        isPlayerDead = (playerHealth <= 0);
        animator.SetBool("Death", isPlayerDead);

        if (isPlayerDead)
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        if (deathTriggered)
        {
            return;
        }

        deathTriggered = true;
        StartCoroutine(GameEndManager.Instance.TriggerLose(1));
    }
    #endregion
}