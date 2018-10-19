using UnityEngine;
using System.IO;

// kan nog kallas MenuManager istället
public class ActivityManager : MonoBehaviour {

    public ActivityContainer container;
    private Activity foundAct;
    private Instructions foundInstruction;
    public GameObject buttonBase;
    public GameObject storedObj;
    public GameObject storedObj2;
    public GameObject[] menus = new GameObject[5];
    public Material[] materials = new Material[2];
    private Vector3[] activityPos = new Vector3[5];
    private GameObject newActivity;
    private GameObject newInstruction;
    private GameObject storedAct;
    private bool firstTime = true;
    
    private int noOfActivities;
    private int noOfPages;
    public int currentPage = 0;

    public int noOfInstruction;
    public int noOfPagesInstruction;
    public int currentPageInstruction = 0;

    private GameObject selectedObj = null;
    public Instructions selectedInstruction;
    public Activity selectedAct;

    /* ------------------------------------ */
    /* General Functions */

    // Use this for initialization
    void Start ()
    {
        activityPos[0] = new Vector3(0, 0.10415f, 0);
        activityPos[1] = new Vector3(0, 0.0749f, 0);
        activityPos[2] = new Vector3(0, 0.04565f, 0);
        activityPos[3] = new Vector3(0, 0.0164f, 0);
        activityPos[4] = new Vector3(0, -0.01285f, 0);

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

    /** Show the arrow keys if we have more than 1 page. */
    public void UpdatePageAmountInstruction()
    {
        // display arrow keys if we have more than 1 page (more than 5 activities)
        if (noOfPagesInstruction > 0)
        {
            storedObj2.transform.GetChild(0).gameObject.SetActive(true);
            storedObj2.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            storedObj2.transform.GetChild(0).gameObject.SetActive(false);
            storedObj2.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    /** Decide which activities to display. Activities */
    public void ChangePage()
    {
        for(int h = 0; h < storedObj.transform.childCount; h++)
        {
            if(h >= 2)
            {
                if ((h - 2) < (currentPage * 5) || (h - 2) > (currentPage * 5 + 4))
                {
                    if (storedObj.transform.GetChild(h).gameObject != null)
                    {
                        storedObj.transform.GetChild(h).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (storedObj.transform.GetChild(h).gameObject != null)
                    {
                        storedObj.transform.GetChild(h).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    /** Decide which activities to display. Instructions */
    public void ChangePageInstruction()
    {
        for (int h = 0; h < storedObj2.transform.childCount; h++)
        {
            if (h >= 2)
            {
                if ((h - 2) < (currentPageInstruction * 5) || (h - 2) > (currentPageInstruction * 5 + 4))
                {
                    if (storedObj2.transform.GetChild(h).gameObject != null)
                    {
                        storedObj2.transform.GetChild(h).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (storedObj2.transform.GetChild(h).gameObject != null)
                    {
                        storedObj2.transform.GetChild(h).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    /** Update the position of all activities */
    public void UpdateActivityPosition()
    {
        int currentButt = 0;
        bool hasPassedSO = false;

        for (int h = 0; h < storedObj.transform.childCount; h++)
        {
            if (h >= 2)
            {
                //storedObj.transform.GetChild(h).gameObject.SetActive(true);

                if (storedObj.transform.GetChild(h).gameObject != selectedObj)
                {
                    if (!hasPassedSO)
                    {
                        storedObj.transform.GetChild(h).gameObject.transform.localPosition = activityPos[currentButt % 5];

                        if ((h - 2) >= (currentPage * 5) && (h - 2) <= (currentPage * 5 + 4))
                        {
                            storedObj.transform.GetChild(h).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        storedObj.transform.GetChild(h).gameObject.transform.localPosition = activityPos[currentButt % 5];

                        if ((h - 3) >= (currentPage * 5) && (h - 3) <= (currentPage * 5 + 4))
                        {
                            storedObj.transform.GetChild(h).gameObject.SetActive(true);
                        }
                    }

                    currentButt++;
                }
                else
                {
                    hasPassedSO = true;
                }
            }
        }
    }

    /** Update the position of all Instruction */
    public void UpdateActivityPositionInstruction()
    {
        int currentButt = 0;
        bool hasPassedSO = false;

        for (int h = 0; h < storedObj2.transform.childCount; h++)
        {
            if (h >= 2)
            {
                //storedObj.transform.GetChild(h).gameObject.SetActive(true);

                if (storedObj2.transform.GetChild(h).gameObject != selectedObj)
                {
                    if (!hasPassedSO)
                    {
                        storedObj2.transform.GetChild(h).gameObject.transform.localPosition = activityPos[currentButt % 5];

                        if ((h - 2) >= (currentPageInstruction * 5) && (h - 2) <= (currentPageInstruction * 5 + 4))
                        {
                            storedObj2.transform.GetChild(h).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        storedObj2.transform.GetChild(h).gameObject.transform.localPosition = activityPos[currentButt % 5];

                        if ((h - 3) >= (currentPageInstruction * 5) && (h - 3) <= (currentPageInstruction * 5 + 4))
                        {
                            storedObj2.transform.GetChild(h).gameObject.SetActive(true);
                        }
                    }

                    currentButt++;
                }
                else
                {
                    hasPassedSO = true;
                }
            }
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
        newActivity = Instantiate(buttonBase);
        newActivity.GetComponent<ButtonBehaviour>().connectedAct = foundAct;    //
        newActivity.name = name;
        newActivity.GetComponent<ButtonBehaviour>().actMan = this.gameObject;
        newActivity.GetComponentInChildren<TextMesh>().text = name;
        newActivity.transform.position = this.transform.position;
        newActivity.transform.rotation = this.transform.rotation;

        // increase noOfActivities and also noOfPages if enough activities
        if (container.activities.Count > noOfActivities)
        {
            if (noOfActivities % 5 == 0 && noOfActivities != 0)
            {
                noOfPages++;
                UpdatePageAmount();
            }

            currentPage = noOfPages;
            ChangePage();

            noOfActivities++;

            return newActivity;
        }
        else
        {
            return null;
        }
    }

    /** Create a button with an attached instruction. If no instruction exists, create new. Else, set button's name to that of instruction. */
    public GameObject CreateInstruction(string text, Instructions instruct)
    {
        if (instruct != null)
        {
            foundInstruction = instruct;
            text = instruct.instructionText;
        }
        else
        {
           selectedAct.instructions.Add(new Instructions(text,selectedAct.name));                                       //
           foundInstruction = selectedAct.instructions.Find(x => x.instructionText == text);     //
        }

        // setup buttons for each activity
        newInstruction = Instantiate(buttonBase);
        newInstruction.GetComponent<ButtonBehaviour>().connectedInstruction = foundInstruction;    //
        newInstruction.name = text;
        newInstruction.GetComponent<ButtonBehaviour>().actMan = this.gameObject;
        newInstruction.GetComponentInChildren<TextMesh>().text = text;
        newInstruction.transform.position = this.transform.position;
        newInstruction.transform.rotation = this.transform.rotation;

        // increase noOfActivities and also noOfPages if enough activities
        if (selectedAct.instructions.Count > noOfInstruction)
        {
            if (noOfInstruction % 5 == 0 && noOfInstruction != 0)
            {
                noOfPagesInstruction++;
                UpdatePageAmountInstruction();
            }

            currentPageInstruction = noOfPagesInstruction;
            ChangePageInstruction();

            noOfInstruction++;

            return newInstruction;
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

            if (noOfActivities % 5 == 0 && noOfActivities != 0)
            {
                noOfPages--;
                UpdatePageAmount();

                if (currentPage >= noOfPages)
                {
                    currentPage = noOfPages;
                    ChangePage();
                }
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

    /** Removes an instruction and its button. */
    public bool DeleteInstruction(GameObject button)
    {
        selectedAct.RemoveInstruction(button);

        if (selectedAct.instructions.Count < noOfInstruction)
        {
            Object.Destroy(button);

            noOfInstruction--;

            if (noOfInstruction % 5 == 0 && noOfInstruction != 0)
            {
                noOfPagesInstruction--;
                UpdatePageAmountInstruction();

                if (currentPageInstruction >= noOfPagesInstruction)
                {
                    currentPageInstruction = noOfPagesInstruction;
                    ChangePageInstruction();
                }
            }

           UpdateActivityPositionInstruction();

            return true;
        }
        else
        {
            Debug.Log("ERR: Nothing removed");
            return false;
        }
    }

    /** Remove all instruction button but not the instruction itself */
    public void DeleteInstructionButton()
    {
        for (int h = 2; h < storedObj2.transform.childCount; h++)
        {
            Object.Destroy(storedObj2.transform.GetChild(h).gameObject);
        }


    noOfInstruction = 0;
    noOfPagesInstruction = 0;
    currentPageInstruction = 0;

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

    /** Get which page we're currently on. */
    public int GetCurrentPage()
    {
        return currentPage;
    }

    /** Set which page we're currently on. */
    public void SetCurrentPage(int cP)
    {
        currentPage = cP;
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

    /** Get firstTime to see if initialization has been completed. */
    public bool GetFirstTime()
    {
        return firstTime;
    }

    /** Set firstTime to clarify that initialization is done. */
    public void SetFirstTime(bool status)
    {
        firstTime = status;
    }

    /** Edit the name of an activity. */
    public void SetName(string newName)
    {
        foundAct = container.activities.Find(x => x.name == selectedAct.name);
        foundAct.name = selectedObj.name = newName;
        selectedObj.GetComponentInChildren<TextMesh>().text = newName;
        foundAct.setName(newName);
        selectedAct = container.activities.Find(x => x.name == newName);
    }

    /** Edit A Instruction. */
    public void SetNameInstruction(string newText)
    {
        selectedInstruction.setInstructionName(newText, selectedAct.name);
        selectedObj.name = selectedInstruction.instructionText;
        selectedObj.GetComponentInChildren<TextMesh>().text = newText;
    }

    public Vector3[] GetActivityPos()
    {
        return activityPos;
    }
}
