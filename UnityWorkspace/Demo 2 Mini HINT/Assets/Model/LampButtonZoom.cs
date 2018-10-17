using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class LampButtonZoom : MonoBehaviour , IFocusable{

    bool focus = false;
    Vector3 size;

    void start(){
        size = gameObject.transform.localScale;
    }

    public void OnFocusEnter(){
        this.transform.localScale = new Vector3(size.x * 3f, size.y * 3f, size.z);
    }

    public void OnFocusExit(){
        this.transform.localScale = size;
    }
}
