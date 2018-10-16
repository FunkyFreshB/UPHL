using UnityEngine;
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

    [XmlElement("StepNumber")]
    public int stepNumber { get; set; }


    // TODO What script do we need or should we implement our own?
    public Instructions(string text, int stepNumber)
    {
        instructionText = text;
        visible = true;
        this.stepNumber = stepNumber;

        this.reInitializer();
    }

    public void reInitializer()
    {
       /* GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube = Resources.Load("Arrow") as GameObject;
        cube.name = this.instructionText;
        cube.AddComponent<TapToPlace>();
        //cube.AddComponent<SolverSurfaceMagnetism>().MagneticSurface = GameObject.Find("SpatialMapping").layer;
        cube.GetComponent<SolverSurfaceMagnetism>().MaxDistance = 0.5f;
        cube.AddComponent<IndicatorBehaviour>().instruction = this;
        cube.AddComponent<WorldAnchorManager>().PersistentAnchors = true;
        indicator = cube;
        */
    }

    public Instructions()
    {

    }

    public void ChangeVisibility()
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


    ~Instructions()
    {
        Debug.Log("The instruction \"" + instructionText + "\" have been destroyed");
    }

}