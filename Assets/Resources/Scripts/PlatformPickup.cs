using UnityEngine;
using System.Collections;

public class PlatformPickup : MonoBehaviour {

    public GameObject platform;
    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Stats>().increaseScore(100);
            platform.GetComponent<TriggeredMovingPad>().activate();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.World);
    }
}
