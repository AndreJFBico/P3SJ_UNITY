using UnityEngine;
using System.Collections;

public class DeactivateTrap : MonoBehaviour {

    private Movement trap;
	// Use this for initialization
	void Start () {
        trap = transform.parent.GetComponentInChildren<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        trap.setDeactivated(true);
    }
}
