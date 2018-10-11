using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class IndicatorBehaviour : MonoBehaviour,IInputClickHandler {
    public Instructions instruction;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject.Find("Instructions").GetComponent<TextMesh>().text = instruction.getInstructionText();
    }

}
