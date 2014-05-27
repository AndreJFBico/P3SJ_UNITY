using UnityEngine;
using System.Collections;

public class DragPlayer : MonoBehaviour {

    private float forceApplied = 9.5f;
	// Use this for initialization
	void Start () {
	
	}
	
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        other.collider.attachedRigidbody.AddForce(transform.forward * forceApplied, ForceMode.Acceleration);
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
            other.collider.attachedRigidbody.AddForce(transform.forward * forceApplied, ForceMode.Acceleration);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
