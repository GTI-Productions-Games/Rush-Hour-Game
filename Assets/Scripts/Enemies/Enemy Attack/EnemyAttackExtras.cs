using UnityEngine;

public class EnemyAttackExtras : MonoBehaviour
{
    [Header("Shockwave Config")]
    [SerializeField] private Transform shockwaveOriginPoint;
    [SerializeField] private GameObject shockwavePrefab;

    [Header("Blast Config")]
    [SerializeField] private Transform blastOriginPoint;
    [SerializeField] private GameObject blastPrefab;

    public void ShockwaveAttack()
    {
        Instantiate(shockwavePrefab, shockwaveOriginPoint.position, Quaternion.identity);
    }

    public void BlastShootAttack()
    {
        LinearAttackMove blast =
            Instantiate(blastPrefab, blastOriginPoint.position, blastOriginPoint.rotation).
            GetComponent<LinearAttackMove>();

        blast.Initialize(Mathf.Sign(transform.localScale.x));
    }
}
