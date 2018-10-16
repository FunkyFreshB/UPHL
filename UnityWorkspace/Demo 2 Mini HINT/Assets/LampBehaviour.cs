using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampBehaviour : MonoBehaviour {

    [Tooltip("Lamp to turn on")]
    [SerializeField]
    private Light lightSource;
    
	// Use this for initialization
	void Start () {
        if (lightSource == null)
            throw new System.NullReferenceException();
	}

    public void LightOn() {
        lightSource.enabled = true;
    }

    public void LightOff() {
        lightSource.enabled = false;
    }
}
