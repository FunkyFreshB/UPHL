using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class AddScriptToManager : MonoBehaviour {
    // Use this for initialization
    void Start () {
        this.gameObject.AddComponent<SpeechInputSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
