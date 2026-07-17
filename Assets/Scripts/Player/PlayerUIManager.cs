using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public float collectAnimationDelay = 1;

    [Header("Main Attachments")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;

    [Header("Items Attachments")]
    [SerializeField] private TextMeshProUGUI coinsCount;
    [SerializeField] private TextMeshProUGUI sodaPopCount;
    [SerializeField] private TextMeshProUGUI sodaLiciousCount;

    [Header("Other UI Display Attachments")]
    [SerializeField] private Slider drinkProcessBar;

    [Header("Item Collect Attachments")]
    [SerializeField] private Transform itemCollectParent;
    [SerializeField] public GameObject collectCoinPrefab;
    [SerializeField] public GameObject collectSodaPopPrefab;
    [SerializeField] public GameObject collectSodaLiciousPrefab;

    private PlayerStats stats;
    private PlayerItemStats itemStats;
    private PlayerAttackController attack;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        itemStats = GetComponent<PlayerItemStats>();
        attack = GetComponent<PlayerAttackController>();
    }

    private void Start()
    {
        SyncItems();
    }

    private void Update()
    {
        SyncHealthBar();
        SyncDrinkProcess();
    }

    private void SyncHealthBar()
    {
        healthBar.value = stats.playerHealth / stats.playerMaxHealth;
        staminaBar.value = stats.playerStamina / stats.playerMaxStamina;
    }

    private void SyncDrinkProcess()
    {
        drinkProcessBar.gameObject.SetActive(attack.isHealing);
        drinkProcessBar.value = attack.healConsumeFalloff / attack.healConsumeTargetDuration;
    }

    public void SyncItems()
    {
        coinsCount.text = itemStats.coin.ToString();
        sodaPopCount.text = itemStats.sodaPop.ToString();
        sodaLiciousCount.text = itemStats.sodaLicious.ToString();
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
