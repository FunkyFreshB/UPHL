using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RotateObject : MonoBehaviour, I_TappedInterface {

    public bool rotatesRight = true;
    GameObject fo;
	
    public void OnTapped()
    {
        fo = Camera.main.GetComponent<S_UserInput>().fo;

        if (rotatesRight) {
            fo.transform.Rotate(0, 10, 0);
        }
        else
        {
            fo.transform.Rotate(0, -10, 0);
        }
    }


    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update () {
		
	}
}
