using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float attackMeleeLength;
    [SerializeField] private float attackRangedLength;

    [Header("Heal Config")]
    [SerializeField] public float healConsumeTargetDuration = 2.5f;
    [SerializeField] private float healSodaAmount = 100;

    [Header("Player Attachments")]
    [SerializeField] private GameObject mainSprite;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAudioManager playerAudio;

    [Header("Ranged Attachments")]
    [SerializeField] private Transform attackGunTip;
    [SerializeField] private GameObject attackRangedBullet;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isHealing;

    [HideInInspector] public float healConsumeFalloff;

    private ControllerInput input;
    private PlayerStats stats;
    private PlayerCollisions collisions;
    private PlayerItemStats itemStats;
    private PlayerArcAimController arcAimController;
    private GameManager gameManager;    

    private bool throwInitiated;

    private void Awake()
    {        
        stats = GetComponent<PlayerStats>();
        collisions = GetComponent<PlayerCollisions>();
        itemStats = GetComponent<PlayerItemStats>();
        arcAimController = GetComponent<PlayerArcAimController>();
        
        gameManager = FindAnyObjectByType<GameManager>();

        InitializeInput();
    }

    private void InitializeInput()
    {
        input = new ControllerInput();
        input.Enable();

        InitializeAttackActinos();
    }

    private void InitializeAttackActinos()
    {
        input.Actions.Attack1.performed += ctx => PerformAttackMelee();

        input.Actions.Attack2.performed += ctx => StartAttackRanged();
        input.Actions.Attack2.canceled += ctx => ThrowAttackRanged();

        input.Actions.Heal.performed += ctx => PerformHeal();
        input.Actions.Heal.canceled += ctx => CancelHeal();
    }

    private void PerformAttackMelee()
    {
        if (isAttacking || stats.isPlayerDead || collisions.isHit || gameManager.stopAllMovementsOverride)
        {
            Debug.Log($"Can't attac. {isAttacking} | {stats.isPlayerDead} || {collisions.isHit}");
            return;
        }

        StartCoroutine(AttackMeleeSequence());
    }

    private IEnumerator AttackMeleeSequence()
    {
        isAttacking = true;

        animator.SetTrigger("Attack1");

        yield return new WaitForSeconds(attackMeleeLength);

        isAttacking = false;
    }

    private void StartAttackRanged()
    {
        if (isAttacking || stats.isPlayerDead || collisions.isHit || throwInitiated || gameManager.stopAllMovementsOverride)
        {
            return;
        }

        if (itemStats.sodaPop <= 0)
        {
            PopUpManager.Instance.ShowNormalCustom("You don't have any Soda Pop to throw.");
            return;
        }

        throwInitiated = true;

        isAttacking = true;
        animator.SetBool("Attack2", true);

        arcAimController.StartAiming();
    }

    private void ThrowAttackRanged()
    {
        if (!throwInitiated)
        {
            return;
        }

        Debug.Log("Release.");

        throwInitiated = false;
        isAttacking = false;
        animator.SetBool("Attack2", false);

        arcAimController.FireProjectile();

        itemStats.ReceiveItem(Items.SodaPop, -1, false, false);
    }

    private void Update()
    {
        HandleHealingDuration();
    }

    private void HandleHealingDuration()
    {
        if (isHealing)
        {
            healConsumeFalloff += Time.deltaTime;
        }        
        else
        {
            healConsumeFalloff = 0;
        }

        CheckForHealingComplete();
    }

    private void CheckForHealingComplete()
    {
        if (healConsumeFalloff >= healConsumeTargetDuration)
        {
            itemStats.ReceiveItem(Items.SodaLicious, -1, false, false);
            stats.ModifyHealth(healSodaAmount);
            healConsumeFalloff = 0;

            playerAudio.PlayHeal();

            CancelHeal();
        }
    }

    private void PerformHeal()
    {
        if (isAttacking || stats.isPlayerDead || gameManager.stopAllMovementsOverride)
        {
            return;
        }

        if (itemStats.sodaLicious <= 0)
        {
            return;
        }

        isAttacking = true;
        animator.SetBool("Drink", true);
        isHealing = (itemStats.sodaLicious > 0);        
    }

    private void CancelHeal()
    {
        isAttacking = false;
        isHealing = false;

        animator.SetBool("Drink", false);
    }
}
