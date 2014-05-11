using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("destroyme", 5);
	}

    void destroyme()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Stats>().decreaseHealth();
            Destroy(transform.gameObject);
        }
        else if (other.gameObject.tag != "Bullet")
            Destroy(transform.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
