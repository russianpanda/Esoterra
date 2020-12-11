using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Teleporter : MonoBehaviour
{
    // Teleporter parent class
    [Header("Teleporter")]
    public GameObject teleportActionParticles;
    public AudioClip teleportActionSound;

    // This should be the only AudioSource on the Teleporter
    [HideInInspector] public AudioSource continuousSound;
    [HideInInspector] public bool onTeleportTrigger = false;
    [HideInInspector] public GameObject player;


    public virtual void Start()
    {
        continuousSound = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public virtual void OnTriggerEnter()
    {
        onTeleportTrigger = true;
    }
    
    void OnTriggerExit()
    {
        onTeleportTrigger = false;
    }

    public void SpawnParticles(GameObject prefab)
    {
        GameObject particles = Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(particles, 5f);
    }

    public void Teleport(Teleporter destination)
    {
        // Here
        SpawnParticles(teleportActionParticles);
        if (teleportActionSound) {
            AudioSource.PlayClipAtPoint(teleportActionSound, transform.position, 1);
        }

        // Here -> Destination
        player.transform.position = destination.transform.position + new Vector3(0, 1.66f, 0);

        // Destination
        destination.SpawnParticles(teleportActionParticles);
        if (teleportActionSound) {
            AudioSource.PlayClipAtPoint(teleportActionSound, player.transform.position, 1);
        }
    }
}
