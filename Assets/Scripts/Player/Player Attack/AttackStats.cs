using UnityEngine;

public class AttackStats : MonoBehaviour
{
    [Header("Health Attributes")]
    public bool hasHealth = false;
    public int attackHealth = 1;

    [Header("Damage Attributes")]
    public float baseDamage = 50;
    public float[] baseDamageScale = { 0.8f, 1.2f };
    public float dotScaleFactor = 0.4f;
    public int baseDotLength = 3;

    [Header("Effects Attributes")]
    public float knockbackPower = 300;
    public float knockbackUpwardMultiplier = 0.6f;

    [Header("Config")]
    public bool hasDamage = true;
    public bool hasDot = false;

    [Header("Self Attachments")]
    [SerializeField] private GameObject mainObject;

    [Header("External Attachments")]
    [SerializeField] private GameObject postDestructionEffect;

    public void ManageHealth(int amountToChange)
    {
        attackHealth += amountToChange;
        CheckForDestroyed();
    }

    private void CheckForDestroyed()
    {
        if (attackHealth > 0)
        {
            return;
        }

        mainObject = (mainObject == null) ? gameObject : mainObject;

        Destroy(mainObject);

        if (postDestructionEffect != null)
        {
            Instantiate(postDestructionEffect, transform.position, Quaternion.identity);
        }
    }
}