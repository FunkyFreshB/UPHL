using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour {
    
    public GameObject buttonBase;
    public ArrayList activities = new ArrayList();
    private int noOfActivities;
    private int noOfPages;
    private GameObject selectedObj;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public GameObject CreateActivity(string name)
    {
        GameObject newActivity = Instantiate(buttonBase);
        newActivity.name = name;
        newActivity.GetComponentInChildren<TextMesh>().text = name;
        newActivity.transform.position = this.transform.position;
        
        activities.Add(newActivity);
        if (activities.Count > noOfActivities)
        {            
            if (noOfActivities % newActivity.GetComponent<ButtonBehaviour>().visibleActs == 0 && noOfActivities != 0)
            {
                noOfPages++;
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
        // GameObject act = button.GetActivity();
        // int index = act.GetActivityNumber():
        // activities.RemoveAt(index);
        activities.Remove(button);

        Debug.Log("NOA: " + noOfActivities);
        Debug.Log("a.C: " + activities.Count);

        if (activities.Count < noOfActivities)
        {
            noOfActivities--;

            if (noOfActivities % button.GetComponent<ButtonBehaviour>().visibleActs == 0 && noOfActivities != 0)
            {
                noOfPages--;
            }

            return true;
        }
        else
        {
            Debug.Log("Removed NOTHING!?");
            return false;
        }
    }

    public int GetActivityAmount()
    {
        return noOfActivities;
    }

    public int GetPageAmount()
    {
        return noOfPages;
    }

    public GameObject GetSelectedObject()
    {
        return selectedObj;
    }

    public bool SetSelectedObject(GameObject so)
    {
        selectedObj = so;
        if (selectedObj == null)
        {
            Debug.Log("NULL OBJ");
            return false;
        }
        else
        {
            Debug.Log(selectedObj.name);
            return true;
        }
    }
}
