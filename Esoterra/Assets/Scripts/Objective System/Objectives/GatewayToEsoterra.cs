using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GatewayToEsoterra : Objective
{
    GameObject teleporter;


    public override void Start()
    {
        base.Start();

        // Enable repair when this Objective is started
        GameObject.Find("Compressor Tube Slow/Compressor Tube")
            .GetComponent<CompressorTube>().enabled = true;
        GameObject.Find("Compressor Tube Fast/Compressor Tube")
            .GetComponent<CompressorTube>().enabled = true;

        // Enable repair when this Objective is complete
        teleporter = GameObject.Find("Teleporter (Olympus)");

        Name = "Gateway to Esoterra";
        Description = "Go to the storage room and repair the Blue Matter Compressor";
        Goals.Add(new RepairGoal(this, 2, "CompressorTube", "Repair 2 Blue Matter Compressor Tubes.", 0, false));
        foreach (Goal g in Goals) {
            g.Init();
        }

        ResourceRewards.Add(6);

        NotCompletedDialogue = new string[]{
            "In the storage room lies the Blue Matter compressor. It compresses and extracts the energy bound inside the matter and uses it to power the advanced technologies here on Olympus.",
            "The compressor ceased to function after Olympus was damaged. If it were to be repaired, I could command it and direct the power flow to the teleporter in the Bridge.",
            "There is a chance the lost teleporter which crashed onto Esoterra remains functional...",
            "If this is the case... This could be your gateway to Esoterra.",
            "You must repair the compressor tubes to allow power to flow. Leave my room, head directly across the Bridge and into the curving corridor to reach the storage room."
        };

        HandInDialogue = new string[]{
            "...",
            "Connecting...",
            "...",
            "Blue Matter Compressor connection successful.",
            "Directing power flow to Bridge teleporter.",
            "I have given you Blue Matter. Insert this into the teleporter... This is your last task here on Olympus.",
            "This is your gateway to Esoterra."
        };

        AlreadyCompletedDialogue = new string[]{
            "I have given you Blue Matter. Insert this into the teleporter behind you. If the lost teleporter remains functional, the Bridge teleporter will transport you from here to there.",
            "This is your final task on Olympus.",
            "This is your gateway to Esoterra."
        };
    }

    public override void GiveReward()
    {
        base.GiveReward();
        teleporter.GetComponentInChildren<TeleporterToEsoterra>().enabled = true;
    }
}
