using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.UI;

public class S_UserInteraction : MonoBehaviour {

    bool sw = true;
    public GameObject curse;
    private MeshRenderer meshr;

    public GameObject obj;
    GameObject focussedObj;
    float hitObjDist;

    AudioSource audioData;

    bool draggin = false;

    public static S_UserInteraction Instance { get; private set; }
    GestureRecognizer recognizer;

    // UI
    public TextMesh destroyTXT;
    int destroyTOT = 0;
    int destroyablesTOT = 0;
    public TextMesh placedTXT;
    int placedTOT = 0;
    public GameObject UIobj;

    private void Awake()
    {
        meshr = curse.GetComponent<MeshRenderer>();

        Instance = this;

        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            TapActions();
        };

        recognizer.HoldStarted += (args) =>
        {
            HoldStartActions();
        };

        recognizer.HoldCompleted += (args) =>
        {
            HoldEndActions();
        };

        recognizer.HoldCanceled += (args) =>
        {
            HoldEndActions();
        };

        recognizer.StartCapturingGestures();
    }

    // Use this for initialization
    void Start ()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Stop();

        foreach(GameObject ds in GameObject.FindGameObjectsWithTag("Destroyable"))
        {
            destroyablesTOT++;
        }
    }

    void TapActions()
    {
        Vector3 mp = transform.forward;

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, mp, out hit))
        {
            if (hit.collider.tag == "Destroyable")
            {
                audioData.pitch = Random.Range(0.5f, 1.2f);
                audioData.Play(0);
                Destroy(hit.collider.gameObject);
                destroyTOT++;
            }
            if (hit.collider.tag == "Area" || hit.collider.tag == "Untagged")
            {
                audioData.pitch = Random.Range(0.5f, 1.2f);
                audioData.Play(0);
                Instantiate(obj, hit.point, Quaternion.Euler(90, 0, 0));
                placedTOT++;
            }/*
            if (hit.collider.tag == "Draggable")
            {
                audioData.pitch = Random.Range(0.5f, 1.2f);
                audioData.Play(0);

                if (draggin)
                {
                    draggin = false;
                }
                else
                {
                    focussedObj = hit.collider.gameObject;
                    hitObjDist = Vector3.Distance(transform.position, focussedObj.transform.position);
                    draggin = true;
                }
            }
            else if(hit.collider.tag != "Draggable")
            {
                draggin = false;
            }*/
        }
    }

    void HoldStartActions()
    {
        Vector3 mp = Input.mousePosition; mp.z = 10; mp = Camera.main.ScreenToWorldPoint(mp);
        mp = transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, mp, out hit))
        {
            if (hit.collider.tag == "Draggable")
            {
                focussedObj = hit.collider.gameObject;
                hitObjDist = Vector3.Distance(transform.position, focussedObj.transform.position);
                draggin = true;
            }
        }
    }

    void HoldEndActions()
    {
        draggin = false;
    }

    // Update is called once per frame
    void Update () {
        MouseKeyboardDebug();
        TextStuff();

        Vector3 mp = Input.mousePosition; mp.z = 10; mp = Camera.main.ScreenToWorldPoint(mp);
        mp = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, mp, out hit))
        {
            if (draggin)
            {
                float g = focussedObj.transform.position.z;
                focussedObj.transform.position =
                    transform.position + transform.forward * hitObjDist;
            }

            curse.transform.position = hit.point;
            curse.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            meshr.enabled = true;
        }
        else
        {
            meshr.enabled = false;
        }
    }

    void TextStuff()
    {
       // UIobj.transform.LookAt(this.transform);
        destroyTXT.text = destroyTOT + " DESTROYED";
        placedTXT.text = placedTOT + " PLACED";
    }

    void MouseKeyboardDebug()
    {
        Vector3 mp2 = Input.mousePosition; mp2.z = 10; mp2 = Camera.main.ScreenToWorldPoint(mp2);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, mp2, out hit))
            {
                if (hit.collider.tag == "Destroyable")
                {
                    audioData.pitch = Random.Range(0.5f, 1.2f);
                    audioData.Play(0);
                    Destroy(hit.collider.gameObject);
                    destroyTOT++;
                }
                if (hit.collider.tag == "Area" || hit.collider.tag == "Untagged")
                {
                    audioData.pitch = Random.Range(0.5f, 1.2f);
                    audioData.Play(0);
                    Instantiate(obj, hit.point, Quaternion.Euler(90, 0, 0));
                    placedTOT++;
                }
                if (hit.collider.tag == "Draggable")
                {
                    audioData.pitch = Random.Range(0.5f, 1.2f);
                    audioData.Play(0);

                    focussedObj = hit.collider.gameObject;
                    hitObjDist = Vector3.Distance(transform.position, focussedObj.transform.position);
                    draggin = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            draggin = false;
        }
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.velocity = transform.TransformDirection(new Vector3(0,0,Input.GetAxis("Vertical")*2));
        if (Input.GetKeyDown(KeyCode.E))
        {
            sw = !sw;
            Debug.Log(sw);
        }

        if (sw)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);
        }
        else
        {
            transform.Rotate(Input.GetAxis("Horizontal") * 3, 0, 0);
        }
    }
}
