using UnityEngine;

public class RandomMove : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed;
    private Vector3 random;

    private void Start()
    {
        random.x = Random.Range(-1, 1);
        random.y = Random.Range(-1, 1);
        random.z = 0;
    }

    private void Update()
    {
        transform.position += random * moveSpeed * Time.deltaTime;
    }
}