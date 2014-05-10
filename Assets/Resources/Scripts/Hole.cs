using UnityEngine;
using System.Collections;

public class Hole : MonoBehaviour {

    private bool open = false;
    private Transform part1;
    private Transform part2;
    private Transform end1;
    private Transform end2;

	// Use this for initialization
	void Start () {
        part1 = FindChild("Part1");
        part2 = FindChild("Part2");
        end1 = FindChild("End1");
        end2 = FindChild("End2");
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !open)
            transform.Translate(0, -0.2f, 0);
    }

    void OnTriggerStay()
    {
        if (!open)
        {
            if (part1.position.x > end1.position.x && part2.position.x < end2.position.x)
                open = true;
            part1.position = Vector3.MoveTowards(part1.position, part1.position + part1.right * 3, Time.deltaTime);
            part2.position = Vector3.MoveTowards(part2.position, part2.position - part1.right * 3, Time.deltaTime);
            
        }
    }

    void OnTriggerExit()
    {
        //open = false;
        //part1.localPosition = new Vector3(1.25f, part1.localPosition.y, part1.localPosition.z);
       // part2.localPosition = new Vector3(-1.25f, part2.localPosition.y, part2.localPosition.z);
        //transform.Translate(0, 0.2f, 0);
    }


	// Update is called once per frame
	void Update () {
	
	}

    private Transform FindChild(string name)
    {
        Transform[] trans = GetComponentsInChildren<Transform>();

        foreach (Transform t in trans)
        {
            if (t.gameObject.name.Equals(name))
                return t;
        }
        return null;
    }
}
