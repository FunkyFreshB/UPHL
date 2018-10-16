using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class LampButtonClick : MonoBehaviour, IInputClickHandler {

    bool isTurnedOn = false;
    public void OnInputClicked(InputClickedEventData eventData) {
        if (!isTurnedOn) {
            SendMessageUpwards("LightOn", SendMessageOptions.DontRequireReceiver);
            isTurnedOn = true;
        } else {
            SendMessageUpwards("LightOff", SendMessageOptions.DontRequireReceiver);
            isTurnedOn = false;
        }
    }
}
