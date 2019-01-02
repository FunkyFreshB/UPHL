using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.ToolTips;


public class SensorManager : MonoBehaviour {

    List<GameObject> sensorList;
    public readonly string getEventsUrl = "http://192.168.1.195:8080/getevents.py";
    public readonly string setEventsUrl = "http://192.168.1.195:8080/setevents.py";
    float timer;
    string lastUpdateTime;
    public bool isLarge;
    public Material lampOn;
    public Material lampOff;
    public Material sensorOn;

    private void Start() {
        sensorList = new List<GameObject>();
        FindSensors(gameObject);
#if UNITY_EDITOR
        foreach (GameObject obj in sensorList)
            Debug.Log("Sensor found in: " + obj);
#endif
        isLarge = false;
        timer = 2.0f;
        lastUpdateTime = "";
        //Sync model with reality
        WWW request = new WWW("http://192.168.1.195:8080/getevents.py?ts=2000-01-01T00:00:00");
        StartCoroutine(OnResponse(request));
    }

    private void FindSensors(GameObject parent) {
        for (int i = 0; i < parent.transform.childCount; i++) {
            FindSensors(parent.transform.GetChild(i).gameObject);
        }
        if (parent.GetComponent<Sensor>() != null) {
            sensorList.Add(parent);
        }
    }

    public IEnumerator OnResponse(WWW req) {
        yield return req;
        UpdateSensors(ProcessJSON(req.text));
    }

    [System.Serializable]
    public class SensorData {
        public String resource;
        public bool sample;
        public String db_time_stamp;
    }
    
    private SensorData[] ProcessJSON(string jsonString) {
        string[] jsonArray = jsonString.Split('\n');
        //jsonArray.Length-1 because jsonString.Split('\n'); generates one extra empty space at the end of the array which we don't need.
        SensorData[] dataArray = new SensorData[jsonArray.Length-1];
        SensorData newData;
        for (int i = 0; i<dataArray.Length; i++) {
            jsonArray[i] = jsonArray[i].Replace("'", "\"");
            jsonArray[i] = jsonArray[i].Replace(",)", "");
            jsonArray[i] = jsonArray[i].Remove(0, 1);
            jsonArray[i] = jsonArray[i].Replace("True", "true");
            jsonArray[i] = jsonArray[i].Replace("False", "false");
            newData = JsonUtility.FromJson<SensorData>(jsonArray[i]);
            lastUpdateTime = newData.db_time_stamp.Replace("T", " ");
            lastUpdateTime = lastUpdateTime.Remove(19, newData.db_time_stamp.Length - 19);
            newData.db_time_stamp = lastUpdateTime;
            dataArray[i] = newData;
        }
        return dataArray;
    }

    private void UpdateSensors(SensorData[] dataArray) {
        foreach (SensorData newData in dataArray) {
            if (newData.resource != null) {
                foreach (GameObject sensorObject in sensorList) {
                    Sensor sensor = sensorObject.GetComponent<Sensor>();
                    if (newData.resource.Equals(sensor.resource)) {
                        sensor.sample = newData.sample;
                        sensor.db_time_stamp = newData.db_time_stamp;
                        if (sensor.sample && sensor.isLamp) {
                            sensor.sensorObject.GetComponent<Renderer>().material = lampOn;
                            sensor.Lamp.SetActive(true);
                        } else if (!sensor.sample && sensor.isLamp) {
                            sensor.sensorObject.GetComponent<Renderer>().material = lampOff;
                            sensor.Lamp.SetActive(false);
                        } else if (sensor.sample && !sensor.isLamp) {
                            sensor.sensorObject.GetComponent<Renderer>().material = sensorOn;
                        } else {
                            sensor.sensorObject.GetComponent<Renderer>().material = sensor.originalMaterial;
                        }
                    }
                }
            }
        }
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 1.0f;
            string url = getEventsUrl + "?ts=" + lastUpdateTime;
            WWW request = new WWW(url);
            StartCoroutine(OnResponse(request));
        }
    }

}
