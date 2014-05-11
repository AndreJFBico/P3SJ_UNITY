using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public float speed = 50f;
    public GameObject firepoint;
    public GameObject explosion;
    private GameObject intAmmo;
    private GameObject exp;
    private PersonController controller;
    private Stats stats;
    private bool firing = false;

    delegate void DelayedMethod();

	// Use this for initialization
	void Start () 
    {
        controller = transform.GetComponent<PersonController>();
        exp = Resources.Load("Prefab/expEffect") as GameObject;

        stats = GetComponent<Stats>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButton(0) && !firing)
        {
            detectTargetPosition();
        }
            
	}

    void fireTowardsTarget(Vector3 targetPos)
    {
        if (stats.bullets > 0)
        {
            controller.runFireAnim(targetPos);
            intAmmo = Instantiate(Resources.Load("Prefab/ammo"), transform.position + transform.forward * 2f + new Vector3(0f, 2f, 0f), Quaternion.identity) as GameObject;
            //intAmmo.rigidbody.velocity = transform.TransformDirection(Vector3.forward * speed);
            intAmmo.rigidbody.AddForce((targetPos - firepoint.transform.position) * speed);
            exp = (GameObject)Instantiate(Resources.Load("Prefab/expEffect"), transform.position + transform.forward * 2f + new Vector3(0f, 2f, 0f), Quaternion.identity);
            //exp.transform.parent = firepoint.transform;
            stats.decreaseBullet();
            Destroy(exp, 0.2f);
            StartCoroutine(WaitAndDo(0.2f, resetAnim));
            StartCoroutine(WaitAndDo(1.5f, explodeAmmo, intAmmo));
        }
    }

    IEnumerator WaitAndDo(float time, DelayedMethod method, GameObject ammo)
    {
        yield return new WaitForSeconds(time);
        intAmmo = ammo;
        method();
    }

    IEnumerator WaitAndDo(float time, DelayedMethod method)
    {
        yield return new WaitForSeconds(time);
        method();
    }

    public void resetAnim()
    {
        firing = false;
        controller.stopFireAnim();
    }

    public void explodeAmmo()
    {
        //exp = (GameObject)GameObject.Instantiate(explosion, intAmmo.transform.position, intAmmo.transform.rotation);
        Destroy(intAmmo);
        //Destroy(exp, 1.0f);
    }

    void detectTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
             fireTowardsTarget(hit.point);
             firing = true;
        }
        Debug.DrawRay(ray.origin, ray.direction);
    }
}
