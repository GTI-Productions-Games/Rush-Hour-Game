using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class EnemySpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private int[] spawnIntervals = { 2, 5 };    

    [Header("Player Detect Config")]
    [SerializeField] private float playerDetectionRange;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attachments")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Dev Options")]
    [SerializeField] private bool seeDetectionRange;
    
    private List<EnemyStats> activeEnemies = new List<EnemyStats>();

    private float intervalTimeout;

    private void Update()
    {
        CheckForPlayer();
        HandleSpawnInterval();
    }

    private void HandleSpawnInterval()
    {
        intervalTimeout -= Time.deltaTime;
        intervalTimeout = Mathf.Clamp(intervalTimeout, 0, Mathf.Infinity);
    }

    private void CheckForPlayer()
    {
        bool playerInDistance = Physics2D.OverlapCircle(transform.position, playerDetectionRange, playerLayer);

        if (playerInDistance && intervalTimeout <= 0)
        {
            intervalTimeout = Random.Range(spawnIntervals[0], spawnIntervals[1]);
            TrySpawnEnemy();            
        }
    }


    public void TrySpawnEnemy()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (activeEnemies.Count >= maxEnemies)
        {
            return;
        }            

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemyObject = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, Quaternion.identity);

        EnemyStats enemy = enemyObject.GetComponent<EnemyStats>();

        activeEnemies.Add(enemy);
    }

    private void OnDrawGizmos()
    {
        if (seeDetectionRange)
        {
            Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        }        
    }
}