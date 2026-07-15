using UnityEngine;

public class ObjectInteractableTrigger : MonoBehaviour
{
    [Header("Attachment")]
    [SerializeField] public StoreInteractions store;
    [SerializeField] public JeepCallInteractions jeepCall;

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

    public bool CheckJeepCall(int currentCoins)
    {
        return currentCoins >= jeepCall.cost;
    }

    public void CallJeep()
    {
        jeepCall.CallJeep();
    }
}