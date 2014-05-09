using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
	
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Stats>().pickupKey();
            Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * 50* Time.deltaTime, Space.World);
	}
}
