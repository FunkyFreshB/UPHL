using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class LampBehaviour : MonoBehaviour, IInputClickHandler {

    [Tooltip("Lamp to turn on")]
    public Light lightSource;

    private Material lampOn;
    private Material lampOff;

    private static float COOLDOWN = 1.0f;
    private float timer = 0.0f;
    private bool isTurnedOn = false;

    void Start() {
        if (lightSource == null)
            throw new System.NullReferenceException();
        lampOn = new Material(Shader.Find("Specular"));
        lampOn.color = Color.green;
        lampOff = new Material(Shader.Find("Specular"));
        lampOff.color = Color.red;
        timer = COOLDOWN;
        //OnInputClicked(null);
    }

    void Update() {
        timer -= Time.deltaTime;
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        if (timer < 0) {
            timer = COOLDOWN;
            if (!isTurnedOn) {
                isTurnedOn = true;
                lightSource.enabled = true;
                gameObject.GetComponent<Renderer>().material = lampOn;
            }
            else {
                isTurnedOn = false;
                lightSource.enabled = false;
                gameObject.GetComponent<Renderer>().material = lampOff;
            }
        }
    }
}
