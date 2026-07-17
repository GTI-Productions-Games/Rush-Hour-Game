using UnityEngine;

public class GameIntroductionManager : MonoBehaviour
{
    [Header("Tutorial Drug Man Attachments")]
    [SerializeField] private EnemyIntroSpecialBehavior[] drugmen;
    [SerializeField] private EnemyIntroSpecialBehavior[] thieves;
    [SerializeField] private EnemyIntroSpecialBehavior[] bystanders;

    [Header("Blockage Atachments")]
    [SerializeField] private GameObject part1Blockage;
    [SerializeField] private GameObject part2Blockage;

    [Header("Config Counts")]
    public int drugmanCount = 1;
    public int thiefCount = 1;
    public int bystandersCount = 1;

    private bool part1Finished = false;
    private bool part2Finished = false;

    public static GameIntroductionManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {       
        drugmanCount = drugmen.Length;
        thiefCount = thieves.Length;
        bystandersCount = bystanders.Length;
    }

    public void InitiateDrugMan()
    {
        foreach(EnemyIntroSpecialBehavior drugman in drugmen)
        {
            drugman.SetMoveOverride(true);
        }
    }

    public void InitiateThief()
    {
        foreach (EnemyIntroSpecialBehavior thief in thieves)
        {
            thief.SetMoveOverride(true);
        }
    }

    public void InitiateBystander()
    {
        foreach (EnemyIntroSpecialBehavior bystander in bystanders)
        {
            bystander.SetMoveOverride(true);
        }
    }

    public void RemoveDrugman()
    {
        drugmanCount--;
        CheckForPart1Finished();
    }

    public void RemoveThief()
    {
        thiefCount--;
        CheckForPart1Finished();
    }

    private void CheckForPart1Finished()
    {
        bool drugmanCleared = drugmanCount <= 0;
        bool thiefCleared = thiefCount <= 0;

        part1Finished = drugmanCleared && thiefCleared;

        if (part1Finished)
        {
            Destroy(part1Blockage);
        }
    }

    public void RemoveBystander()
    {
        bystandersCount--;
        CheckForPart2Finished();
    }

    private void CheckForPart2Finished()
    {
        bool bystanderCleared = bystandersCount <= 0;

        part2Finished = bystanderCleared;

        if (part2Finished)
        {
            Destroy(part2Blockage);
        }
    }
}