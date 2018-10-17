using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using HoloToolkit.Unity;

[System.Serializable]
public class Activity{
    
    [XmlElement("name")]
    public string name { get; set; }

    [XmlArray("Instructions"), XmlArrayItem("Instruction")]
    public List<Instructions> instructions;

    [XmlElement("CurrentStep")]
    public int currentStep { get; set;}

    public Activity(string name)
    {
        this.name = name;
        currentStep = 0;
        instructions = new List<Instructions>();
        instructions.Add(new Instructions("Standard instruction step 1",this.name));
    }

    public Activity()
    {
       // this.name = "Standard Activity";
        //instructions = new List<Instructions>();
        //instructions.Add(new Instructions("Standard instruction step 1",stepCounter));
    }

    ~Activity()
    {
        Debug.Log("Activity" + name + " have been removed");
    }

    public void reInitializer()
    {
        foreach(Instructions i in instructions)
        {
            i.reInitializer(this.name);
        }
    }

    public void AddInstruction(string description)
    {
        instructions.Add(new Instructions(description,this.name));
    }



    //Hur gör vi detta? Behöver veta snarast för implementera resten.
    public void RemoveInstruction()
    {
        
    }

    public void NextStep()
    {
        if(currentStep != instructions.Count - 1)
        {
            currentStep++;
            GameObject.Find("SoundSourceHandler").GetComponent<TextToSpeech>().
            StartSpeaking("Step " + currentStep + " " + instructions[currentStep].instructionText);
        }
    }

    public void PreviousStep()
    {
        if(currentStep != 0)
        {
            currentStep--;
            GameObject.Find("SoundSourceHandler").GetComponent<TextToSpeech>().
            StartSpeaking("Step " + currentStep + " " + instructions[currentStep].instructionText);
        }
    }

    public void RepeatStep()
    {
        GameObject.Find("SoundSourceHandler").GetComponent<TextToSpeech>().
            StartSpeaking("Step " + currentStep + " " + instructions[currentStep].instructionText);
    }



    public void PrintIns()
    {
        /*
        foreach(Instructions element in instructions)
        {
            Debug.Log(element.getInstructionText());
        }
        */
        
    }

}
