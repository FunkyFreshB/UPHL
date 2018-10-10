using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour {
    
    public GameObject buttonBase;
    public List<GameObject> activities = new List<GameObject>();
    private int noOfActivities;
    private int noOfPages;
    private GameObject selectedObj;
    public  GameObject storedObj;
    public int currentPage = 0;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        if(noOfPages > 0)
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

    public GameObject CreateActivity(string name)
    {
        GameObject newActivity = Instantiate(buttonBase);
        newActivity.name = name;
        newActivity.GetComponentInChildren<TextMesh>().text = name;
        newActivity.transform.position = this.transform.position;
        newActivity.transform.rotation = this.transform.rotation;
        
        activities.Add(newActivity);
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
            Debug.Log("Name: " + selectedObj.name);
            return true;
        }
    }

    public void SetName(string name)
    {
        Debug.Log("Prev: " + selectedObj.name + ", " + selectedObj.GetComponentInChildren<TextMesh>().text);
        selectedObj.name = name;
        selectedObj.GetComponentInChildren<TextMesh>().text = name;
        Debug.Log("Next: " + selectedObj.name + ", " + selectedObj.GetComponentInChildren<TextMesh>().text);
    }
}
