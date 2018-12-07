using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.ToolTips;


public class SensorManager : MonoBehaviour {

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

    public Material lampOn;
    public Material lampOff;
    public Material sensorOn;

    public List<SensorData> sensorDataList;

    string webServiceUrl = "http://192.168.1.195:8080/getevents.py";
    float timer;
    string lastUpdateTime;

    private void Start() {
        timer = 2.0f;
        lastUpdateTime = "";
        sensorDataList = new List<SensorData>();

        WWW request = new WWW(webServiceUrl);
        StartCoroutine(OnResponse(request));

        //id = "a78a81027d6e4fa5980ec51d32598212";
        
        addSensorData(Lampbutton_Livingroom_2, "a78a81027d6e4fa5980ec51d32598212", "Lamp button");
        addSensorData(Livingroom_Sofa_Cushion_1, "_resource_", "Pressure sensor");
    }

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
        Processjson(req.text);
    }
    
    private void Processjson(string jsonString) {
        //Debug.Log(jsonString);
        string[] jsonArray = jsonString.Split('\n');
        //Debug.Log(jsonArray.ToString());


        SensorData data = new SensorData();
     
        for (int i = 0; i<jsonArray.Length-1; i++) {
            data.resource = "";

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

            foreach (SensorData sensorData in sensorDataList) {
                string s = sensorData.resource;
                if (data.resource != null) {
                    if (data.resource.Equals(s)) {
                        sensorData.sample = data.sample;
                        sensorData.db_time_stamp = data.db_time_stamp;

                        if (sensorData.sample) {
                            sensorData.sensorObject.GetComponent<Renderer>().material = lampOn;
                            /*if (sensorData.resource.Equals("a78a81027d6e4fa5980ec51d32598212")) {
                                Light_Table.SetActive(true);
                            }*/
                        }
                        else {
                            sensorData.sensorObject.GetComponent<Renderer>().material = lampOff;
                            /*if (sensorData.resource.Equals("a78a81027d6e4fa5980ec51d32598212")) {
                                Light_Table.SetActive(false);
                            }*/
                        }
                    }
                }
            }
        }
    }
}
