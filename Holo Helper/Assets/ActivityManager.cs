using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kan nog kallas MenuManager istället
public class ActivityManager : MonoBehaviour {

    public GameObject buttonBase;
    public List<GameObject> activities = new List<GameObject>();
    private int noOfActivities;
    private int noOfPages;
    GameObject selectedObj = null;
    public GameObject storedObj;
    public int currentPage = 0;
    public int pressd = 0;

    /* ------------------------------------ */
    /* General Functions */

    // Use this for initialization
    void Start () {
        // keep this object for every scene
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {


        Debug.Log("so " + selectedObj);

        // display arrow keys if we have more than 1 page (more than 5 activities)
        if (noOfPages > 0)
        {
            storedObj.transform.GetChild(0).gameObject.SetActive(true);
            storedObj.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            storedObj.transform.GetChild(0).gameObject.SetActive(false);
            storedObj.transform.GetChild(1).gameObject.SetActive(false);
        }
    }


    /* ------------------------------------ */
    /* Button Functions */

    public void OnAdminButtonPress(InputClickedEventData eventData, GameObject button)
    {
        ButtonBehaviour bb = button.GetComponent<ButtonBehaviour>();

        pressd++;
        Debug.Log(pressd);

        if (bb.isCreate)
        {
            bb.CreateKeyboard(true);
        }
        else if (bb.isActivity)
        {
            if (GetSelectedObject() != null)
            {
                GetSelectedObject().GetComponent<Renderer>().material = bb.materials[0];
            }

            SetSelectedObject(bb.gazedAtObj);
        }
        else if (bb.isEdit)
        {
            bb.CreateKeyboard(false);
            //menus[3].SetActive(true);
            //menus[1].SetActive(false);
        }
        else if (bb.isDelete)
        {
            DeleteActivity(GetSelectedObject());
        }
        else if (bb.isReturn)
        {
            bb.storedActs.SetActive(false);
            bb.menus[0].SetActive(true);
            bb.menus[1].SetActive(false);
        }
        else if (bb.isPageRight)
        {
            if (currentPage < GetPageAmount())
            {
                currentPage++;
            }
            else
            {
                currentPage = 0;
            }
        }
        else if (bb.isPageLeft)
        {
            if (currentPage > 0)
            {
                currentPage--;
            }
            else
            {
                currentPage = GetPageAmount();
            }
        }
    }

    /* ------------------------------------ */
    /* ActivityManager Functions */

    public void SetupActivityButtons()
    {
        // läs in aktiviteter
        // för varje aktivitet, CreateActivity(aktvitetsnamn)

        // ska endast göras om ex. en FirstTime variabel = false. sätts till true efteråt
    }

    public GameObject CreateActivity(string name)
    {
        // ActivityContainer.Save(name);
        // lägg activity i en lista och gör koden som följer

        // setup buttons for each activity
        GameObject newActivity = Instantiate(buttonBase);
        // newActivity.activity = ActivityContainer.Load(name);
        newActivity.name = name;
        newActivity.GetComponentInChildren<TextMesh>().text = name;
        newActivity.transform.position = this.transform.position;
        newActivity.transform.rotation = this.transform.rotation;
        
        activities.Add(newActivity);

        // increase noOfActivities and also noOfPages if enough activities
        if (activities.Count > noOfActivities)
        {            
            if (noOfActivities % newActivity.GetComponent<ButtonBehaviour>().visibleActs == 0 && noOfActivities != 0)
            {
                noOfPages++;
                currentPage = noOfPages;
            }

            noOfActivities++;
            
            return newActivity;
        }
        else
        {
            return null;
        }
    }

    public bool DeleteActivity(GameObject button)
    {
        // börja med att radera en aktivitet
        // ta samtidigt bort aktivitet från en knapp.
        // om knappen inte har en aktivitet, radera knappen
        // minska sen antal aktiviteter (och sidor om så behövs)

        // GameObject act = button.GetActivity();
        // int index = act.GetActivityNumber():
        // activities.RemoveAt(index);
        activities.Remove(button);
        Destroy(button);

        Debug.Log("nOA: " + noOfActivities + ", a.C: " + activities.Count);

        if (activities.Count < noOfActivities)
        {
            noOfActivities--;

            if (noOfActivities % button.GetComponent<ButtonBehaviour>().visibleActs == 0 && noOfActivities != 0)
            {
                noOfPages--;
                currentPage = noOfPages;
            }

            return true;
        }
        else
        {
            Debug.Log("Removed NOTHING!?");
            return false;
        }
    }

    /** Return amount of activities we have. */
    public int GetActivityAmount()
    {
        return noOfActivities;
    }

    /** Return amount of pages we have. */
    public int GetPageAmount()
    {
        return noOfPages;
    }

    /** Return the currently selected activity. */
    public GameObject GetSelectedObject()
    {
        return selectedObj;
    }

    /** Set which object is currently the marked one. */
    public bool SetSelectedObject(GameObject so)
    {
        selectedObj = so;

        if (selectedObj == so)
        {
            return true;
        }
        else
        {
            Debug.Log("NULL OBJ");
            return false;
        }
    }

    /** Edit the name of an activity. */
    public void SetName(GameObject act, string name)
    {
        //selectedObj = act;
        Debug.Log("Prev: " + selectedObj.name + ", " + selectedObj.GetComponentInChildren<TextMesh>().text);
        selectedObj.name = name;
        selectedObj.GetComponentInChildren<TextMesh>().text = name;
        Debug.Log("Next: " + selectedObj.name + ", " + selectedObj.GetComponentInChildren<TextMesh>().text);
    }
}
