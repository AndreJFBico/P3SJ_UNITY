using UnityEngine;
using System.Collections;

public class FellOff : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Stats>().fellOff();
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
