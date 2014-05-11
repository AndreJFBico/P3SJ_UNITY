using UnityEngine;
using System.Collections;

public class EnemyAgent : MonoBehaviour
{

    private GameObject prefab;
    private bool firing = false;
    private Transform inicial;
    private float health = 50;
    private NavMeshAgent agent;
    private bool followPlayer;
    private Transform player;
    private bool found = false;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        prefab = Resources.Load("Prefab/Bullet") as GameObject;
        inicial = transform;
        StartCoroutine(fireBullet());     
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {          
            transform.LookAt(new Vector3(other.transform.position.x, other.transform.position.y + 2f, other.transform.position.z));
            if (!firing)
            {
                firing = true;               
                player = other.gameObject.transform;
                found = true;
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
                bullet.rigidbody.AddForce(transform.forward * 1000);
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
    void Update()
    {
        if (found && player != null)
            agent.SetDestination(player.transform.position - player.transform.forward * 1f);
    }
}
