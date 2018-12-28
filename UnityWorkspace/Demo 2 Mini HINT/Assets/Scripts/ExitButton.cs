using HoloToolkit.Unity.InputModule;
using HoloToolkit.UX.ToolTips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour, IInputClickHandler, IFocusable {

    public Material onFocusMaterial;
    public Material offFocusMaterial;

    public void OnInputClicked(InputClickedEventData eventData) {
        Application.Quit();
    }

    public void OnFocusEnter() {
        gameObject.GetComponent<Renderer>().material = onFocusMaterial;
    }

    public void OnFocusExit() {
        gameObject.GetComponent<Renderer>().material = offFocusMaterial;
    }

}
