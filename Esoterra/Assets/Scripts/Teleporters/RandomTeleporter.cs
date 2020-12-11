using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomTeleporter : Teleporter
{
    // RandomTeleporter
    [Header("RandomTeleporter")]
    public AudioClip unlockSound;
    public GameObject continuousParticles;
    public string teleportActionKey = "e";

    bool unlocked = false;
    RandomTeleporter[] randomTeleporters;


    public override void Start()
    {
        base.Start();
        randomTeleporters = GameObject.FindObjectsOfType<RandomTeleporter>();
    }

    void Update()
    {
        if (onTeleportTrigger && Input.GetKeyDown(teleportActionKey)) {
            TryRandomTeleport();
        }
    }
    
    public override void OnTriggerEnter()
    {
        onTeleportTrigger = true;
        if (!unlocked) unlock();
    }

    public virtual void unlock()
    {
        unlocked = true;
        if (unlockSound) AudioSource.PlayClipAtPoint(unlockSound, player.transform.position, 1);

        continuousParticles.SetActive(true);
        continuousSound.enabled = true;
        SpawnParticles(teleportActionParticles);
    }

    void TryRandomTeleport()
    {
        List<RandomTeleporter> viableTeleporters = GetViableTeleporters();

        // Don't do anything if there is nowhere to go!
        if (viableTeleporters.Count == 0) return;     

        // Otherwise, pick a target teleporter at random
        RandomTeleporter targetTeleport = viableTeleporters[Random.Range(0, viableTeleporters.Count)];
        Teleport(targetTeleport);
    }

    // Viable teleporters are:
    // - Already unlocked by the player
    // - Not the RandomTeleporter being currently visited
    List<RandomTeleporter> GetViableTeleporters()
    {
        List<RandomTeleporter> viableTeleporters = new List<RandomTeleporter>();
        foreach (RandomTeleporter t in randomTeleporters) {
            if (t.unlocked && t.transform!=gameObject.transform) {
                viableTeleporters.Add(t);
            }
        }
        return viableTeleporters;
    }
}
