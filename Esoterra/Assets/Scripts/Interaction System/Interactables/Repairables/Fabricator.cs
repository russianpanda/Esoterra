using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabricator : Repairable
{
    public override void Reset()
    {
        base.Reset();
        displayName = "Fabricator";
        interactionVerb = "Deposit";
        repairButtonString = "Deposit";
        requiredResources = new int[] {1, 2, 3, 4, 5};
        requiredQuantity = new int[] {1, 1, 1, 1, 1};
        continuousAudioWhenBroken = true;
    }

    public override void Repair()
    {
        // Don't call base.Repair()
        // broken = true to allow this "repair" to be repeatable
        // Fabricator remains as an Interactable

        // For each Resource in requiredResources
        for (int res=0; res<requiredResources.Length; res++) {
            // Remove Resources required to make the "repair"
            inventory.RemoveResource(requiredResources[res], requiredQuantity[res]);
        }

        // Give the player the fabricated Blue Matter!
        inventory.AddResource(6, 1);

        // Audio
        if (audioRepair != null) {
            audioRepair.Play();
        }
    }
}
