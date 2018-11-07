using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class Menu : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler {

    private bool isFocused;
    private bool isHolding;
    private int tapCounter;
    private float doubleTapTimer;
    private static float DOUBLETAPTIME = 1.0f;

    private bool isMenuActive;
    [Tooltip("Menu to show")]
    [SerializeField]
    private GameObject menu;


    public void OnFocusEnter() {
        isFocused = true;
    }

    public void OnFocusExit() {
        isFocused = false;
    }

    public void OnInputDown(InputEventData eventData) {
        if (isFocused) {
            isHolding = true;
        }
    }

    public void OnInputUp(InputEventData eventData) {
        if (isHolding) {
            isHolding = false;
        }
        if (isFocused)
            tapCounter++;
    }

    public void OnSourceDetected(SourceStateEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnSourceLost(SourceStateEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void toggleMenu() {
        
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    // Use this for initialization
    void Start() {
        isFocused = false;
        isHolding = false;
        tapCounter = 0;
        doubleTapTimer = DOUBLETAPTIME;
    }

    void Update() {
        doubleTapTimer -= Time.deltaTime;
        if(doubleTapTimer > 0 && tapCounter > 1) {
            tapCounter = 0;
            doubleTapTimer = DOUBLETAPTIME;
            toggleMenu();
        }
        else if(doubleTapTimer <= 0) {
            doubleTapTimer = DOUBLETAPTIME;
            tapCounter = 0;
        }
    }
}
