using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class IndicatorOkOrCancel : MonoBehaviour, IInputClickHandler, IFocusable {

    // Use this for initialization

    public bool okOrCancel;
    public Material[] materials = new Material[2];

    public void OnFocusEnter()
    {
        this.gameObject.GetComponent<Renderer>().material = materials[1];
    }

    public void OnFocusExit()
    {
        this.gameObject.GetComponent<Renderer>().material = materials[0];
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (okOrCancel)
        {

        }

        else
        {

        }
    }

    public void Start()
    {
        
    }
}
