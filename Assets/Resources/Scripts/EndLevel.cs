using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Stats playerStats = other.gameObject.GetComponent<Stats>();
            if (playerStats.haveKey())
            {
                playerStats.endGame();
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
