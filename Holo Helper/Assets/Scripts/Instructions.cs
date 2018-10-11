using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class Instructions
    {
        [XmlElement("instructionText")]
        public string instructionText { get; set; }
        [XmlIgnore]
        public GameObject indicator;
        [XmlAttribute("visible")]
        public bool visible;

        public Instructions(string text)
        {
            instructionText = text;
            visible = true;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = text;
            
            cube.AddComponent<TapToPlace>();
            cube.AddComponent<AudioSource>();
            cube.AddComponent<TextToSpeech>();
            cube.AddComponent<IndicatorBehaviour>().instruction = this;
            indicator = cube;

        }

        public void changeVisibility()
        {
            if (visible)
            {
                indicator.SetActive(false);
                visible = !visible;
            }
            else
            {
                indicator.SetActive(true);
                visible = !visible;
            }
        }

        public Instructions()
        {
     
        }

        public string getInstructionText()
        {
            return instructionText;
        }

        public void setInstructionText(string newText)
        {
            instructionText = newText;
        }

        public void editText(string newText)
        {
        instructionText = newText;
        }

    }