using System.Collections;
using UnityEngine;

public class HighwayInteractions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float transportationDelay = 4;    
    [SerializeField] private Transform endDestination;

    public bool EnterHighway(bool inJeep, Transform player)
    {
        if (!inJeep)
        {
            string[] cannotCrossLines = { 
                "I need somethign to ride to cross faster...", 
                "...or I can just walk."
            };
            PopUpManager.Instance.StartMonologue(cannotCrossLines);

            return false;
        }

        StartCoroutine(TransportingSequence(player));

        return true;
    }

    private IEnumerator TransportingSequence(Transform player)
    {
        PopUpManager.Instance.ShowLoadingCoverFull(true, "Transporting...");

        yield return new WaitForSeconds(transportationDelay * .5f);

        player.position = endDestination.position;

        yield return new WaitForSeconds(transportationDelay * 0.5f);        

        PopUpManager.Instance.ShowLoadingCoverFull(false);
    }
}