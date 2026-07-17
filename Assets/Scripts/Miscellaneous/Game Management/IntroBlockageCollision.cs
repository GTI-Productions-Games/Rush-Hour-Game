using UnityEngine;

public class IntroBlockageCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PopUpManager.Instance.ShowNormalCustom("Knock all enemies first before continuing.");
        }
    }
}