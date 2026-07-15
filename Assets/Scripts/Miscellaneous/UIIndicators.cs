using TMPro;
using UnityEngine;

public class UIIndicators : MonoBehaviour
{
    
    public void ShowDamageNumberIndicator(float amount, GameObject damageIndicator)
    {
        TextMeshPro indicator = Instantiate(damageIndicator, transform.position, Quaternion.identity).GetComponent<TextMeshPro>();
        indicator.text = amount.ToString();
    }

    public void ShowHitEffect(GameObject hitEffect, Vector2 hitPosition)
    {
        Instantiate(hitEffect, hitPosition, Quaternion.identity);
    }
}