using System.Collections;
using UnityEngine;

public class LootDropManager : MonoBehaviour
{
    [Header("Config - Coins")]
    [SerializeField] private int coindDropBase = 1; 
    [SerializeField] private int[] coinDropModifier = { -1, 3 };
    [SerializeField] private Transform[] dropRanges = new Transform[2];

    [Header("Attachments")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Dev Options")]
    [SerializeField] private bool seeDropRange;

    public IEnumerator DropCoinsSequence()
    {

        int dropCount = coindDropBase + Random.Range(coinDropModifier[0], coinDropModifier[1]);

        while (dropCount > 0)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(dropRanges[0].position.x, dropRanges[1].position.x), dropRanges[0].position.y);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

            dropCount--;

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

    private void OnDrawGizmos()
    {
        if (seeDropRange)
        {
            Gizmos.DrawWireSphere(dropRanges[0].position, 1);
            
            Gizmos.DrawWireSphere(dropRanges[1].position, 1);
        }
    }
}