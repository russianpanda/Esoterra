﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Interactable : MonoBehaviour
{
    [Header("Interactable Details")]
    [Tooltip("A human-readable name displayed to the player.")]
    public string displayName;
    [Tooltip("What the player will do with this interactable.")]
    public string interactionVerb = "Interact";
    [Tooltip("How close the player needs to enable interaction.")]
    [Range(1.0f, 10.0f)]
    public float interactionDistance = 4f;
    [Tooltip("Target object, if not this gameObject. Must have a collider.")]
    public GameObject outlineObject;
    public TMP_Text displayNameText;
    public TMP_Text interactionVerbText;

    [HideInInspector] public string interactKey = "e";
    [HideInInspector] public AudioSource[] audioSources;
    [HideInInspector] public PlayerRaycasting playerController;


    public virtual void Awake()
    {
        audioSources = gameObject.GetComponents<AudioSource>();
        SetWorldSpaceText();
    }
    
    public virtual void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRaycasting>();
	}

    void Update()
    {
        ToggleWorldSpaceText();
        TryInteract();
    }

    public virtual void SetWorldSpaceText()
    {
        displayNameText.GetComponent<TextMeshPro>().text = displayName;
        interactionVerbText.GetComponent<TextMeshPro>().text =
            interactionVerb + " [" + interactKey.ToUpper() + "]";
    }

    // Show or hide name and verb above interactables in the world
    // Player must be in range
    public virtual void ToggleWorldSpaceText()
    {
        if (PlayerInRange())
        {
            SetActiveObjects(true, new GameObject[] {
                displayNameText.gameObject,
                interactionVerbText.gameObject
            });
        }
        else
        {
            SetActiveObjects(false, new GameObject[] {
                displayNameText.gameObject,
                interactionVerbText.gameObject
            });
        }
    }

    // Set the active state of multiple GameObjects at once
    public void SetActiveObjects(bool boolean, GameObject[] objects)
    {
        foreach (GameObject obj in objects){
            obj.SetActive(boolean);
        }
    }

    // Check if the player is within range of this interactable
    public bool PlayerInRange()
    {
        if (playerController) {
            float playerDistance = playerController.DistanceTo(gameObject);
            if (playerDistance <= interactionDistance) {
                return true;
            }
        }
        return false;
    }

    // Return true if player is in range and looking at interactable
    public virtual bool CanInteract()
    {
        if (PlayerInRange() && playerController.LookingAt() == gameObject) {
            return true;
        }
        return false;
    }

    // Do interact on condition
    void TryInteract()
    {
        if (CanInteract() && Input.GetKeyDown(interactKey)) {
            Interact();
        }
    }

    public bool HasAudio()
    {
        if (audioSources.Length > 0) {
            return true;
        }
        return false;
    }

    public virtual void Interact()
    {
        if (HasAudio()){
            audioSources[0].Play();
        }
    }
}
