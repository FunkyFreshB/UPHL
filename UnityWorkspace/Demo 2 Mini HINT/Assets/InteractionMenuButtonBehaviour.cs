using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class InteractionMenuButtonBehaviour : MonoBehaviour, IFocusable, IInputClickHandler {

    private Material activeMaterial;
    private Material cachedMaterial;

    public GameObject model;
    public GameObject buttonMove;
    public GameObject buttonRotate;
    public GameObject buttonResize;
    public GameObject buttonWalls;

    public bool modeActive = true;

    // Use this for initialization
    void Start () {
        activeMaterial = new Material(GetComponent<Renderer>().material.shader);
        activeMaterial.color = Color.blue;
        cachedMaterial = GetComponent<Renderer>().material;
    }
	
    public void activate() {
        GazeManager.Instance.HitObject.GetComponent<Renderer>().material = activeMaterial;
    }

    public void deactivate() {
        GazeManager.Instance.HitObject.GetComponent<Renderer>().material = cachedMaterial;
        
    }

    public void OnFocusEnter() {
        this.activate();
    }

    public void OnFocusExit() {
        this.deactivate();
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        GameObject focusedObject = GazeManager.Instance.HitObject;
        if(focusedObject == buttonMove) {
            model.GetComponent<HanddraggableHH>().changeStatus();
        } 
        else if(focusedObject == buttonRotate) {

        }
        else if(focusedObject == buttonResize) {

        }
        else if((focusedObject == buttonWalls)) {

        }
        
    }
}
