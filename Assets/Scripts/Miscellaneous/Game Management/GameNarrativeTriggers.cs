using Unity.VisualScripting;
using UnityEngine;

public enum NarrativeTriggers
{
    DrugManIntroduce,
    ThiefIntroduce,
    BystanderIntroduce,
    AfterFirstFight,
    DarkAlleyIntroduce,
    AfterJeepRide
}

public class GameNarrativeTriggers : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private NarrativeTriggers triggerType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckNarrativeTrigger();
        }
    }

    private void CheckNarrativeTrigger()
    {
        switch (triggerType)
        {
            case NarrativeTriggers.DrugManIntroduce:
                GameNarrativeManager.Instance.IntroduceDrugMan();
                break;

            case NarrativeTriggers.ThiefIntroduce:
                GameNarrativeManager.Instance.IntroduceThief();
                break;

            case NarrativeTriggers.BystanderIntroduce:
                GameNarrativeManager.Instance.IntroduceBystander();
                break;

            case NarrativeTriggers.DarkAlleyIntroduce:
                GameNarrativeManager.Instance.IntroduceDarkAlleys();
                break;
            
            case NarrativeTriggers.AfterJeepRide:
                GameNarrativeManager.Instance.AfterJeepRide();
                break;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (triggerType == NarrativeTriggers.AfterFirstFight)
        {
            GameNarrativeManager.Instance.AfterFirstFight();
        }
    }
}