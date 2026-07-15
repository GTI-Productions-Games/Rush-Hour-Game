using System.Collections;
using UnityEngine;

public class JeepVehicleInteractions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool isTamed = false;

    [Header("Attachments - Non tamed")]
    [SerializeField] private GameObject jeepRobot;
    [SerializeField] private Transform robotRelative;

    private bool interacted = false;

    public void InteractWithJeep()
    {
        if (interacted)
        {
            PopUpManager.Instance.ShowTooManyActions();
            return;
        }

        interacted = true;

        if (isTamed)
        {
            RideJeep();
        }
        else
        {
            StartJeepRobotFight();
        }
    }

    private void RideJeep()
    {

    }

    private void StartJeepRobotFight()
    {
        Instantiate(jeepRobot, robotRelative.position, Quaternion.identity);
        Destroy(gameObject);
    }
}