using System.Collections;
using UnityEngine;

public class PlayerObjectInteractions : MonoBehaviour
{
    private ControllerInput input;
    private ObjectInteractableTrigger interactable;
    private PlayerItemStats itemStats;
    private PlayerStats stats;
    private PlayerUIManager playerUI;

    private void Awake()
    {
        InitializeInteractionsInput();

        stats = GetComponent<PlayerStats>();
        itemStats = GetComponent<PlayerItemStats>();
        playerUI = GetComponent<PlayerUIManager>();
    }

    private void InitializeInteractionsInput()
    {
        input = new ControllerInput();
        input.Enable();

        input.Actions.Interact.performed += ctx => InteractWithObject();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ObjectInteractableTrigger>())
        {
            interactable = collision.GetComponent<ObjectInteractableTrigger>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ObjectInteractableTrigger>())
        {
            interactable = null;
        }
    }

    private void InteractWithObject()
    {
        if (interactable == null)
        {
            return;
        }

        if (interactable.store != null)
        {
            if (!interactable.CheckStoreCost(itemStats.coin))
            {
                PopUpManager.Instance.ShowInsufficientMoney();
                return;
            }

            var bought = interactable.InteractStore();

            itemStats.ReceiveItem(Items.Coins, bought.cost, false);
            itemStats.ReceiveItem(bought.itemBought, 1, true, false);

            return;
        }
        
        if (interactable.jeepCall != null)
        {
            if (!interactable.CheckJeepCall(itemStats.coin))
            {
                PopUpManager.Instance.ShowInsufficientMoney();
                return;
            }

            itemStats.ReceiveItem(Items.Coins, interactable.jeepCall.cost, false);
            interactable.CallJeep();

            return;
        }
    }
}