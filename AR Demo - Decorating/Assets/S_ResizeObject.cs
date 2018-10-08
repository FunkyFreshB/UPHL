using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ResizeObject : MonoBehaviour, I_TappedInterface {

    public bool makesLarger = true;
    GameObject fo;

    public void OnTapped()
    {
        fo = Camera.main.GetComponent<S_UserInput>().fo;

        if (makesLarger)
        {
            fo.transform.localScale *= 1.05f;
        }
        else
        {
            fo.transform.localScale *= 0.95f;
        }
    }


    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
