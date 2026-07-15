using UnityEngine;

public class PlayerStats : MonoBehaviour
{  
    public float playerHealth = 500;
    public bool isPlayerDead = false;
    public bool isInJeep = false;

    [HideInInspector] public float playerMaxHealth;

    private Animator animator;

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
    }
    #endregion
}