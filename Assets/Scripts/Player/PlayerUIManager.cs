using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public float collectAnimationDelay = 1;

    [Header("Main Attachments")]
    [SerializeField] private Slider healthBar;

    [Header("Item Collect Attachments")]
    [SerializeField] private Transform itemCollectParent;
    [SerializeField] public GameObject collectCoinPrefab;
    [SerializeField] public GameObject collectSodaPopPrefab;
    [SerializeField] public GameObject collectSodaLiciousPrefab;

    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        SyncHealthBar();
    }

    private void SyncHealthBar()
    {
        healthBar.value = stats.playerHealth / stats.playerMaxHealth;
    }

    public void ShowItemCollect(Items item)
    {
        switch (item)
        {
            case Items.Coins:
                Instantiate(collectCoinPrefab, itemCollectParent);
                break;

            case Items.SodaLicious:
                Instantiate(collectSodaLiciousPrefab, itemCollectParent);
                break;

            case Items.SodaPop:
                Instantiate(collectSodaPopPrefab, itemCollectParent);
                break;
        }        
    }
}
