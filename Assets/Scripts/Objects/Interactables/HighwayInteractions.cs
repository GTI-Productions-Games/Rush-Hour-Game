using System.Collections;
using UnityEngine;

public class HighwayInteractions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float transportationDelay = 4;

    public void EnterHighway(bool inJeep)
    {
        if (!inJeep)
        {
            PopUpManager.Instance.ShowNormalCustom("I cannot cross the highway without a vehicle.");
            return;
        }

        StartCoroutine(TransportingSequence());
    }

    private IEnumerator TransportingSequence()
    {
        PopUpManager.Instance.ShowLoadingCover(true);

        yield return new WaitForSeconds(transportationDelay);

        PopUpManager.Instance.ShowLoadingCover(false);
    }
}