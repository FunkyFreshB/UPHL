using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour {

    public GameObject buttonBase;
    public ArrayList activities = new ArrayList();
    private int noOfActivities = 0;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
    //    this.transform.LookAt(Camera.main.transform);
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
            noOfActivities++;
            Debug.Log("Added " + name);
            return newActivity;
        }
        else
        {
            Debug.Log("Added NOTHING!?");
            return null;
        }
    }
    public bool DeleteActivity(int index)
    {
         activities.RemoveAt(index);
        
        if(activities.Count < noOfActivities)
        {
            noOfActivities--;
            Debug.Log("Removed " + name);
            return true;
        }
        else
        {
            Debug.Log("Removed NOTHING!?");
            return false;
        }
    }
}
