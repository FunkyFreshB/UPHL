using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class Activity{
    [XmlAttribute("name")]
    public string name { get; set; }

    [XmlArray("Instructions"), XmlArrayItem("Instruction")]
    public List<Instructions> instructions;

    [XmlAttribute("stepCounter")]
    public int stepCounter;

    public Activity(Instructions[] data, string name)
    {
        this.name = name;
        instructions = new List<Instructions>();
        foreach(Instructions element in data)
        {
            instructions.Add(element);
        }
    }

    public Activity(string name)
    {
        this.name = name;
        instructions = new List<Instructions>();
        instructions.Add(new Instructions("Standard instruction step 1"));
    }

    public Activity()
    {
      
    }

    public void printIns()
    {
        /*
        foreach(Instructions element in instructions)
        {
            Debug.Log(element.getInstructionText());
        }
        */
        
    }

    public void createCubes()
    {
        foreach(Instructions element in instructions)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = element.getInstructionText();
            cube.AddComponent<TapToPlace>();
            cube.AddComponent<IndicatorBehaviour>().instruction = element;
            element.indicator = cube;
            GameObject obj = GameObject.Find("Test");
            element.indicator.transform.parent = obj.transform;
        }
    }
}
