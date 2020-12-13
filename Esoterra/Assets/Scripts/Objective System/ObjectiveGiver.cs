using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectiveGiver : DialogueUser
{
    // Track state and use this to control what happens on interaction
    public bool Given {get; set;}
    public bool Completed {get; set;}

    [Header("Objective Giver Details")]
    [Tooltip("Objective script (class name) to give to player.")]
    public string objectiveScript;
    [Tooltip("Clip to use after the first interaction when the objective has been given.")]
    public AudioClip objectiveAudioClip;
    [Tooltip("Clip to use after the objective has been completed.")]
    public AudioClip completedAudioClip;

    // Object in the hierarchy which stores the player's objectives
    GameObject playerObjectives;
    // A reference to the specific objective given
    Objective MyObjective {get; set;}


    public override void Reset()
    {
        interactionVerb = "Talk";
        interactionDistance = 6f;
    }

    public override void Start()
    {
        base.Start();
        playerObjectives =
            GameObject.FindGameObjectWithTag("Player")
            .transform.Find("Objectives").gameObject;
    }

    public override void Interact()
    {
        // Never interacted with this giver before
        if (!Given && !Completed) {
            // Original dialogue and audio
            base.Interact();
            GiveObjective();
        }

        // Change audio after interacting based on state

        // Have interacted and received the objective, but not completed it
        else if (Given && !Completed) {
            if (HasAudio()) {
                audioSources[0].clip = objectiveAudioClip;
                audioSources[0].Play();
            }
            CheckObjective();
        }

        // Have interacted and completed the objective
        else {
            if (HasAudio()) {
                audioSources[0].clip = completedAudioClip;
                audioSources[0].Play();
            }

            dialogueManager.AddNewDialogue(
                displayName,
                MyObjective.AlreadyCompletedDialogue,
                nextButtonFinalString
            );
        }
    }

    void GiveObjective()
    {
        Given = true;

        // Add objective to player and get a reference to it
        MyObjective = (Objective)playerObjectives.AddComponent(
            System.Type.GetType(objectiveScript)
        );
    }

    void CheckObjective()
    {
        if (MyObjective.Completed) {
            Completed = true;
            MyObjective.GiveReward();

            dialogueManager.AddNewDialogue(
                displayName,
                MyObjective.HandInDialogue,
                nextButtonFinalString
            );

        // Not complete yet
        } else {
            dialogueManager.AddNewDialogue(
                displayName,
                // Use GoalsStrings to get player's up to date progress
                MyObjective.GoalsStrings(MyObjective.NotCompletedDialogue),
                nextButtonFinalString
            );
        }
    }
}
