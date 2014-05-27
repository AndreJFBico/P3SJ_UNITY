using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeTrap : MonoBehaviour {

    private SpikeBehaviour[] spikes;
    private bool toBeDamaged = false;
	// Use this for initialization
	void Start () {
        spikes = GetComponentsInChildren<SpikeBehaviour>();
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("AFSGD");
        if(other.tag == "Player")
        {
            for(int i = 0; i< spikes.Length; i++)
            {
                spikes[i].launchSpike();
            }
            Invoke("damagePlayer", 1.5f);
            toBeDamaged = true;
        }
    }

    private void damagePlayer()
    {
        if (toBeDamaged)
            GameObject.FindWithTag("Player").GetComponent<Stats>().decreaseHealth();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < spikes.Length; i++)
            {
                spikes[i].destroySpike();
            }
            toBeDamaged = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
