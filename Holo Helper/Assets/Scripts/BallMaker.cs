using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.IO;

public class BallMaker : MonoBehaviour,IInputClickHandler {
    public CubeBehavior cu;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        ActivityContainer acts = GameObject.Find("Cube (1)").GetComponent<CubeBehavior>().activityList;
        acts = ActivityContainer.Load(Path.Combine(Application.dataPath, "ActivityList.xml"));

        foreach (Activity element in acts.activities)
        {
            element.PrintIns();
            element.CreateCubes();
        }
    }

    // Use this for initialization
    void Start () {
        cu = new CubeBehavior();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
