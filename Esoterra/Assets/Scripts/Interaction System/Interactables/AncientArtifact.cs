using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AncientArtifact : DialogueUser
{
    [Header("Ancient Artifact")]
    public AudioClip firstInteractAudio;
    public Material activeCrystalMaterial;

    // To edit or enable on first interaction
    GameObject outerCrystals;
    GameObject specialCrystals;

    bool interacted = false;


    public override void Reset()
    {
        displayName = "Ancient Artifact";
        interactionVerb = "Touch";
        interactionDistance = 4f;
        nextButtonFinalString = "End";
    }
    
    public override void Awake()
    {
        base.Awake();
        outerCrystals = GameObject.Find("Ancient Artifact/Crystals/Outer");
        specialCrystals = GameObject.Find("Ancient Artifact/Crystals/Special");
    }
    
    public override void Interact()
    {
        if (!interacted) {
            interacted = true;
            FirstInteract();
        }
        base.Interact();
    }

    void FirstInteract()
    {
        AudioSource.PlayClipAtPoint(firstInteractAudio, transform.position, 0.6f);
        specialCrystals.SetActive(true);
        outerCrystals.transform.GetChild(0).gameObject.SetActive(true);
        outerCrystals.transform.GetChild(1).gameObject.SetActive(true);
        outerCrystals.GetComponent<MeshRenderer>().materials = new Material[]{
            activeCrystalMaterial
        };
    }
}
