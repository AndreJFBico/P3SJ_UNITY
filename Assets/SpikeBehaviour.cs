using UnityEngine;
using System.Collections;

public class SpikeBehaviour : MonoBehaviour {

    private GameObject spike;
    private GameObject s;
    private bool spikesToBeLaunched = false;
    private bool spikesLaunched = false;

	// Use this for initialization
	void Start () {
        spike = Resources.Load("Prefab/Spike") as GameObject;
	}
	
    public void launchSpike()
    {
        if (!spikesToBeLaunched)
        {
            spikesToBeLaunched = true;
            Invoke("releaseSpike", 1.5f);
        }
    }

    private void releaseSpike()
    {
        if (spikesToBeLaunched)
        {
            spikesLaunched = true;
            s = Instantiate(spike, transform.position, transform.rotation) as GameObject;
        }
    }

    public void destroySpike()
    {
        spikesToBeLaunched = false;
        spikesLaunched = false;
        Destroy(s);
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
