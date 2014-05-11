using UnityEngine;
using System.Collections;

public class ammo : MonoBehaviour {
	
	const float damage = 10; 
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "EnemyAgent")
        {
            EnemyAgent scrpt = col.gameObject.GetComponent<EnemyAgent>();
            if(scrpt != null)
                scrpt.damage(5);
            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "Enemy")
        {
            Turret scrpt = col.gameObject.GetComponent<Turret>();
            if (scrpt != null)
                scrpt.damage(5);
            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag != "Player")
        {
            GameObject exp = (GameObject)Instantiate(Resources.Load("Prefab/expEffect"), transform.position, Quaternion.identity);
            Destroy(exp, 0.2f);
            Destroy(this.gameObject);
        }
    }
	
	public float getDamage() {
		return damage;
	}
}
