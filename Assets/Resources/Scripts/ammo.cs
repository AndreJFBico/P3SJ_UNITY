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
    void OnColliderEnter(Collider col)
    {
        Destroy(this);
    }
	
	public float getDamage() {
		return damage;
	}
}
