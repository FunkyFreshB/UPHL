using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SensorClick : MonoBehaviour, IInputClickHandler {

    bool isTurnedOn = false;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isTurnedOn)
        {
            SendMessageUpwards("activate", SendMessageOptions.DontRequireReceiver);
            isTurnedOn = true;
        }
        else
        {
            SendMessageUpwards("deactivate", SendMessageOptions.DontRequireReceiver);
            isTurnedOn = false;
        }
    }
}
