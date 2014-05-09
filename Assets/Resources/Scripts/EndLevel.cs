using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ;//endlevel
    }

	// Update is called once per frame
	void Update () {
	
	}
}
