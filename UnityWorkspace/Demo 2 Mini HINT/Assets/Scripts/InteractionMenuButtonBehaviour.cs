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

    public GameObject lastFocusObject;
    public GameObject activeButton;
    public bool isButtonActive = false;

    public bool modeActive = true;

    // Use this for initialization
    void Start () {
        activeMaterial = new Material(Shader.Find("Specular"));
        activeMaterial.color = Color.blue;
        cachedMaterial = buttonMove.GetComponent<Renderer>().material;
    }
	
    public void activate() {
        if(activeButton != lastFocusObject)
            lastFocusObject.GetComponent<Renderer>().material = activeMaterial;
    }

    public void deactivate() {
        if (activeButton != lastFocusObject)
            lastFocusObject.GetComponent<Renderer>().material = cachedMaterial;

        
    }

    public void OnFocusEnter() {
        lastFocusObject = GazeManager.Instance.HitObject;
        this.activate();
    }

    public void OnFocusExit() {
        this.deactivate();
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        GameObject focusedObject = GazeManager.Instance.HitObject;
        if(focusedObject == buttonMove) {
            if (buttonMove == activeButton) {
                activeButton = null;
                isButtonActive = false;
                buttonMove.GetComponent<Renderer>().material = cachedMaterial;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
            else{
                if(activeButton!=null)
                    activeButton.GetComponent<Renderer>().material = cachedMaterial;
                activeButton = focusedObject;
                isButtonActive = true;
                buttonMove.GetComponent<Renderer>().material = activeMaterial;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
        } 
        else if(focusedObject == buttonRotate) {
            if (buttonRotate == activeButton) {
                activeButton = null;
                isButtonActive = false;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
            else {
                if (activeButton != null)
                    activeButton.GetComponent<Renderer>().material = cachedMaterial;
                activeButton = focusedObject;
                isButtonActive = true;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
        }
        else if(focusedObject == buttonResize) {
            if (buttonResize == activeButton) {
                activeButton = null;
                isButtonActive = false;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
            else {
                if (activeButton != null)
                    activeButton.GetComponent<Renderer>().material = cachedMaterial;
                activeButton = focusedObject;
                isButtonActive = true;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
        }
        else if((focusedObject == buttonWalls)) {
            if (buttonWalls == activeButton) {
                activeButton = null;
                isButtonActive = false;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
            else {
                if (activeButton != null)
                    activeButton.GetComponent<Renderer>().material = cachedMaterial;
                activeButton = focusedObject;
                isButtonActive = true;
                model.GetComponent<HanddraggableHH>().changeStatus();
            }
        }
        
    }
}
