using System.Collections;
using UnityEngine;

public class GameNarrativeManager : MonoBehaviour
{  
    public static GameNarrativeManager Instance { get; private set; }

    private bool introDone;
    private bool drugmanDone;
    private bool firstFightDone;
    private bool thiefDone;
    private bool bystanderDone;
    private bool darkAlleyDone;
    private bool jeepAcquiredDone;
    private bool jeepRideDone;
    private bool moreExpensiveDone;    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Intro());
    }

    public IEnumerator Intro()
    {
        if (!introDone)
        {
            introDone = true;

            string[] lines =
            {
                "Why is it so dark outside? I wonder what time it is.",
                "Doesn't matter. All I know is I still have to get to work.",
                "And since it's dark, the streets might be crawling with thugs...",
                "...or whatever other struggles are out there.",
                "Will I make it on time?"
            };

            yield return StartCoroutine(PopUpManager.Instance.StartMonologueCoroutine(lines));

            PopUpManager.Instance.ShowControls();
        }
        else
        {
            yield return null;
        }
    }

    public void IntroduceDrugMan()
    {
        if (drugmanDone)
        {
            return;
        }

        drugmanDone = true;

        string[] lines =
        {
            "Judging by the way he looks... he must be a drug addict.",
            "I heard they're aggressive. ",
            "I must be careful.",
            "And they've got knives."
        };

        PopUpManager.Instance.StartMonologue(lines);
        GameIntroductionManager.Instance.InitiateDrugMan();
    }

    public void AfterFirstFight()
    {
        if (firstFightDone)
        {
            return;
        }

        firstFightDone = true;

        string[] lines =
        {
            "That encounter made me a bit peckish...",
            "... and dizzy.",
            "Good thing they dropped some coins.",
            "I hope there are stores open along the way.",
            "So I can buy something to eat or drink...",
            "...or maybe anything to throw at those thugs."
        };

        PopUpManager.Instance.StartMonologue(lines);        
    }

    public void IntroduceThief()
    {
        if (thiefDone)
        {
            return;
        }

        thiefDone = true;

        string[] lines =
{
            "Thieves!",
            "Are those... ",
            "...guns?",            
        };

        PopUpManager.Instance.StartMonologue(lines);
        GameIntroductionManager.Instance.InitiateThief();
    }

    public void IntroduceBystander()
    {
        if (bystanderDone)
        {
            return;
        }

        bystanderDone = true;

        string[] lines =
{
            "Bystanders!",
            "I wonder what they're planning to do with those bottles."
        };

        PopUpManager.Instance.StartMonologue(lines);
        GameIntroductionManager.Instance.InitiateBystander();
    }

    public void IntroduceDarkAlleys()
    {
        if (darkAlleyDone)
        {
            return;
        }

        darkAlleyDone = true;

        string[] lines =
        {
            "I get it now.",
            "They're coming out of those dark alleys."
        };

        PopUpManager.Instance.StartMonologue(lines);
    }

    public void JeepAcquired()
    {
        string[] lines =
        {
            "Now I can ride a jeepney.",
            "I can now cross the highway without having to walk."
        };

        PopUpManager.Instance.StartMonologue(lines);
    }

    public void AfterJeepRide()
    {
        if (jeepRideDone)
        {
            return;
        }

        jeepRideDone = true;

        StartCoroutine(AfterJeepRideSequence());
    }

    private IEnumerator AfterJeepRideSequence()
    {        
        string[] lines =
        {
            "Whew!",
            "Getting a ride is really a struggle."
        };

        yield return new WaitForSeconds(1);

        PopUpManager.Instance.StartMonologue(lines);
    }

    public void MoreExpensive()
    {
        if (moreExpensiveDone)
        {
            return;
        }

        moreExpensiveDone = true;

        string[] lines =
        {
            "What's wrong with these prices?",
            "These are much more expensive than in the small stores."
        };

        PopUpManager.Instance.StartMonologue(lines);
    }
}
