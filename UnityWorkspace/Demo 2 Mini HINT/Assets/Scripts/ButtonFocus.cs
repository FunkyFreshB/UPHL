    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ButtonFocus : MonoBehaviour, IFocusable {
    
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

    private void Start() {
        if (onFocusMaterial == null) onFocusMaterial = gameObject.GetComponent<Renderer>().material;
        if (offFocusMaterial == null) offFocusMaterial = gameObject.GetComponent<Renderer>().material;
    }
}
