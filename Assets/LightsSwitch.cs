using UnityEngine;
using System.Collections;

public class LightsSwitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            gameObject.GetComponent<Light>().enabled = true;
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //        gameObject.GetComponent<Light>().enabled = false;
    //}

	// Update is called once per frame
	void Update () {
	
	}
}
