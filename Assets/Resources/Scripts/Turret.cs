using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    private GameObject prefab;
    private bool firing = false;
    private Transform inicial;

	// Use this for initialization
	void Start () {
        prefab = Resources.Load("Prefab/Bullet") as GameObject;
        inicial = transform;
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            firing = true;
            transform.LookAt(other.transform);
            StartCoroutine("fireBullet");
        }
    }

    private IEnumerator fireBullet()
    {
        while (firing)
        {
            GameObject bullet = Instantiate(prefab, transform.position + 2 * transform.forward, transform.rotation) as GameObject;
            bullet.rigidbody.AddForce(transform.forward * 3000);
            yield return new WaitForSeconds(1.5f);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.LookAt(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            firing = false;
            transform.rotation = inicial.rotation;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
