using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_DraggableObj : MonoBehaviour {

    bool holding = false;

	// Use this for initialization
	void Start () {
		
	}
	
    void OnHoldStart()
    {
        holding = true;
    }

    void OnHoldCompleted()
    {
        holding = false;
    }

	// Update is called once per frame
	void Update () {
        if (holding)
        {
            transform.position = Camera.main.GetComponent<S_UserInteraction>().dragPos;
        }
	}
}
