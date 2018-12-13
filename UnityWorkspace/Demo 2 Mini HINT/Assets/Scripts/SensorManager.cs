using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.ToolTips;


public class SensorManager : MonoBehaviour {

    /*
    public GameObject Bedroom_Bed;
    public GameObject Bedroom_Chair_1;
    public GameObject Bedroom_Chair_2;
    public GameObject Bedroom_Dresser_1_Drawer_1;
    public GameObject Bedroom_Dresser_1_Drawer_2;
    public GameObject Bedroom_Dresser_1_Drawer_3;
    public GameObject Bedroom_Dresser_1_Drawer_4;
    public GameObject Bedroom_Dresser_2_Drawer_1;
    public GameObject Bedroom_Dresser_2_Drawer_2;
    public GameObject Bedroom_Dresser_2_Drawer_3;
    public GameObject Bedroom_Dresser_2_Drawer_4;
    public GameObject Bedroom_Dresser_2_Drawer_5;
    public GameObject Bedroom_Dresser_2_Drawer_6;
    public GameObject Bedroom_Dresser_3_Drawer_1;
    public GameObject Bedroom_Dresser_3_Drawer_2;
    public GameObject Bedroom_Dresser_3_Drawer_3;
    public GameObject Bedroom_Dresser_3_Drawer_4;
    public GameObject Bedroom_Dresser_3_Drawer_5;
    public GameObject Bedroom_Nightstand_Drawer;
    public GameObject Floor_Bathroom;
    public GameObject Floor_Bedroom;
    public GameObject Floor_Kitchen;
    public GameObject Floor_Livingroom_1;
    public GameObject Floor_Livingroom_2;
    public GameObject Floor_Livingroom_3;
    public GameObject Kitchen_Left_Cabinet_1;
    public GameObject Kitchen_Left_Cabinet_2;
    public GameObject Kitchen_Left_Cabinet_3;
    public GameObject Kitchen_Left_Cabinet_4;
    public GameObject Kitchen_Left_Dishwasher;
    public GameObject Kitchen_Left_Drawer_1;
    public GameObject Kitchen_Left_Drawer_2;
    public GameObject Kitchen_Left_Drawer_3;
    public GameObject Kitchen_Refridgerator;
    public GameObject Lampbutton_Bathroom;
    public GameObject Lampbutton_Bedroom;
    public GameObject Lampbutton_Kitchen;
    public GameObject Lampbutton_Livingroom_1;
    public GameObject Lampbutton_Livingroom_2;
    public GameObject Lampbutton_Livingroom_3;
    public GameObject Livingroom_Chair_1;
    public GameObject Livingroom_Chair_2;
    public GameObject Livingroom_Chair_Cover_1;
    public GameObject Livingroom_Chair_Cover_2;
    public GameObject Livingroom_Chair_Cover_3;
    public GameObject Livingroom_Chair_Cover_4;
    public GameObject Livingroom_Sofa_Cushion_1;
    public GameObject Livingroom_Sofa_Cushion_2;
    public GameObject Livingroom_Sofa_Cushion_3;
    public GameObject Livingroom_Sofa_Cushion_1_1;
    public GameObject Livingroom_Sofa_Cushion_2_1;
    public GameObject Livingroom_Sofa_Cushion_2_2;
    public GameObject Livingroom_Sofa_Cushion_3_1;
    public GameObject Livingroom_Stool_1_Cushion;
    public GameObject Livingroom_Stool_2_Cushion;
    public GameObject Light_Bathroom;
    public GameObject Light_Bedroom;
    public GameObject Light_kitchen;
    public GameObject Light_Livingroom;
    public GameObject Light_Table;
    public GameObject Light_StorageArea;
    */

    public Material lampOn;
    public Material lampOff;
    public Material sensorOn;
    
    List<GameObject> sensorList;
    readonly string webServiceUrl = "http://192.168.1.195:8080/getevents.py";
    float timer;
    string lastUpdateTime;

    private void Start() {

        sensorList = new List<GameObject>();
        SetupSensorList();
#if UNITY_EDITOR
        foreach (GameObject obj in sensorList)
            Debug.Log("Sensor found in: " + obj);
#endif
        timer = 2.0f;
        lastUpdateTime = "";
        //a78a81027d6e4fa5980ec51d32598212
    }

    private void SetupSensorList() {
        FindSensors(gameObject);
    }

    private void FindSensors(GameObject parent) {
        for (int i = 0; i < parent.transform.childCount; i++) {
            FindSensors(parent.transform.GetChild(i).gameObject);
        }
        if (parent.GetComponent<Sensor>() != null)
            sensorList.Add(parent);
    }
/*
    private void addSensorData(GameObject sensorObject, String resource, String description) {
        GameObject toolTip = setupToolTip(sensorObject, resource, description);
        SensorData sensor = new SensorData(sensorObject, toolTip, resource);
        sensorDataList.Add(sensor);
    }

    private GameObject setupToolTip(GameObject sensorObject, String resource, String description) {
        GameObject toolTip = (GameObject)Instantiate(Resources.Load("SensorToolTip"));
        toolTip.GetComponent<ToolTip>().ToolTipText = description + "\n" + resource;
        toolTip.GetComponent<ToolTipConnector>().Target = sensorObject;
        //toolTip.transform.position = new Vector3(sensorObject.transform.position.x, sensorObject.transform.position.y, sensorObject.transform.position.z);
        toolTip.SetActive(false);
        return toolTip;
    }
*/
    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 1.0f;
            string url = webServiceUrl + "?ts=" + lastUpdateTime;
            WWW request = new WWW(url);
            StartCoroutine(OnResponse(request));
        }
    }

    public IEnumerator OnResponse(WWW req) {
        yield return req;
        UpdateSensors(ProcessJSON(req.text));
    }

    private class SensorData {
        public String resource;
        public bool sample;
        public String db_time_stamp;
    }

    private SensorData[] ProcessJSON(string jsonString) {

        string[] jsonArray = jsonString.Split('\n');
        //jsonArray.Length-1 because jsonString.Split('\n'); generates one extra empty space at the end of the array
        SensorData[] dataArray = new SensorData[jsonArray.Length-1];
        SensorData data;
     
        for (int i = 0; i<dataArray.Length; i++) {

            jsonArray[i] = jsonArray[i].Replace("'", "\"");
            jsonArray[i] = jsonArray[i].Replace(",)", "");
            jsonArray[i] = jsonArray[i].Remove(0, 1);
            jsonArray[i] = jsonArray[i].Replace("True", "true");
            jsonArray[i] = jsonArray[i].Replace("False", "false");
            data = JsonUtility.FromJson<SensorData>(jsonArray[i]);

            lastUpdateTime = data.db_time_stamp.Replace("T", " ");
            lastUpdateTime = lastUpdateTime.Remove(19, data.db_time_stamp.Length - 19);
            data.db_time_stamp = lastUpdateTime;

            Debug.Log("Iteration: " + i);
            Debug.Log("Resource: " + data.resource);
            Debug.Log("Sample: " + data.sample);
            Debug.Log("Time: " + data.db_time_stamp);

            dataArray[i] = data;

        }
        return dataArray;
    }

    private void UpdateSensors(SensorData[] dataArray) {

        foreach (SensorData data in dataArray) {
            if (data.resource != "") {
                foreach (GameObject sensorObject in sensorList) {
                    Sensor sensor = sensorObject.GetComponent<Sensor>();
                    if (data.resource.Equals(sensor.resource)) {
                        sensor.sample = data.sample;
                        sensor.db_time_stamp = data.db_time_stamp;

                        if (sensor.sample && sensor.isLamp) {
                            sensor.sensorObject.GetComponent<Renderer>().material = lampOn;
                            sensor.Lamp.SetActive(true);
                        }
                        else if (!sensor.sample && sensor.isLamp) {
                            sensor.sensorObject.GetComponent<Renderer>().material = lampOff;
                            sensor.Lamp.SetActive(false);
                        }
                        else if (sensor.sample && !sensor.isLamp) {
                            sensor.sensorObject.GetComponent<Renderer>().material = sensorOn;
                        }
                        else
                            sensor.sensorObject.GetComponent<Renderer>().material = sensor.originalMaterial;
                    }
                }
            }
        }
    }
}
