using UnityEngine;

public class BottleMechanic : MonoBehaviour
{
    [Header("Post-Break Attachments")]
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private GameObject postBreakInstantiate;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            BreakGlass();
        }
    }

    private void BreakGlass()
    {
        Destroy(gameObject);

        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }        

        if (postBreakInstantiate != null)
        {
            Instantiate(postBreakInstantiate, transform.position, Quaternion.identity);
        }
    }
}
