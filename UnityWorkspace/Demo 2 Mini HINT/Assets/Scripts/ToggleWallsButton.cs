using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWallsButton : MonoBehaviour, IInputClickHandler, IFocusable {

    public GameObject walls;
    public Material onFocusMaterial;
    public Material offFocusMaterial;

    public void OnInputClicked(InputClickedEventData eventData) {
        if (walls.activeSelf) {
            walls.SetActive(false);
        }
        else {
            walls.SetActive(true);
        }
    }

    public void OnFocusEnter() {
        gameObject.GetComponent<Renderer>().material = onFocusMaterial;
    }

    public void OnFocusExit() {
        gameObject.GetComponent<Renderer>().material = offFocusMaterial;
    }

}
