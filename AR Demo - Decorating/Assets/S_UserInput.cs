using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Input;

public class S_UserInput : MonoBehaviour {

    GestureRecognizer recognizer;

    public GameObject fo;
    public GameObject co;
    public GameObject marker;

    private void Awake()
    {

        SpatialMappingRenderer renderer = this.GetComponent<SpatialMappingRenderer>();

        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);

        recognizer.Tapped += (args) =>
        {

            Vector3 gaze = transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, gaze, out hit))
            {
                while(true){
                    if (hit.collider.tag == "Menu" ||
                        hit.collider.tag == "Menu_Main")
                    {
                        co = hit.collider.gameObject;
                        Debug.Log("co");
                        break;
                    }
                    else if (hit.collider.tag == "Interactable")
                    {
                        co = hit.collider.gameObject;
                        fo = hit.collider.gameObject;
                        Debug.Log("fo");
                        break;
                    }
                    else
                    {
                        Destroy(fo.GetComponent<S_SelectedObject>().menuClone);
                        co = null;
                        fo = null;
                        break;
                    }
                }
            }

            if (co != null)
            {
                co.SendMessageUpwards("OnTapped");
            }
            else
            {
            }
        };

        recognizer.StartCapturingGestures();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 gaze = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, gaze, out hit))
        {
            marker.transform.position = hit.point;
        }
    }
}
