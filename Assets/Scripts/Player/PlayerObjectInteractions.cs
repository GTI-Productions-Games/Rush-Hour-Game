using System.Collections;
using UnityEngine;

public class PlayerObjectInteractions : MonoBehaviour
{
    [SerializeField] private PlayerAudioManager playerAudio;

    private ControllerInput input;
    private ObjectInteractableTrigger interactable;
    private PlayerItemStats itemStats;
    private PlayerStats stats;

    private void Awake()
    {
        InitializeInteractionsInput();

        itemStats = GetComponent<PlayerItemStats>();
        stats = GetComponent<PlayerStats>();
    }

    private void InitializeInteractionsInput()
    {
        input = new ControllerInput();
        input.Enable();

        input.Actions.Interact.performed += ctx => InteractWithObject();
        input.Monologue.Proceed.performed += ctx => PopUpManager.Instance.ProceedMonologue();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    #region Object Interactions
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

            itemStats.ReceiveItem(Items.Coins, -bought.cost, false, true);
            itemStats.ReceiveItem(bought.itemBought, 1, true, true);

            return;
        }
        
        if (interactable.jeepCall != null)
        {
            if (!interactable.CheckJeepCall(itemStats.coin))
            {
                PopUpManager.Instance.ShowInsufficientMoney();
                return;
            }

            itemStats.ReceiveItem(Items.Coins, -interactable.jeepCall.cost, false, true);
            interactable.CallJeep();

            return;
        }

        if (interactable.jeepVehicle != null)
        {
            int rideCost = interactable.jeepVehicle.InteractWithJeep(itemStats.coin);

            if (rideCost != 0)
            {
                itemStats.ReceiveItem(Items.Coins, -rideCost, false, true);
                stats.acquiredJeep = true;
                playerAudio.PlayVehicleStart();
            }
        }

        if (interactable.highway != null)
        {
            bool success = interactable.highway.EnterHighway(stats.acquiredJeep, transform);
            
            if (success)
            {
                stats.acquiredJeep = false;
                playerAudio.PlayVehicleStart();
            }
        }
    }

    #endregion
}