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
        instructions.Add(new Instructions("Insert description", this.name));
    }

    public Activity()    {}

    public void setName(string newName)
    {
        foreach(Instructions i in instructions)
        {
            i.setInstructionName(i.instructionText, newName);
        }

        this.name = newName;
    }

    public void reInitializer()
    {
        foreach (Instructions i in instructions)
        {
            i.reInitializer(this.name);
        }
    }

    public void AddInstruction(string description)
    {
        instructions.Add(new Instructions(description,this.name));
    }

    public void RemoveInstruction(GameObject selectedInstruction)
    {
        Instructions temp = selectedInstruction.GetComponent<ButtonBehaviour>().connectedInstruction;

        temp.removeIndicator();
        instructions.Remove(temp);
    }

    public string NextStep()
    {
        if(currentStep != instructions.Count - 1)
        {
            ++currentStep;
            GameObject.Find("ACT_MANAGER").GetComponent<TextToSpeech>().StartSpeaking("Step " + (currentStep + 1) + ". " + instructions[currentStep].instructionText);
            instructions[currentStep - 1].indicator.SetActive(false);
            instructions[currentStep].indicator.SetActive(true);
        }

        return instructions[currentStep].instructionText;
    }

    public string PreviousStep()
    {
        if(currentStep != 0)
        {
            currentStep--;
            GameObject.Find("ACT_MANAGER").GetComponent<TextToSpeech>().StartSpeaking("Step " + (currentStep + 1) + ". " + instructions[currentStep].instructionText);
            instructions[currentStep + 1].indicator.SetActive(false);
            instructions[currentStep].indicator.SetActive(true);
        }

        return instructions[currentStep].instructionText;
    }

    public void RepeatStep()
    {
        GameObject.Find("ACT_MANAGER").GetComponent<TextToSpeech>().StartSpeaking("Step " + (currentStep + 1) + ". " + instructions[currentStep].instructionText);
    }
}
