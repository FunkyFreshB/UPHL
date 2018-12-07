using HoloToolkit.Unity.InputModule;
using HoloToolkit.UX.ToolTips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallScaleButton : MonoBehaviour, IInputClickHandler {

    public GameObject HINTModel;
    private float OriginalTransformY;

    public void OnInputClicked(InputClickedEventData eventData) {
        HINTModel.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        HINTModel.transform.position = new Vector3(HINTModel.transform.position.x, OriginalTransformY, HINTModel.transform.position.z);
        //gameObject.GetComponentInParent<GameObject>().gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void Start() {
        OriginalTransformY = HINTModel.transform.position.y;
    }
}