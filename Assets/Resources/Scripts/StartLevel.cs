﻿using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {

    private GameObject player;
	// Use this for initialization
	void Awake () {
        player = Resources.Load("Prefab/Rambo") as GameObject;
        player.transform.rotation *= Quaternion.Euler(0, 180, 0);
        player = Instantiate(player, transform.position - new Vector3(0, 2.8f, 0), transform.rotation) as GameObject;
        
	}

	// Update is called once per frame
	void Update () {
	
	}

    public GameObject getPlayer()
    {
        return player;
    }
}
