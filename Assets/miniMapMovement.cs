using UnityEngine;
using System.Collections;

public class miniMapMovement : MonoBehaviour {

    //private GameObject player;
    private GameObject target;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player");
        //player = target.GetComponent<StartLevel>().getPlayer();
        //Debug.Log(player);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(target!= null)
        {
            transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        }      
	}
}
