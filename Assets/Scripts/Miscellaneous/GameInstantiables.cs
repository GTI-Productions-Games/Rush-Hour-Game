using UnityEngine;

public class GameInstantiables : MonoBehaviour
{

    #region Damage Indicators
    [Header("Damage Indicators")]
    [SerializeField] public GameObject normalDamageIndicator;
    [SerializeField] public GameObject enemyDamageIndicator;
    [SerializeField] public GameObject dotDamageIndicator;

    [Header("Hit Effects")]
    [SerializeField] public GameObject playerHitEffect;
    [SerializeField] public GameObject enemyHitEffect;
    #endregion

    public static GameInstantiables Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}