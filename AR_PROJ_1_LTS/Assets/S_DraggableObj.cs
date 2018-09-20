using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_DraggableObj : MonoBehaviour {

    bool holding = false;
    AudioSource assds;

	// Use this for initialization
	void Start () {
        assds = Camera.main.GetComponent<S_UserInteraction>().audioData;
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
            Debug.Log("HOLDING");
            assds.pitch = Random.Range(0.2f, 0.4f);
            assds.Play(0);
            transform.position = Camera.main.GetComponent<S_UserInteraction>().dragPos;
        }
	}
}
