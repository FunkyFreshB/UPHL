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

    // TODO What script do we need or should we implement our own?
    public Instructions(string text,string activityName)
    {
        instructionText = text;
        visible = true;
        this.reInitializer(activityName);
    }

    public Instructions()
    {

    }

    public void reInitializer(string activityName)
    {
        GameObject cube;
        cube = Object.Instantiate(Resources.Load("Arrow") as GameObject);
        cube.name = activityName + this.instructionText;
        cube.AddComponent<TapToPlace>();
        //cube.AddComponent<SolverSurfaceMagnetism>().MagneticSurface = GameObject.Find("SpatialMapping").layer;
        //cube.GetComponent<SolverSurfaceMagnetism>().MaxDistance = 0.5f;
        cube.AddComponent<IndicatorBehaviour>().instruction = this;
        indicator = cube;
        indicator.transform.SetParent(GameObject.Find("MenuPos").transform);
        indicator.SetActive(false);
        
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