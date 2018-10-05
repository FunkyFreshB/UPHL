using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.Input;

public class ButtonBehaviour : MonoBehaviour, IInputClickHandler, IFocusable {

    public GameObject actMan;
    public GameObject[] menus = new GameObject[5];
    public Material[] materials = new Material[2];
    GameObject obj;
    int activityNo = 1;

    public bool isAdmin;
    public bool isActivity = false;
    public bool isEdit = false;
    public bool isCreate = false;
    public bool isDelete = false;
    public bool isReturn = false;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Unhighlight the highlighted button
        this.gameObject.GetComponent<Renderer>().material = materials[0];

        // 0: Main Menu
        if (menus[0].activeSelf)
        {
            if (isAdmin)
            {
                menus[1].SetActive(true);
                menus[0].SetActive(false);
            }
            else
            {
                menus[2].SetActive(true);
                menus[0].SetActive(false);
            }
        }

        // ---------------------------------------------

        // 1: Administrator Menu
        else if (menus[1].activeSelf)
        {
            if (isAdmin && isCreate)
            {
                // TANGENTBORD HÄR
                obj = GetComponentInParent<ActivityManager>().CreateActivity("name " + activityNo);
                obj.transform.SetParent(actMan.transform);
                activityNo++;
            }
            if (isAdmin && isEdit)
            {
                // VAD SKA GÖRAS HÄR
            }
            if (isAdmin && isDelete)
            {
                menus[3].SetActive(true);
                menus[1].SetActive(false);
            }
            if (isReturn)
            {
                menus[0].SetActive(true);
                menus[1].SetActive(false);
            }
        }

        // ---------------------------------------------

        // 2: Activity Menu
        else if (menus[2].activeSelf)
        {
            if (isActivity)
            {
                GetComponentInParent<ActivityManager>().activities.IndexOf(23);
                GetComponentInChildren<TextMesh>().text = obj.name;
            }
        }

        // ---------------------------------------------

        // 3: Delete Menu
        else if (menus[3].activeSelf)
        {

        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnFocusEnter()
    {
        this.gameObject.GetComponent<Renderer>().material = materials[1];
    }

    public void OnFocusExit()
    {
        this.gameObject.GetComponent<Renderer>().material = materials[0];
    }
}
