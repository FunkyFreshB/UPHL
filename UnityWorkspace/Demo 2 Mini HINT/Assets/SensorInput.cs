using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorInput : MonoBehaviour {

    Material activeMaterial;
    Material cachedMaterial;

    void Start() {
        activeMaterial = new Material(GetComponent<Renderer>().material.shader);
        activeMaterial.color = Color.red;
        cachedMaterial = GetComponent<Renderer>().material;
    }

    public void activate() {
        GetComponent<Renderer>().material = activeMaterial;
    }

    public void deactivate() {
        GetComponent<Renderer>().material = cachedMaterial;
    }
}
