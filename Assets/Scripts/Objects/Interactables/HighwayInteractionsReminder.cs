using UnityEngine;

public class HighwayInteractionsReminder :MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Transform backSide;
    [SerializeField] private float backsideDetectRadius;
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Physics2D.OverlapCircle(backSide.position, backsideDetectRadius, playerLayer))
        {
            string[] message =
            {
                "Should I really walk rather than ride jeepney or something?"
            };

            PopUpManager.Instance.StartMonologue(message);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(backSide.position, backsideDetectRadius);
    }
}