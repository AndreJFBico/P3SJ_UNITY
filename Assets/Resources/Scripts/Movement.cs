using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public GameObject start;
    public GameObject end;

    private bool goingStart, deactivated;
    public int speed;

	// Use this for initialization
	void Start () {
        goingStart = true;
        deactivated = false;
        speed = 3;
        rigidbody.AddTorque(Vector3.right * 10);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (!deactivated)
        {
            
            transform.position = transform.position;
            if (goingStart)
            {
                transform.Translate((start.transform.position - transform.position) * (Time.deltaTime * speed), Space.World);
            }
            else
            {
                transform.Translate((end.transform.position - transform.position) * (Time.deltaTime * speed), Space.World);
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Lside")
        {
            goingStart = false;
            //Debug.Log("Hit left side");
        }
        else if (col.gameObject.tag == "Rside")
        {
            goingStart = true;
            //Debug.Log("Hit right side");
        }
        else if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Stats>().fellOff();
        }
    }

    public void setDeactivated(bool s)
    {
        deactivated = s;
    }
}
