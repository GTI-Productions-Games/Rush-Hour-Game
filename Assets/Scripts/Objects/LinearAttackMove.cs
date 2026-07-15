using UnityEngine;

public class LinearAttackMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
   
    public void Initialize(float direction)
    {
        rb.linearVelocity = transform.right * direction * moveSpeed;
    }
}
