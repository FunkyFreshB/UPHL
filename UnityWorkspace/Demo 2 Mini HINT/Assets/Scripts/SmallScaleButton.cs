using HoloToolkit.Unity.InputModule;
using HoloToolkit.UX.ToolTips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallScaleButton : MonoBehaviour, IInputClickHandler, IFocusable {

    public GameObject HINTModel;
    private float OriginalTransformY;
    public Material onFocusMaterial;
    public Material offFocusMaterial;

    public void OnInputClicked(InputClickedEventData eventData) {
        gameObject.GetComponent<Renderer>().material = offFocusMaterial;
        HINTModel.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        HINTModel.transform.position = new Vector3(HINTModel.transform.position.x, OriginalTransformY, HINTModel.transform.position.z);
        HINTModel.GetComponent<SensorManager>().isLarge = false;
        gameObject.transform.parent.gameObject.SetActive(false);
    }
    
    public void OnFocusEnter() {
        gameObject.GetComponent<Renderer>().material = onFocusMaterial;
    }

    public void OnFocusExit() {
        gameObject.GetComponent<Renderer>().material = offFocusMaterial;
    }

    private void Start() {
        OriginalTransformY = HINTModel.transform.position.y;
        if (onFocusMaterial == null) onFocusMaterial = gameObject.GetComponent<Renderer>().material;
        if (offFocusMaterial == null) offFocusMaterial = gameObject.GetComponent<Renderer>().material;
    }
}