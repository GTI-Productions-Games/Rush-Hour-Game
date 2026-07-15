using UnityEngine;

public class TemporaryObjects : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] public float destroyAfter;
    [SerializeField] private bool deactivateOnly;

    [Header("Attachments")]
    [SerializeField] private GameObject mainObject;
    [SerializeField] private GameObject postDestructionEffect;

    private void Update()
    {
        ManageTimeout();

        if (deactivateOnly)
        {
            DeactivateAfterTimeout();
        }
        else
        {
            DestroyAfterTimeout();
        }
    }

    private void ManageTimeout()
    {
        destroyAfter -= Time.deltaTime;
        destroyAfter = Mathf.Clamp(destroyAfter, 0, Mathf.Infinity);
    }

    private void DeactivateAfterTimeout()
    {
        if (destroyAfter <= 0f)
        {
            if (mainObject != null)
            {
                mainObject.SetActive(false);
                return;
            }

            gameObject.SetActive(false);
        }
    }

    private void DestroyAfterTimeout()
    {
        if (destroyAfter <= 0f)
        {
            if (mainObject != null)
            {
                Destroy(mainObject);
                return;
            }

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        PostDestructionEffect();
    }

    private void OnDisable()
    {
        if (deactivateOnly)
        {
            PostDestructionEffect();
        }
    }

    private void PostDestructionEffect()
    {
        if (postDestructionEffect != null)
        {
            Instantiate(postDestructionEffect, transform.position, Quaternion.identity);
        }
    }
}