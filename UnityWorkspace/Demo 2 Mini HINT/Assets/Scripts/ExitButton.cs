using HoloToolkit.Unity.InputModule;
using HoloToolkit.UX.ToolTips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour, IInputClickHandler {

    public void OnInputClicked(InputClickedEventData eventData) {
        Application.Quit();
    }
}
