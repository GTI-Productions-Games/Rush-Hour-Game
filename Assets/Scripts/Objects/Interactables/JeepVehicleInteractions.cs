using System.Collections;
using UnityEngine;

public class JeepVehicleInteractions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] public bool isTamed = false;
    [SerializeField] public int ridePrice;

    [Header("Attachments - Non tamed")]
    [SerializeField] private GameObject jeepRobot;    
    [SerializeField] private Transform robotRelative;

    private bool interacted = false;

    public int InteractWithJeep(int currentCoins)
    {
        if (interacted)
        {
            PopUpManager.Instance.ShowTooManyActions();
            return 0;
        }

        interacted = true;

        if (!isTamed)
        {
            PopUpManager.Instance.ShowNormalCustom("You must defeat the robot to ride the jeep.");
            StartJeepRobotFight();

            interacted = false;
            return 0;
        }

        if (currentCoins < ridePrice)
        {
            PopUpManager.Instance.ShowInsufficientMoney();

            interacted = false;
            return 0;
        }

        GameNarrativeManager.Instance.JeepAcquired();
        interacted = false;

        Destroy(gameObject);

        return ridePrice;
    }



    private void StartJeepRobotFight()
    {
        Instantiate(jeepRobot, robotRelative.position, Quaternion.identity);
        Destroy(gameObject);
    }
}