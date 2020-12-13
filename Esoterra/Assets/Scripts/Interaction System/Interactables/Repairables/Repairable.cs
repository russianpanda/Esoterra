using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Repairable : Interactable
{
    [Header("Repairable Details")]
    [Tooltip("IDs of resources required to make the repair.")]
    public bool continuousAudioWhenBroken = false;
    public string repairButtonString = "Repair";
    public int[] requiredResources;
    public int[] requiredQuantity;

    // To be displayed in the body of the repair UI
    string bodyString;
    [HideInInspector] public bool broken;

    // Audio
    [HideInInspector] public AudioSource audioRepair;
    AudioSource continuousAudio;

    [HideInInspector] public Inventory inventory;
    [HideInInspector] public RepairManager repairManager;
    
    
    public virtual void Reset()
    {
        interactionVerb = "Repair";
    }

    public override void Awake()
    {
        base.Awake();
        ConfigureAudio();
        broken = true;
    }

    void ConfigureAudio()
    {
        // Audio on repair
        if (audioSources.Length > 0) {
            audioRepair = audioSources[0];
        }
            
        // Continuous audio
        if (audioSources.Length > 1) {
            continuousAudio = audioSources[1];
            continuousAudio.spatialBlend = 1;
            continuousAudio.playOnAwake = true;
            continuousAudio.loop = true;

            if (continuousAudioWhenBroken) {
                continuousAudio.enabled = true;
            } else {
                continuousAudio.enabled = false;
            }
        }
    }

    public override void Start()
    {
        base.Start();
        repairManager =
            GameObject.Find("UI/Canvas/TextMenu")
            .GetComponent<RepairManager>();
        inventory =
            GameObject.FindGameObjectWithTag("Player")
            .GetComponentInChildren<Inventory>();
        }

    // Update based on required Resources and quantities
    void UpdateBodyString()
    {
        bodyString = "Required resources:";

        // For each Resource in requiredResources
        for (int res=0; res<requiredResources.Length; res++) {
            bodyString += "\n"
                // Get Resource displayName from ID
                + ResourceDatabase.Instance.GetDisplayName(requiredResources[res])
                // Inventory quantity vs required quantity
                + " (" + inventory.Quantity(requiredResources[res])
                + "/" + requiredQuantity[res] + ")"
            ;
        }
    }

    // Return true if the player could repair based on Resources in their inventory
    public bool PlayerCanRepair()
    {
        // For each Resource in requiredResources
        for (int res=0; res<requiredResources.Length; res++) {
            // If the player does not have the required amount in their inventory...
            if (inventory.Quantity(requiredResources[res]) < requiredQuantity[res]){
                return false;
            }
        }
        return true;
    }

    // Do repair on condition
    public void TryRepair()
    {
        if (broken && PlayerCanRepair()) {
            Repair();
        }
    }

    public virtual void Repair()
    {
        broken = false;

        // Subtype should tell EventManager its specific type
        // EventManager.Repaired("Repairable");

        // For each Resource in requiredResources
        for (int res=0; res<requiredResources.Length; res++) {
            // Remove Resources required to make the repair
            inventory.RemoveResource(requiredResources[res], requiredQuantity[res]);
        }

        // Audio
        if (audioRepair != null) {
            audioRepair.Play();
        }
        if (continuousAudio != null) {
            // Enable or disable continuous audio after making the repair
            if (continuousAudioWhenBroken) {
                continuousAudio.enabled = false;
            } else {
                continuousAudio.enabled = true;
            }
        }

        // No longer Interactable
        gameObject.tag = "Untagged";
        gameObject.layer = 0;  // Default
        SetActiveObjects(false, new GameObject[] {
            displayNameText.gameObject,
            interactionVerbText.gameObject
        });
        gameObject.GetComponent<Repairable>().enabled = false;
    }

    public override void Interact()
    {
        UpdateBodyString();
        repairManager.AddNewRepair(displayName, bodyString, gameObject, repairButtonString);
    }
}
