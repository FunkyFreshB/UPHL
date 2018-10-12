using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.IO;

public class CubeBehavior : MonoBehaviour, IInputClickHandler
    {
        public Instructions ins;
        public Activity act;
        public ActivityContainer activityList = new ActivityContainer();


        public CubeBehavior() { }

        public void OnInputClicked(InputClickedEventData eventData)
        {
        /*
        Instructions in1 = new Instructions("TESTTEXT1");
        Instructions in2 = new Instructions("TESTTEXT2");
        Instructions in3 = new Instructions("TESTTEXT3");
        Instructions[] block = { in1, in2, in3 };
        act = new Activity(block,"KOKA");
        activityList.activities.Add(act);

        activityList.Save(Path.Combine(Application.persistentDataPath, "ActivityList.xml"));
        */

    }

    }