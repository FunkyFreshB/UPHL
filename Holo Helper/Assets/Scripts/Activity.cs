using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using System.Xml;
using System.Xml.Serialization;
using HoloToolkit.Unity;

[System.Serializable]
public class Activity{
    
    [XmlElement("name")]
    public string name { get; set; }

    [XmlArray("Instructions"), XmlArrayItem("Instruction")]
    public List<Instructions> instructions;

    [XmlIgnore]
    public GameObject buttonBase;

    [XmlElement("stepCounter")]
    public int stepCounter { get; set; }

    [XmlElement("CurrentStep")]
    public int currentStep { get; set;}

   //TODO Hur ska vi göra med buttonBase, fråga Sebastian.
    public Activity(string name)
    {
        this.name = name;
        currentStep = 0;
        stepCounter = 1;
        instructions = new List<Instructions>();
        instructions.Add(new Instructions("Standard instruction step 1",stepCounter));

        buttonBase = Resources.Load("BUTTON") as GameObject;
        buttonBase.name = name;
    }

    public Activity()
    {
        this.name = "Standard Activity";
        instructions = new List<Instructions>();
        instructions.Add(new Instructions("Standard instruction step 1",stepCounter));
    }

    ~Activity()
    {
        Debug.Log("Activity" + name + " have been removed");
    }

    public void reInitializer()
    {

    }

    public void addInstruction(string description)
    {
        stepCounter++;
        instructions.Add(new Instructions(description,stepCounter));
    }



    //Hur gör vi detta? Behöver veta snarast för implementera resten.
    public void removInstruction()
    {
        if(stepCounter > 0)
        {

        }
    }

    public void changeActivityName(string newName)
    {
        this.name = newName;
    }



    public void nextStep()
    {
        if(currentStep != stepCounter-1)
        {
            currentStep++;
            GameObject.Find("SoundSourceHandler").GetComponent<TextToSpeech>().
            StartSpeaking("Step " + currentStep + " " + instructions[currentStep].instructionText);
        }
    }

    public void previousStep()
    {
        if(currentStep != 0)
        {
            currentStep--;
            GameObject.Find("SoundSourceHandler").GetComponent<TextToSpeech>().
            StartSpeaking("Step " + currentStep + " " + instructions[currentStep].instructionText);
        }
    }

    public void repeatStep()
    {
        GameObject.Find("SoundSourceHandler").GetComponent<TextToSpeech>().
            StartSpeaking("Step " + currentStep + " " + instructions[currentStep].instructionText);
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
            cube.name = element.instructionText;
            cube.AddComponent<TapToPlace>();
            cube.AddComponent<IndicatorBehaviour>().instruction = element;
            element.indicator = cube;
            GameObject obj = GameObject.Find("Test");
            element.indicator.transform.parent = obj.transform;
        }
    }
}
