using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour, Enemy {

    private GameObject prefab;
    private bool firing = false;
    private Transform inicial;
    private float health = 50;
    private Transform player;

	// Use this for initialization
	void Start () {
        prefab = Resources.Load("Prefab/Bullet") as GameObject;
        inicial = transform;
        StartCoroutine(fireBullet());
	}
	
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Player")
        {         
            if (!firing)
            {
                transform.LookAt(new Vector3(other.transform.position.x, other.transform.position.y + 2f, other.transform.position.z));
                firing = true;
                player = other.gameObject.transform;
            }
        }
    }

    private IEnumerator fireBullet()
    {
        while (true)
        {
            if (firing)
            {
                GameObject bullet = Instantiate(prefab, transform.position + 2 * transform.forward, transform.rotation) as GameObject;
                bullet.rigidbody.AddForce(((player.position + new Vector3(0f, 2f, 0f)) - transform.position).normalized * 1000);
            }
            yield return new WaitForSeconds(0.5f);
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

    public void damage(float damage)
    {
        if (health - damage < 0)
        {
            Destroy(this.gameObject);
        }
        else health -= damage;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
