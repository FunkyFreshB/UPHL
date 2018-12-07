using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWallsButtonPress : MonoBehaviour, IInputClickHandler {

    public GameObject walls;

    public void OnInputClicked(InputClickedEventData eventData) {
        if (walls.activeSelf) {
            walls.SetActive(false);
        }
        else {
            walls.SetActive(true);
        }
    }
}
