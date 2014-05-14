using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {

    private GameObject player;
	// Use this for initialization
	void Awake () {
        player = Resources.Load("Prefab/Rambo") as GameObject;
        Instantiate(player, transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
	
	}

    public GameObject getPlayer()
    {
        return player;
    }
}
