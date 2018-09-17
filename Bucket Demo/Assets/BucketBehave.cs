using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketBehave : MonoBehaviour {

    public GameObject handle;
    public GameObject bucket;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        handle.transform.position = bucket.transform.position;

        handle.transform.rotation = 
            Quaternion.Euler(bucket.transform.rotation.x,
            bucket.transform.rotation.y,
            bucket.transform.rotation.z);
	}
}
