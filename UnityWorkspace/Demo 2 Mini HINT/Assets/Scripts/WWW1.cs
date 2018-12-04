using System.Collections;
using System.Collections.Generic;
//using SimpleJSON;
//using JsonUtility;
using UnityEngine;


public class WWW1 : MonoBehaviour {

    //public string url = "https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1GJzk?ver=8b59&q=60&m=6&h=600&w=2048&b=%23FFFFFFFF&l=f&o=t&aim=true";
    //public string url2 = "http://192.168.1.168:8080/DatabaseConnectServiceHololens/rest/UserInfoService/run/YourName";
    WWW request;
    public string webServiceUrl = "http://192.168.1.195:8080/getevents.py?ts="; //2018-12-03 18:29:33
    bool isDone = false;
    public float timer = 2f;
    public SensorData data;

    private void Start() {
        //request = new WWW(webServiceUrl);
    }

    public IEnumerator OnResponse(WWW req) {
        yield return req;
        //Debug.Log(req.text);
        Processjson(request.text);
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 3f;
            request = new WWW(webServiceUrl + data.db_time_stamp);
            StartCoroutine(OnResponse(request));

            /*
            if (request.error == null) {
                //Processjson(request.text);
                //Debug.Log(request.text);
            }
            else {
                Debug.Log("ERROR: " + request.error);
            }*/
        }
    }

    [System.Serializable]
    public class SensorData {
        public string resource;
        public string sample;
        public string db_time_stamp;
    }

    private void Processjson(string jsonString) {
        
        jsonString = jsonString.Replace("'", "\"");
        jsonString = jsonString.Remove(0, 1);
        jsonString = jsonString.Replace(",)", "");

        string jsonData = "";
        /*if(string.IsNullOrEmpty(request.error)) {
            jsonData = System.Text.Encoding.UTF8.GetString(request.bytes, 3, request.bytes.Length - 3);
        }*/
        //Debug.Log(jsonString);
        //Debug.Log(request.bytes);
        data = JsonUtility.FromJson<SensorData>(jsonString);

        data.db_time_stamp = data.db_time_stamp.Replace("T"," ");
        int i = data.db_time_stamp.IndexOf(".");
        data.db_time_stamp = data.db_time_stamp.Remove(i, 13);


        Debug.Log(data.resource);
        Debug.Log(data.sample);
        Debug.Log(data.db_time_stamp);
    }
}
