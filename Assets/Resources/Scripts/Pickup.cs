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
            other.GetComponent<Stats>().increaseScore(100);
            Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * 50* Time.deltaTime, Space.World);
	}
}
