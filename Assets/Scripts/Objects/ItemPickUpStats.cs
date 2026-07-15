using UnityEngine;

public class ItemPickUpStats : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Items itemType;
    [SerializeField] private int amount = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCollisions player = collision.GetComponent<PlayerCollisions>();

        if (player != null)
        {
            ItemCollect(player);
        }
    }

    private void ItemCollect(PlayerCollisions player)
    {
        player.ReceiveItem(itemType, amount);
        Destroy(gameObject);
    }
}
