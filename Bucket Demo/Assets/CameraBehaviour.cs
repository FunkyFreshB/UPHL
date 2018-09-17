using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Input;

public class CameraBehaviour : MonoBehaviour {

    bool holding = false;
    GameObject fo = null;
    float dist = 0;
    public Material mat1;
    public Material mat2;
    Vector3 pos;
    public GameObject cursor;
    public Vector3 dragPos;

    GestureRecognizer recognizer;
    SpatialMappingRenderer renderer;

    private void Awake()
    {
        recognizer = new GestureRecognizer();

        /*recognizer.Tapped += (args) =>{
            Tapping();
        };
        */
        recognizer.HoldStarted += (args) => {
            if (fo != null)
            {
                fo.SendMessageUpwards("OnHoldStart");
            }
           // HoldStart();
        };
        recognizer.HoldCompleted += (args) => {
            if (fo != null)
            {
                fo.SendMessageUpwards("OnHoldCompleted");
            }
            //HoldEnd();
        };
        recognizer.HoldCanceled += (args) => {
            if (fo != null)
            {
                fo.SendMessageUpwards("OnHoldCompleted");
            }
           // HoldStart();
        };

        recognizer.StartCapturingGestures();
    }

    // Use this for initialization
    void Start () {
        renderer = this.GetComponent<SpatialMappingRenderer>();
        renderer.visualMaterial = mat1;
    }

    void HoldStart()
    {
        Debug.Log("H_START");

        Vector3 mp = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, mp, out hit))
        {
            Debug.DrawRay(transform.position, mp, Color.red);
            if (hit.collider.tag == "Handle")
            {
                fo = hit.collider.gameObject;
                dist = Vector3.Distance(transform.position, hit.point);
                holding = true;
            }
        }

        renderer.visualMaterial = mat2;
    }
    void HoldEnd()
    {
        Debug.Log("H_END");
        
        holding = false;

        renderer.visualMaterial = mat1;

        Chang();
    }

    void Chang()
    {
        recognizer.CancelGestures();
        recognizer.StartCapturingGestures();
    }

    void Tapping()
    {
        Vector3 mp = transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, mp, out hit))
        {
            Debug.DrawRay(transform.position, mp, Color.red);
            if (hit.collider.tag == "Handle")
            {
                fo = hit.collider.gameObject;
                dist = Vector3.Distance(transform.position, hit.point);
                holding = !holding;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //MouseDebug();
        Debug.Log("HOLDING: " + holding);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward*10, out hit))
        {
            if (holding)
            {
                dragPos = transform.position + transform.forward * dist;
                fo.GetComponent<Rigidbody>().useGravity = false;
                // fo.GetComponentInChildren<Rigidbody>().useGravity = false;
            }
        }
        if (!holding) { 
            cursor.transform.position = hit.point;
            if (fo != null)
            {
                fo.GetComponent<Rigidbody>().useGravity = true;
                //  fo.GetComponentInChildren<Rigidbody>().useGravity = true;
            }
            fo = null;
        }
    }
    void MouseDebug()
    {
        Vector3 mp = Input.mousePosition; mp.z = 100; mp = Camera.main.ScreenToWorldPoint(mp);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, mp, out hit))
        {
            Debug.DrawRay(transform.position, mp, Color.green);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag == "Handle")
                {
                    Debug.Log("HIT");
                    fo = hit.collider.gameObject;
                    Debug.Log(fo.name);
                    dist = Vector3.Distance(transform.position, hit.point);
                    holding = !holding;
                }
            }
        }
    }
}

