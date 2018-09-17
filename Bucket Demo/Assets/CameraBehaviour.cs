using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    bool holding = false;
    GameObject fo = null;
    float dist = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 mp = Input.mousePosition; mp.z = 100; mp = Camera.main.ScreenToWorldPoint(mp);
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, mp, out hit))
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag == "Handle")
                {
                    fo = hit.collider.gameObject;
                    dist = Vector3.Distance(transform.position, hit.point);
                    Debug.Log(dist);
                    holding = !holding;
                }
            }
        }

        if (holding)
        {
            fo.transform.position = new Vector3(hit.point.x, hit.point.y, dist);
            fo.GetComponent<Rigidbody>().useGravity = false;
           // fo.GetComponentInChildren<Rigidbody>().useGravity = false;
        }
        else
        {
            if (fo != null)
            {
                fo.GetComponent<Rigidbody>().useGravity = true;
              //  fo.GetComponentInChildren<Rigidbody>().useGravity = true;
            }
            fo = null;
        }
	}
}
