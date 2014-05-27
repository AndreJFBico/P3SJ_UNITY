using UnityEngine;
using System.Collections;

public class TriggeredMovingPad : MonoBehaviour {

    private float forceApplied = 9.5f;
    private bool movingForward = true;
    private bool insideTrigger = true;
    private bool activated = false;

    void onAwake()
    {
        //rigidbody.AddForce(transform.forward * 0.1f, ForceMode.Impulse);
    }

    public void activate()
    {
        activated = true;
    }

    void OnTriggerStay(Collider col)
    {
        if (activated)
        {
            if (col.transform.tag == "MovingArea")
            {
                if (movingForward)
                {
                    rigidbody.AddForce(transform.forward * forceApplied, ForceMode.Acceleration);
                }
                else
                {
                    rigidbody.AddForce(-transform.forward * forceApplied, ForceMode.Acceleration);
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "MovingArea")
        {
            insideTrigger = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (insideTrigger)
        {
            if (col.transform.tag == "MovingArea")
            {
                // rigidbody.velocity = Vector3.zero;
                if (movingForward)
                {
                    //Debug.Break();
                    movingForward = false;
                }
                else
                {
                    movingForward = true;
                }
                insideTrigger = false;
            }

        }
    }

    void FixedUpdate()
    {
        if (!insideTrigger)
        {
            if (movingForward)
            {
                rigidbody.AddForce(transform.forward * forceApplied, ForceMode.Acceleration);
            }
            else
            {
                rigidbody.AddForce(-transform.forward * forceApplied, ForceMode.Acceleration);
            }
        }
    }
}
