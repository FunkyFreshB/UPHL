using UnityEngine;
using System.IO;

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

        foreach (Activity a in container.activities)
        {
            a.reInitializer();
        }

        UpdatePageAmount();
    }
	
	// Update is called once per frame
	void Update () {

       // UpdateActivityPosition();
    }

    /* ------------------------------------ */
    /* ActivityManager Functions */

    /** Show the arrow keys if we have more than 1 page. */
    public void UpdatePageAmount()
    {
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

    /** Decide which activities to display. */
    public void ChangePage()
    {
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("ActivityButton"))
        {
            b.SetActive(false);
            
            for (int i = 0; i < 5; i++)
            {
                Debug.Log("b: " + b.GetComponent<ButtonBehaviour>().connectedAct + ", act: " + container.activities[currentPage * 5 + i]);
                /*
                if (b.GetComponent<ButtonBehaviour>().connectedAct == container.activities[currentPage * 5 + i])
                {
                    b.SetActive(true);
                    break;
                }*/
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

    /** Update the position of all activities */
    public void UpdateActivityPosition()
    {
        int current = 0;

        foreach (GameObject b in GameObject.FindGameObjectsWithTag("ActivityButton"))
        {
            b.transform.localPosition = b.GetComponent<ButtonBehaviour>().activityPos[current % 5];
            current++;
        }
    }

    /** Create a button with an attached activity. If no activity exists, create new. Else, set button's name to that of activity. */
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
                UpdatePageAmount();
                ChangePage();
            }

            noOfActivities++;

            return newActivity;
        }
        else
        {
            return null;
        }
    }

    /** Removes an activity and its button. */
    public bool DeleteActivity(GameObject button)
    {
        container.RemoveActivity(button);

        if (container.activities.Count < noOfActivities)
        {
            Object.Destroy(button);

            noOfActivities--;

            if (noOfActivities % button.GetComponent<ButtonBehaviour>().visibleActs == 0 && noOfActivities != 0)
            {
                noOfPages--;
                currentPage = noOfPages;
                UpdatePageAmount();
                ChangePage();
            }

            UpdateActivityPosition();

            return true;
        }
        else
        {
            Debug.Log("ERR: Nothing removed");
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
    public void SetName(string newName)
    {
        foundAct = container.activities.Find(x => x.name == selectedObj.name);
        foundAct.name = selectedObj.name = newName;
        selectedObj.GetComponentInChildren<TextMesh>().text = newName;
    }
}
