using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthGlobe : Pickup
{
    public override void DoAction()
    {
        Debug.Log("HealthGlobe action!");

        // Destroy
        base.DoAction();
    }
}
