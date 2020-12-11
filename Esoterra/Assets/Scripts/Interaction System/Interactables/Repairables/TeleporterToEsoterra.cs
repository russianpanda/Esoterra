using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeleporterToEsoterra : Repairable
{
    public GameObject activateOnRepair;


    public override void Reset()
    {
        base.Reset();
        displayName = "Teleporter";
        requiredResources = new int[] {6};
        requiredQuantity = new int[] {1};
        continuousAudioWhenBroken = false;
    }

    public override void Repair()
    {
        base.Repair();
        activateOnRepair.gameObject.SetActive(true);
    }
}
