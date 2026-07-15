using System.Collections;
using UnityEngine;

public class EnemyJeepSpecialBehavior : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float[] chanceRange = { 30, 60 };

    [Header("Attachments")]
    [SerializeField] private GameObject tamedJeepPrefab;
    [SerializeField] private Transform tamedJeepSpawnpoint;

    private EnemyStats stats;

    public bool tamable = false;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        DecideIfTamedAfterDeath();
    }

    private void DecideIfTamedAfterDeath()
    {
        float chance = Random.Range(chanceRange[0], chanceRange[1]);
        float decision = Random.Range(1, 100);

        tamable = decision <= chance;
    }

    public void SummonTamedJeep()
    {
        
    }

    private IEnumerator SummonTamedJeepSequence()
    {
        yield return new WaitForSeconds(stats.deathDestroyDelay * 0.99f);

        Instantiate(tamedJeepPrefab, tamedJeepSpawnpoint.position, Quaternion.identity);    
    }
}