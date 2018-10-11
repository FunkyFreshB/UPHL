using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using System;

public class TextSpeechWords : MonoBehaviour, ISpeechHandler {

    private TextToSpeech speech;
    private String[] instructions;
    public int counter = 0;
    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<TextMesh>().text = "Say Repeat";
        instructions = new String[4];
        instructions[0] = "Step 1";
        instructions[1] = "Step 2";
        instructions[2] = "Step 3";
        instructions[3] = "Step 4";

        speech = GetComponent<TextToSpeech>();
        
    }
	
    public void repeat()
    {
        this.gameObject.SetActive(true);
        GetComponent<TextMesh>().text = instructions[counter];
        speech.StartSpeaking(GetComponent<TextMesh>().text);
    }

    public void next()
    {
        counter++;
        if(counter == instructions.Length)
        {
            counter = 0;
        }

        GetComponent<TextMesh>().text = instructions[counter];
        speech.StartSpeaking(GetComponent<TextMesh>().text);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
