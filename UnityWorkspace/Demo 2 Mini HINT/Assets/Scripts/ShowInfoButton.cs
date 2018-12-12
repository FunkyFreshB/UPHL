using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfoButton : MonoBehaviour, IInputClickHandler {

    public GameObject HINTModel;

    public void OnInputClicked(InputClickedEventData eventData) {
        Debug.Log("Clicked Info button");
        /*List<SensorData> sensorDataList = HINTModel.GetComponent<SensorManager>().sensorDataList;
        foreach(SensorData sensor in sensorDataList) {
            sensor.sensorToolTip.SetActive(true);
            sensor.sensorToolTip.transform.position = new Vector3(sensor.sensorObject.transform.position.x, sensor.sensorObject.transform.position.y, sensor.sensorObject.transform.position.z);
            Debug.Log("sensor");
        }
        */
    }
}
