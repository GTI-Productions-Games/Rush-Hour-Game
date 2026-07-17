using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    private SpriteRenderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (var sr in renderers)
            sr.material.SetFloat("_FlashAmount", 1);

        yield return new WaitForSeconds(0.1f);

        foreach (var sr in renderers)
            sr.material.SetFloat("_FlashAmount", 0);
    }
}