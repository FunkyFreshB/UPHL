using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class HoldToOpenMenu : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler {

    private bool isFocused;
    private bool isHolding;


    public void OnFocusEnter() {
        isFocused = true;
    }

    public void OnFocusExit() {
        isFocused = false;
    }

    public void OnInputDown(InputEventData eventData) {
        if(isFocused) {
            isHolding = true;
        }
    }

    public void OnInputUp(InputEventData eventData) {
        if(isHolding) {
            isHolding = false;
        }
    }

    public void OnSourceDetected(SourceStateEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnSourceLost(SourceStateEventData eventData) {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
