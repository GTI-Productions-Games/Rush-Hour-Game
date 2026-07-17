using UnityEngine;

public class EnemyIntroSpecialBehavior : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private string type;

    private EnemyMoveBehavior move;
   
    private void Awake()
    {
        move = GetComponent<EnemyMoveBehavior>();
    }

    private void Start()
    {
        SetMoveOverride(false);
    }

    public void SetMoveOverride(bool allowMove)
    {
        move.walkDisabledOverride = !allowMove;
    }

    private void OnDestroy()
    {
        switch (type)
        {
            case "DRUG MAN":
                GameIntroductionManager.Instance.RemoveDrugman();
                break;

            case "THIEF":
                GameIntroductionManager.Instance.RemoveThief();
                break;

            case "BYSTANDER":
                GameIntroductionManager.Instance.RemoveBystander();
                break;
        }
        
    }
}