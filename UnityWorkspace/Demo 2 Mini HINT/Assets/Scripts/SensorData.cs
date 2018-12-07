using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SensorData {
    public GameObject sensorObject;
    public Material originalMaterial;
    public GameObject sensorToolTip;
    public string resource;
    public bool sample;
    public string db_time_stamp;

    public SensorData() { }

    public SensorData(GameObject go, GameObject toolTip, string resource) {
        sensorObject = go;
        originalMaterial = sensorObject.GetComponent<Renderer>().material;
        sensorToolTip = toolTip;
        this.resource = resource;
        sample = false;
    }
}
