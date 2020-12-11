using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Toggler : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject[] toggleObjInRange;
    public GameObject[] toggleObjOnUse;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Toggle(toggleObjInRange);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            Toggle(toggleObjInRange);
        }
    }

    public void Toggle(GameObject[] objects)
    {
        foreach (GameObject obj in objects) {
            bool state = obj.activeSelf;
            obj.SetActive(!state);
        }
    }
}
