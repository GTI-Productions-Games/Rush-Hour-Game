using UnityEngine;

public class PlayerCameraMove : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Config")]
    [SerializeField] private float damping;
    [SerializeField] private Vector3 offset;

    private Vector3 targetPos;

    private void FixedUpdate()
    {
        targetPos = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, damping * Time.fixedDeltaTime);
    }
}
