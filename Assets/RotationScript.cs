using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody.AddTorque(transform.up * 0.2f * Time.deltaTime);
	}
}
