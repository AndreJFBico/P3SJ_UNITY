using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    private GameObject end;
	// Use this for initialization
	void Start () {
        end = GameObject.FindWithTag("End");
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Stats>().pickupKey();
            other.GetComponent<Stats>().increaseScore(100);
            Color c = Color.green;
            c.a = 0.19f;
            end.transform.renderer.material.SetColor("_Color", c);
            Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * 50* Time.deltaTime, Space.World);
	}
}
