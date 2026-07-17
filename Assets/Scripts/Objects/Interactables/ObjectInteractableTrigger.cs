using UnityEngine;

public class ObjectInteractableTrigger : MonoBehaviour
{
    [Header("Mechanic Attachment")]
    [SerializeField] public StoreInteractions store;
    [SerializeField] public JeepCallInteractions jeepCall;
    [SerializeField] public JeepVehicleInteractions jeepVehicle;
    [SerializeField] public HighwayInteractions highway;

    [Header("Hint Config")]
    [SerializeField] private float hintDetectRadius;
    [SerializeField] private LayerMask playerLayer;

    [Header("UI Attachments")]
    [SerializeField] private GameObject hint;

    [Header("Dev Options")]
    [SerializeField] private bool seeHintDetection;

    #region Store
    public (Items itemBought, int cost) InteractStore()
    {
        return (store.itemToBuy, store.cost);
    }

    public bool CheckStoreCost(int currentCoins)
    {
        return currentCoins >= store.cost;
    }
    #endregion

    #region Jeep Call
    public bool CheckJeepCall(int currentCoins)
    {
        return currentCoins >= jeepCall.cost;
    }

    public void CallJeep()
    {
        jeepCall.CallJeep();
    }
    #endregion

    private void Update()
    {
        bool playerInRange = Physics2D.OverlapCircle(transform.position, hintDetectRadius, playerLayer);

        ToggleInt(playerInRange);
    }

    private void ToggleInt(bool toggle)
    {
        if (hint != null)
        {
            hint.SetActive(toggle);
        }        
    }

    private void OnDrawGizmos()
    {
        if (seeHintDetection)
        {
            Gizmos.DrawWireSphere(transform.position, hintDetectRadius);
        }
    }
}