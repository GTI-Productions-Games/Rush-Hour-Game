using System.Collections;
using TMPro;
using UnityEngine;

public class JeepCallInteractions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] public int cost;
    [SerializeField] private float jeepCallCooldown = 10;

    [Header("Attachments")]
    [SerializeField] private GameObject jeepPrefab;
    [SerializeField] private Transform jeepSpawnpoint;
    [SerializeField] private TextMeshPro jeepCooldownmUI;

    private bool callAvailable = false;

    public void CallJeep()
    {
        if (!callAvailable)
        {
            PopUpManager.Instance.ShowNormalCustom("Cannot complete action. Jeep call in cooldown.");
            return;            
        }

        StartCoroutine(HandleJeepCallTimeout());
        Instantiate(jeepPrefab, jeepSpawnpoint.position, Quaternion.identity);
    }

    private IEnumerator HandleJeepCallTimeout()
    {
        callAvailable = false;

        float seconds = jeepCallCooldown;

        jeepCooldownmUI.gameObject.SetActive(true);
        jeepCooldownmUI.text = $"Available again in: {seconds}";        

        while (seconds > 0)
        {
            jeepCooldownmUI.text = $"Available again in: {seconds}";

            yield return new WaitForSeconds(1);

            seconds--;
        }

        yield return null;

        jeepCooldownmUI.gameObject.SetActive(false);
        callAvailable = true;
    }
}