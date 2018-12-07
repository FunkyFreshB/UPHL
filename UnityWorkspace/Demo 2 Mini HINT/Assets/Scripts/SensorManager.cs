using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.ToolTips;


public class SensorManager : MonoBehaviour {

    public GameObject Light_table;
    public GameObject Lampbutton_Livingroom_001;
    public GameObject lampButton2;
    public GameObject lampButton3;

    List<SensorData> listSensorData;

    public string id;

    public Material lampOn;
    public Material lampOff;

    public string webServiceUrl = "http://192.168.1.195:8080/getevents.py";
    public float timer;
    public string time;

    private void Start() {
        timer = 2.0f;
        time = "";
        listSensorData = new List<SensorData>();

        WWW request = new WWW(webServiceUrl);
        StartCoroutine(OnResponse(request));

        //id = "a78a81027d6e4fa5980ec51d32598212";
        GameObject toolTip = (GameObject)Instantiate(Resources.Load("SensorToolTip"));
        toolTip.transform.position = new Vector3(Lampbutton_Livingroom_001.transform.position.x, Lampbutton_Livingroom_001.transform.position.y, Lampbutton_Livingroom_001.transform.position.z);
        toolTip.GetComponent<ToolTip>().ToolTipText = "Lampbutton_Livingroom_001";
        toolTip.GetComponent<ToolTipConnector>().Target = Lampbutton_Livingroom_001;
        listSensorData.Add(new SensorData(Lampbutton_Livingroom_001, toolTip, "a78a81027d6e4fa5980ec51d32598212", false));
        
    }
    
    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 1.0f;
            string url = webServiceUrl + "?ts=" + time;
            WWW request = new WWW(url);
            StartCoroutine(OnResponse(request));
        }
    }
    
    public IEnumerator OnResponse(WWW req) {
        yield return req;
        Processjson(req.text);
    }

    [System.Serializable]
    private class SensorData {
        public GameObject sensorObject;
        public GameObject sensorToolTip;
        public string resource;
        public bool sample;
        public string db_time_stamp;

        public SensorData() { }

        public SensorData(GameObject go, GameObject toolTip, string s, bool b) {
            sensorObject = go;
            sensorToolTip = toolTip;
            resource = s;
            sample = b;
        }
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

            data.db_time_stamp = data.db_time_stamp.Replace("T", " ");

            
            time = data.db_time_stamp.Remove(19, data.db_time_stamp.Length - 19);
            
            
            data.db_time_stamp = time;

            Debug.Log("Iteration: " + i);
            Debug.Log("Resource: " + data.resource);
            Debug.Log("Sample: " + data.sample);
            Debug.Log("Time: " + data.db_time_stamp);

            foreach (SensorData sensorData in listSensorData) {
                string s = sensorData.resource;
                if (data.resource != null) {
                    if (data.resource.Equals(s)) {
                        sensorData.sample = data.sample;
                        sensorData.db_time_stamp = data.db_time_stamp;

                        if (sensorData.sample) {
                            sensorData.sensorObject.GetComponent<Renderer>().material = lampOn;
                            if (sensorData.resource.Equals("a78a81027d6e4fa5980ec51d32598212")) {
                                Light_table.SetActive(true);
                            }
                        }
                        else {
                            sensorData.sensorObject.GetComponent<Renderer>().material = lampOff;
                            if (sensorData.resource.Equals("a78a81027d6e4fa5980ec51d32598212")) {
                                Light_table.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
}
