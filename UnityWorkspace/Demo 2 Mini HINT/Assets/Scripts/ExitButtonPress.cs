using HoloToolkit.Unity.InputModule;
using HoloToolkit.UX.ToolTips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonPress : MonoBehaviour, IInputClickHandler {

    public void OnInputClicked(InputClickedEventData eventData) {
        Application.Quit();
    }

    private void Start() {
        gameObject.transform.GetChild(1).gameObject.GetComponent<ToolTip>().ToolTipText = "Yahallo!";
    }
}
