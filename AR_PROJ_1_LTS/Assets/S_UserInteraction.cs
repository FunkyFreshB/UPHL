using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.UI;

public class S_UserInteraction : MonoBehaviour {

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
    public Text destroyTXT;
    int destroyTOT = 0;
    int destroyablesTOT = 0;

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
            HoldActions();
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
            }
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
            }
        }
    }

    void HoldActions()
    {
        Vector3 mp = Input.mousePosition; mp.z = 10; mp = Camera.main.ScreenToWorldPoint(mp);
        mp = transform.forward;

        RaycastHit hit;
    }
	
	// Update is called once per frame
	void Update () {
        MouseDebug();

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
            meshr.enabled = true;
        }
        else
        {
            meshr.enabled = false;
        }
    }

    void MouseDebug()
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
                }
                if (hit.collider.tag == "Draggable")
                {
                    audioData.pitch = Random.Range(0.5f, 1.2f);
                    audioData.Play(0);
                    Debug.Log("HIT");

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
                else
                {
                    draggin = false;
                }
            }
        }
    }
}
