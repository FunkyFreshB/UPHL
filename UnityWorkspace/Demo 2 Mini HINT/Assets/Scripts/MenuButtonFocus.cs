    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class MenuButtonFocus : MonoBehaviour, IFocusable {
    
    public Material onFocusMaterial;
    public Material offFocusMaterial;

    public void OnFocusEnter() {
        gameObject.GetComponent<Renderer>().material = onFocusMaterial;
    }

    public void OnFocusExit() {
        gameObject.GetComponent<Renderer>().material = offFocusMaterial;
    }
}
