using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:UnityWorkspace/Demo 2 Mini HINT/Assets/HoldToOpenMenu.cs
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
=======
public class Move_Rotate_Scale : MonoBehaviour {

	// Use this for initialization
	void Start () {
>>>>>>> 8a0bf37e49ebbb2680bb7af4e7701ebd028a9283:UnityWorkspace/Demo 2 Mini HINT/Assets/Move_Rotate_Scale.cs
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
