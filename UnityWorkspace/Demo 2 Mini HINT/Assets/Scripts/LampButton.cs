using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class LampButton : MonoBehaviour, IInputClickHandler, IFocusable {

    AudioSource audio;
    public Material onFocusMaterial;
    public Material offFocusMaterial;

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
        Debug.Log(gameObject.name);
    }

    // Use this for initialization
    void Start () {
        audio = GameObject.Find("MiniHINT").GetComponent<AudioSource>();
        if (onFocusMaterial == null) onFocusMaterial = gameObject.GetComponent<Renderer>().material;
        if (offFocusMaterial == null) offFocusMaterial = gameObject.GetComponent<Renderer>().material;
    }
}
