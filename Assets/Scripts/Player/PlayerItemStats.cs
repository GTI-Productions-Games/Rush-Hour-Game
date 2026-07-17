using System.Collections;
using UnityEngine;

public class PlayerItemStats : MonoBehaviour
{
    public int coin = 0;
    public int sodaLicious = 0;
    public int sodaPop = 0;

    [SerializeField] private PlayerAudioManager playerAudio;

    private PlayerUIManager playerUI;

    private void Awake()
    {
        playerUI = GetComponent<PlayerUIManager>();
    }

    private void AddItem(Items item, int amountToAdd, bool hasSound = false)
    {
        switch (item)
        {           
            case Items.SodaLicious:
                sodaLicious += amountToAdd;
                break;

            case Items.SodaPop:
                sodaPop += amountToAdd;
                break;
        }

        if (hasSound)
        {
            playerAudio.PlayCoin();
        }
    }

    private void AddCoin(int amountToAdd, bool hasSound = false)
    {
        coin += amountToAdd;

        if (hasSound)
        {
            playerAudio.PlayCoin();
        }
    }

    public void ReceiveItem(Items item, int amountToAdd, bool hasSequence = true, bool hasSound = false)
    {
        float delay = hasSequence ? playerUI.collectAnimationDelay : 0;

        if (hasSequence)
        {
            playerUI.ShowItemCollect(item);
        }

        StartCoroutine(ReceiveItemSequence(item, amountToAdd, delay, hasSound));
    }

    private IEnumerator ReceiveItemSequence(Items item, int amountToAdd, float delay, bool hasSound = false)
    {      
        yield return new WaitForSeconds(delay);

        ReceiveItemMain(item, amountToAdd, hasSound);
    }

    private void ReceiveItemMain(Items item, int amountToAdd, bool hasSound = false)
    {
        switch (item)
        {
            case Items.Coins:
                AddCoin(amountToAdd, hasSound);
                break;

            case Items.SodaPop:
                AddItem(item, amountToAdd, hasSound);
                break;

            case Items.SodaLicious:
                AddItem(item, amountToAdd, hasSound);
                break;
        }

        playerUI.SyncItems();
    }
}