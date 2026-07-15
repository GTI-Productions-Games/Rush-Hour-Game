using System.Collections;
using UnityEngine;

public class EnemyMiniBossBehavior : MonoBehaviour
{
    [Header("Config - Decision Intervals")]
    [SerializeField] private float[] firstAttackDecisionIntervals = { 1, 4 };
    [SerializeField] private float[] secondAttackDecisionIntervals = { 2, 5 };

    [Header("Config - Chances")]
    [SerializeField] private float firstAttackChance = 80;
    [SerializeField] private float secondAttackChance = 60;

    [Header("Config - Attack Duration")]
    [SerializeField] private float firstAttackDuration;
    [SerializeField] private float secondAttackDuration;

    [Header("Special Behavior")]
    [SerializeField] private EnemyJeepSpecialBehavior jeepSpecialBehavior;

    private Animator animator;
    private EnemyStats stats;
    private EnemyMoveBehavior move;
    private EnemyAttackBehavior attack;

    private Transform target;

    private bool playerInOuterRange = false;
    private bool playerInInnerRange = false;

    private bool isDecidingForFirstAttack;
    private bool isDecidingForSecondAttack;

    private bool isAttacking = false;
    private bool isTamedCheck = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attack = GetComponent<EnemyAttackBehavior>();
        move = GetComponent<EnemyMoveBehavior>();    
        stats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        if (stats.isDead)
        {
            CheckForTamed();
            return;
        }

        GetPlayerTarget();

        if (isAttacking)
        {
            return;
        }

        DetectForPlayer();
        GetStates();
    }

    private void GetPlayerTarget()
    {
        target = attack.DetectPlayer();
    }

    private void DetectForPlayer()
    {               
        Debug.Log(target);

        playerInInnerRange = attack.DetectPlayerSecondary();
        playerInOuterRange = (target != null);
    }

    private void GetStates()
    {
        if (!isDecidingForFirstAttack && playerInInnerRange)
        {
            StartCoroutine(DecideForFirstAttack());
        }

        Debug.Log("Getting state 3");
        if (!isDecidingForSecondAttack && (playerInOuterRange && !playerInInnerRange))
        {
            StartCoroutine(DecideForSecondAttack());
        }
    }

    private IEnumerator DecideForFirstAttack()
    {
        Debug.Log("Decide for first initiated.");
        isDecidingForFirstAttack = true;

        bool decision = (Random.Range(0f, 100f) <= firstAttackChance);

        if (decision)
        {
            isAttacking = true;
            StartCoroutine(InitiateAttackSequence("Attack1", firstAttackDuration));
            yield return null;
        }
        else
        {
            Debug.Log("No decide for first.");
            yield return new WaitForSeconds(Random.Range(firstAttackDecisionIntervals[0], firstAttackDecisionIntervals[1]));
        }

        isDecidingForFirstAttack = false;
    }

    private IEnumerator DecideForSecondAttack()
    {
        Debug.Log("Decide for second initiated.");
        isDecidingForSecondAttack = true;        

        bool decision = (Random.Range(0f, 100f) <= secondAttackChance);

        if (decision)
        {
            isAttacking = true;
            StartCoroutine(InitiateAttackSequence("Attack2", secondAttackDuration));
            yield return null;            
        }
        else
        {
            Debug.Log("No decide for second.");
            yield return new WaitForSeconds(Random.Range(secondAttackDecisionIntervals[0], secondAttackDecisionIntervals[1]));
        }

        isDecidingForSecondAttack = false;
    }

    private IEnumerator InitiateAttackSequence(string attackName, float duration)
    {
        int dir = target.position.x >= transform.position.x ? 1 : -1;
        move.FaceDirection(dir);

        move.walkDisabledOverride = true;
        animator.SetBool(attackName, true);

        yield return new WaitForSeconds(duration);

        animator.SetBool(attackName, false);
        isAttacking = false;
        move.walkDisabledOverride = false;
    }

    private void CheckForTamed()
    {
        if (isTamedCheck)
        {
            return;
        }

        isTamedCheck = true;

        if (jeepSpecialBehavior != null)
        {
            if (!jeepSpecialBehavior.tamable)
            {
                return;
            }

            jeepSpecialBehavior.SummonTamedJeep();
        }
    }    
}