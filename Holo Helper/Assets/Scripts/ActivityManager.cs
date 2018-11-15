using UnityEngine;
using System.IO;
using HoloToolkit.Unity;

// kan nog kallas MenuManager istället
public class ActivityManager : MonoBehaviour {

    private bool firstTime = true;                  // checks if first time setup has been done

    private GameObject selectedObj;                 // button we have selected by tapping on it
    private Activity selectedAct;                   // selectedObj's attached activity
    private Instructions selectedInstruction;       // selectedObj's attached instruction

    public ActivityContainer container;             // container of activities
    public GameObject buttonBase;                   // prefab for buttons
    public GameObject storedAct;                    // Stored Activities gameObject
    public GameObject storedIns;                    // Stored Instructions gameObject
    public GameObject menuBg;                       // background plane of menus

    public GameObject[] menus = new GameObject[5];  // array containing each menu gameObject
    public Material[] materials = new Material[3];  // array containing each material used for focusing on buttons
    private Vector3[] activityPos = new Vector3[5]; // array containing positions for placing buttons

    private int noOfActivities;                     // controls the number of activities
    private int noOfInstructions;                   // controls the number of instructions
    private int noOfPagesActivities;                // controls the number of pages
    private int noOfPagesInstructions;              // controls the number of pages
    private int currentPage = 0;                    // checks which page we're on

    private Activity foundAct;                      // activity found on a gameObject
    private GameObject newActivity;                 // used when creating an activity
    private Instructions foundInstruction;          // instruction found on a gameobject
    private GameObject newInstruction;              // used when creating an instruction

    /* ------------------------------------ */
    /* General Functions */
    /* ------------------------------------ */

    // Use this for initialization
    void Start ()
    {
        activityPos[0] = new Vector3(0, 0.10415f, 0);
        activityPos[1] = new Vector3(0, 0.0749f, 0);
        activityPos[2] = new Vector3(0, 0.04565f, 0);
        activityPos[3] = new Vector3(0, 0.0164f, 0);
        activityPos[4] = new Vector3(0, -0.01285f, 0);

        container = null;

        if (Application.isEditor)
        {
            container = ActivityContainer.Load(Path.Combine(Application.dataPath, "ActivityList.xml"));
        }
        else
        {
            container = ActivityContainer.Load(Path.Combine(Application.persistentDataPath, "ActivityList.xml"));
        }

        if(container == null)
        {
            container = new ActivityContainer();
        }

        foreach (Activity a in container.activities)
        {
            a.reInitializer();
        }

        UpdatePageAmount(storedAct);
    }
	
	// Update is called once per frame
	void Update () {
    }

    /* ------------------------------------ */
    /* ActivityManager Functions */
    /* ------------------------------------ */

    /** Create a button with an attached activity. If no activity exists, create new. Else, set button's name to that of activity. */
    public GameObject CreateActivity(string name, Activity act)
    {
        // if an activity is sent through, set that to foundAct and get its name
        if (act != null)
        {
            foundAct = act;
            name = act.name;
        }
        // else, create a new based on the sent through name
        else
        {
            container.CreateActivity(name);
            foundAct = container.activities.Find(x => x.name == name);
        }

        // setup buttons for each activity
        // create a new instance of a button and set its name
        newActivity = Instantiate(buttonBase);
        newActivity.name = name;
        if (name.Length > 20)
        {
            newActivity.GetComponentInChildren<TextMesh>().text = name.Remove(20) + "...";
        }
        else
        {
            newActivity.GetComponentInChildren<TextMesh>().text = name;
        }
        // assign variables to the button's script
        newActivity.GetComponent<ButtonBehaviour>().connectedAct = foundAct;
        newActivity.GetComponent<ButtonBehaviour>().actMan = this.gameObject;
        // set the buttons position and rotation to align with MenuPos
        newActivity.transform.position = this.transform.position;
        newActivity.transform.rotation = this.transform.rotation;

        // increase noOfActivities + noOfPages (if enough activities)
        if (container.activities.Count > noOfActivities)
        {
            if (noOfActivities % 5 == 0 && noOfActivities != 0)
            {
                noOfPagesActivities++;
                UpdatePageAmount(storedAct);
            }

            currentPage = noOfPagesActivities;

            // update which page we're on
            ChangePage(storedAct);

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
        // if an instruction is sent through, set that to foundInstruction and get its name
        if (instruct != null)
        {
            foundInstruction = instruct;
            text = instruct.instructionText;
        }
        // else, create a new based on the sent through name
        else
        {
           selectedAct.instructions.Add(new Instructions(text,selectedAct.name));
           foundInstruction = selectedAct.instructions.Find(x => x.instructionText == text);
        }

        // setup buttons for each activity
        // create a new instance of a button and set its name
        newInstruction = Instantiate(buttonBase);
        newInstruction.name = text;
        if (text.Length > 20)
        {
            newInstruction.GetComponentInChildren<TextMesh>().text = text.Remove(20) + "...";
        }
        else
        {
            newInstruction.GetComponentInChildren<TextMesh>().text = text;
        }
        // assign variables to the button's script
        newInstruction.GetComponent<ButtonBehaviour>().connectedInstruction = foundInstruction;
        newInstruction.GetComponent<ButtonBehaviour>().actMan = this.gameObject;
        // set the buttons position and rotation to align with MenuPos
        newInstruction.transform.position = this.transform.position;
        newInstruction.transform.rotation = this.transform.rotation;

        // increase noOfActivities and also noOfPages if enough activities
        if (selectedAct.instructions.Count > noOfInstructions)
        {
            if (noOfInstructions % 5 == 0 && noOfInstructions != 0)
            {
                noOfPagesInstructions++;
                UpdatePageAmount(storedIns);
            }

            currentPage = noOfPagesInstructions;

            // update which page we're on
            ChangePage(storedIns);

            noOfInstructions++;

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
        // remove from the saved list
        container.RemoveActivity(button);

        // if amount of activities is fewer than we had before (activity deleted)
        if (container.activities.Count < noOfActivities)
        {
            // destroy this button
            Object.Destroy(button);

            noOfActivities--;

            // check if amount of pages has decreased
            if (noOfActivities % 5 == 0 && noOfActivities != 0)
            {
                noOfPagesActivities--;

                // check if left/right buttons should show
                UpdatePageAmount(storedAct);

                if (currentPage >= noOfPagesActivities)
                {
                    currentPage = noOfPagesActivities;

                    // update which page we're on
                    ChangePage(storedAct);
                }
            }

            // update each button's position to remove any gaps
            UpdateButtonPosition(storedAct);

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
        // remove from the saved list
        selectedAct.RemoveInstruction(button);
        
        // if amount of instructions is fewer than we had before (instruction deleted)
        if (selectedAct.instructions.Count < noOfInstructions)
        {
            Object.Destroy(button);

            noOfInstructions--;

            // check if amount of pages has decreased
            if (noOfInstructions % 5 == 0 && noOfInstructions != 0)
            {
                noOfPagesInstructions--;

                // check if left/right buttons should show
                UpdatePageAmount(storedIns);

                if (currentPage >= noOfPagesInstructions)
                {
                    currentPage = noOfPagesInstructions;

                    // update which page we're on
                    ChangePage(storedIns);
                }
            }

            // update each button's position to remove any gaps
            UpdateButtonPosition(storedIns);

            return true;
        }
        else
        {
            Debug.Log("ERR: Nothing removed");
            return false;
        }
    }

    /** Remove all instruction buttons but not the instruction itself. Used when exiting out of an activity. */
    public void DeleteInstructionButton()
    {
        // for every child that's not left/right buttons (for every instruction)
        for (int h = 2; h < storedIns.transform.childCount; h++)
        {
            // remove the instruction
            Object.Destroy(storedIns.transform.GetChild(h).gameObject);
        }

        // reset variables
        noOfInstructions = 0;
        noOfPagesInstructions = 0;
        currentPage = 0;

        // check if we have enough pages to display left/right arrows
        UpdatePageAmount(storedAct);
    }
    /* ------------------------------------ */
    /** Decide which activities to display. Activities */
    public void ChangePage(GameObject storedObj)
    {
        for (int h = 0; h < storedObj.transform.childCount; h++)
        {
            // if child is not left/right buttons
            if (h >= 2)
            {
                // set inactive if gameobject's position in list isn't within range
                if ((h - 2) < (currentPage * 5) || (h - 2) > (currentPage * 5 + 4))
                {
                    if (storedObj.transform.GetChild(h).gameObject != null)
                    {
                        storedObj.transform.GetChild(h).gameObject.SetActive(false);
                    }
                }
                // set active if gameobject's position is within range
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

    /** Show the arrow keys if we have more than 1 page. */
    public void UpdatePageAmount(GameObject storedObj)
    {
        int noOfPages = 0;

        if (storedObj.name == storedAct.name)
        {
            noOfPages = noOfPagesActivities;
        }
        else if (storedObj.name == storedIns.name)
        {
            noOfPages = noOfPagesInstructions;
        }

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

    /** Update the position of all activities */
    public void UpdateButtonPosition(GameObject storedObj)
    {
       // int currentButt = 0;
        // has passed storedobject - used in order to not count it as an object as it isn't deleted before this check for some reason
        bool hasPassedSo = false;

        for (int h = 0; h < storedObj.transform.childCount; h++)
        {
            if (h >= 2)
            {
                // if gameObject isn't selectedObject
                if (storedObj.transform.GetChild(h).gameObject != selectedObj)
                {
                    // if it's not passed so
                    if (!hasPassedSo)
                    {
                        // set new position
                        storedObj.transform.GetChild(h).gameObject.transform.localPosition = activityPos[(h - 2) % 5]; // h-2 = currentButt

                        // if within range of currently visible buttons (h - 2 is equivalent of h if left/right buttons weren't included)
                        if ((h - 2) >= (currentPage * 5) && (h - 2) <= (currentPage * 5 + 4))
                        {
                            // make sure it's visible
                            storedObj.transform.GetChild(h).gameObject.SetActive(true);
                        }
                    }
                    // if has passed so
                    else
                    {
                        // set new position
                        storedObj.transform.GetChild(h).gameObject.transform.localPosition = activityPos[(h - 3) % 5]; // h-3 = currentButt

                        // if within range of currently visible buttons (h - 3 is equivalent of h if left/right/selectedObj buttons weren't included)
                        if ((h - 3) >= (currentPage * 5) && (h - 3) <= (currentPage * 5 + 4))
                        {
                            // make sure it's visible
                            storedObj.transform.GetChild(h).gameObject.SetActive(true);
                        }
                    }

                   // currentButt++;
                }
                // if gameObject is selectedObject
                else
                {
                    hasPassedSo = true;
                }
            }
        }
    }

    /* ------------------------------------ */
    /* Get/Set Functions */
    /* ------------------------------------ */

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
    /* ------------------------------------ */
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
    /* ------------------------------------ */
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
    /* ------------------------------------ */
    public Activity GetSelectedActivity()
    {
        return selectedAct;
    }

    public void SetSelectedActivity(Activity sa)
    {
        selectedAct = sa;
    }
    /* ------------------------------------ */
    public Instructions GetSelectedInstruction()
    {
        return selectedInstruction;
    }

    public void SetSelectedInstruction(Instructions si)
    {
        selectedInstruction = si;
    }
    /* ------------------------------------ */
    /** Return amount of activities we have. */
    public int GetActivityAmount()
    {
        return noOfActivities;
    }
    
    /** Return amount of instructions we have. */
    public int GetInstructionAmount()
    {
        return noOfInstructions;
    }

    /** Return amount of pages we have. */
    public int GetActivityPageAmount()
    {
        return noOfPagesActivities;
    }   
    /** Return amount of pages we have. */
    public int GetInstructionPageAmount()
    {
        return noOfPagesInstructions;
    }

    /** Return the set positions of buttons for the menus. */
    public Vector3[] GetActivityPos()
    {
        return activityPos;
    }
    /* ------------------------------------ */
    /** Edit the name of an activity. */
    public void SetNameActivity(string newName)
    {
        foundAct = container.activities.Find(x => x.name == selectedAct.name);
        foundAct.setName(newName);
        selectedAct = container.activities.Find(x => x.name == newName);
    }

    /** Edit an instruction. */
    public void SetNameInstruction(string newText)
    {
        selectedInstruction.setInstructionName(newText, selectedAct.name);
        selectedObj.name = selectedInstruction.instructionText;
        selectedObj.GetComponentInChildren<TextMesh>().text = newText;
    }
}
