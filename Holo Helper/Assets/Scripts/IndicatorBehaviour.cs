using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class IndicatorBehaviour : MonoBehaviour,IInputClickHandler{
    public Instructions instruction { get; set; }
    public bool what;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        
    }

    public void Start()
    {
        what = true;
    }

    public void Update()
    {
        if (what)
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(50, 50, 50);
            what = !what;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
            what = !what;
        }
        
    }

}
