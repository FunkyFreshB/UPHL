﻿using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[System.Serializable]
[XmlRoot("AcitivitlistsAndInstructions")]
public class ActivityContainer
{
    [XmlArray("Activities"), XmlArrayItem("Activity")]
    public List<Activity> activities;

    public ActivityContainer()
    {
        activities = new List<Activity>();
    }


    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ActivityContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static ActivityContainer Load(string path)
    {
        if (File.Exists(path)) { 
            var serializer = new XmlSerializer(typeof(ActivityContainer));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as ActivityContainer;
            }
        }
        else
        {
            return null;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    //Kommer förmodligen inte användas
    public static ActivityContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ActivityContainer));
        return serializer.Deserialize(new StringReader(text)) as ActivityContainer;
    }


    public void CreateActivity(string name)
    {
        activities.Add(new Activity(name));
    }


    public void RemoveActivity(GameObject selectedActivity)
    {
        Activity temp = selectedActivity.GetComponent<ButtonBehaviour>().connectedAct;

        foreach (Instructions i in temp.instructions)
        {
            i.removeIndicator();
        }
        activities.Remove(temp);

        if (temp == null)
        {
           // Debug.Log("Activity " + temp + " destroyed");
        }
    }

}