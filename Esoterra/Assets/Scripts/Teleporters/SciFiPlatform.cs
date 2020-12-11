using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiPlatform : RandomTeleporter
{
    [Header("SciFiPlatform")]
    public GameObject[] outerRings;
    public Material[] outerRingMaterials;
    public Material unlockedMaterial;


    public override void unlock()
    {
        base.unlock();

        foreach (GameObject ring in outerRings) {
            ring.GetComponent<MeshRenderer>().materials = new Material[]{
                unlockedMaterial, outerRingMaterials[1], outerRingMaterials[2]
            };
        }
    }
}
