using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class LampButton : MonoBehaviour, IInputClickHandler, IFocusable {

    AudioSource audio;
    public Material onFocusMaterial;
    public Material offFocusMaterial;
    string setEventsUrl;
    Sensor sensor;

    public void OnFocusEnter() {
        offFocusMaterial = gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material = onFocusMaterial;
    }

    public void OnFocusExit() {
        if (gameObject.GetComponent<Renderer>().material.color == onFocusMaterial.color)
            gameObject.GetComponent<Renderer>().material = offFocusMaterial;
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        audio.Play();
        if(sensor.resource != null) {
            int state = sensor.sample ? 0 : 1;
            WWW request = new WWW(setEventsUrl + "?resource=" + sensor.resource + "&state=" + state);
            StartCoroutine(OnResponse(request));
        }
    }

    public IEnumerator OnResponse(WWW req) {
        yield return req;
    }

    // Use this for initialization
    void Start () {
        GameObject miniHINT = GameObject.Find("MiniHINT");
        audio = miniHINT.GetComponent<AudioSource>();
        sensor = GetComponent<Sensor>();
        setEventsUrl = miniHINT.GetComponent<SensorManager>().setEventsUrl;
        if (onFocusMaterial == null) onFocusMaterial = gameObject.GetComponent<Renderer>().material;
        if (offFocusMaterial == null) offFocusMaterial = gameObject.GetComponent<Renderer>().material;
    }
}
