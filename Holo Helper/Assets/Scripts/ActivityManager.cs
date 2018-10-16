using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

// kan nog kallas MenuManager istället
public class ActivityManager : MonoBehaviour {

    public ActivityContainer container;
    private Activity foundAct;
    public GameObject buttonBase;
    public GameObject storedObj;
    
    private int noOfActivities;
    private int noOfPages;
    public int currentPage = 0;

    private GameObject selectedObj = null;
    private Activity selectedAct;

    /* ------------------------------------ */
    /* General Functions */

    // Use this for initialization
    void Start () {
        // keep this object for every scene
        DontDestroyOnLoad(this);

        container = null;
      container = ActivityContainer.Load(Path.Combine(Application.dataPath, "ActivityList.xml"));

        if(container == null)
        {
            container = new ActivityContainer();
        }
       // container.CreateActivity("FirstOne");
       // container.Save(Path.Combine(Application.dataPath, "ActivityList.xml"));

        foreach (Activity a in container.activities)
        {
            a.reInitializer();
            GetComponent<ButtonBehaviour>().InstantiateActivityButton(a.name, a);
        }

       // container.Save(Path.Combine(Application.dataPath, "ActivityList.xml"));
    }
	
	// Update is called once per frame
	void Update () {

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
    /* ActivityManager Functions */

    public void ChangePage()
    {
        foreach(GameObject b in GameObject.FindGameObjectsWithTag("ActivityButton"))
        {
            b.SetActive(false);

            for (int i = 0; i < 5; i++)
            {
                if (b.GetComponent<ButtonBehaviour>().connectedAct == container.activities[currentPage * 5 + i])
                {
                    b.SetActive(true);
                }
            }
        }


        // for each activity
        // SetActive(false);
        // activities[page*5+0].SetActive(true);
        // activities[page*5+1].SetActive(true);
        // activities[page*5+2].SetActive(true);
        // activities[page*5+3].SetActive(true);
        // activities[page*5+4].SetActive(true);
    }

    public GameObject CreateActivity(string name, Activity act)
    {
        if (act != null)
        {
            foundAct = act;
            name = act.name;
        }
        else
        {
            container.CreateActivity(name);                                         //
            foundAct = container.activities.Find(x => x.name == name);     //
        }

        // setup buttons for each activity
        GameObject newActivity = Instantiate(buttonBase);
        newActivity.GetComponent<ButtonBehaviour>().connectedAct = foundAct;    //
        newActivity.name = name;
        newActivity.GetComponent<ButtonBehaviour>().actMan = this.gameObject;
        newActivity.GetComponentInChildren<TextMesh>().text = name;
        newActivity.transform.position = this.transform.position;
        newActivity.transform.rotation = this.transform.rotation;

        // increase noOfActivities and also noOfPages if enough activities
        if (container.activities.Count > noOfActivities)
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

        Debug.Log("nOA: " + noOfActivities + ", a.C: " + container.activities.Count);

        if (container.activities.Count < noOfActivities)
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
            Debug.Log(selectedObj);
            return true;
        }
        else
        {
            Debug.Log("NULL OBJ");
            return false;
        }
    }

    /** Edit the name of an activity. */
    public void SetName(string name)
    {
        selectedObj.name = name;
        selectedObj.GetComponentInChildren<TextMesh>().text = name;
    }
}
