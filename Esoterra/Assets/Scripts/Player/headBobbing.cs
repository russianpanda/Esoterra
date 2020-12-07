using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headBobbing : MonoBehaviour
{
    private float timer = 0.0f;

    public float speed; //bobbing speed
    public float amount; //bobbing amount
    public float midpoint; //where the camera returns to

    private float waveslice; //a section of the sine wave

    private float translateChange;

    private float totalAxes;
    void Update()
    {
        waveslice = 0.0f; //start of sine wave
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalAxis) ==0 && Mathf.Abs(verticalAxis) ==0){
            timer = 0.0f; //if the person isnt moving reset the timer
        }
        else{
            waveslice = Mathf.Sin(timer); //waveslice is a value between -1 and 1
            //its inputting the timer value into a sin function.

            timer = timer + speed; //intervals of the sine wave

            if (timer > Mathf.PI * 2){
                timer = timer - (Mathf.PI * 2);  
                //if timer is more than a full sine wave, take away the sine wave to start the loop to get a positive value
                // otherwise it would increase timer
            }
        }
        if (waveslice !=0){
            translateChange = waveslice*amount;
            totalAxes = Mathf.Abs(horizontalAxis) + Mathf.Abs(verticalAxis); //takes the positve value when adding axis.
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f); //moves the axis between 0 and 1
            translateChange = totalAxes * translateChange;
            
            transform.position=new Vector3(transform.position.x,midpoint + translateChange,transform.position.z);
            //moves the camera in the y axis

        }
        else{
            // when the player lifts up the key, the camera goes back to the normal y positon
            // and stops bobbing
            transform.position = new Vector3(transform.position.x,midpoint,transform.position.z);
        }
    }
}
