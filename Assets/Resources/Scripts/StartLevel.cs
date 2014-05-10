using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = Resources.Load("Prefab/Rambo") as GameObject;
        Instantiate(player, transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
