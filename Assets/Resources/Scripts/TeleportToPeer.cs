using UnityEngine;
using System.Collections;

public class TeleportToPeer : MonoBehaviour {

    public GameObject peer;
    private bool wasTeleported = false;

	// Use this for initialization
	void Start () {
	
	}

    public void teleported()
    {
        wasTeleported = true;
    }

    void OnTriggerExit(Collider other)
    {
        wasTeleported = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!wasTeleported)
        {
            peer.GetComponent<TeleportToPeer>().teleported();
            other.transform.position = peer.transform.position - new Vector3(0, 2.8f, 0);
            other.transform.rotation = peer.transform.rotation;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
