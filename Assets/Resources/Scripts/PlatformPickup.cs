using UnityEngine;
using System.Collections;

public class PlatformPickup : MonoBehaviour {

    public GameObject platform;
    private GameObject particles;
    // Use this for initialization
    void Start()
    {
        particles = (GameObject)Resources.Load("UberParticleSystem/Creations/ParticleBoom");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Instantiate(particles, transform.position, particles.transform.rotation);
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
