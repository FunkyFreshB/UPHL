using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using System.Xml;
using System.Xml.Serialization;
using HoloToolkit.Unity;

[System.Serializable]
public class Instructions
{
    [XmlElement("instructionText")]
    public string instructionText { get; set; }

    [XmlIgnore]
    public GameObject indicator;

    [XmlAttribute("visible")]
    public bool visible;

    [XmlIgnore]
    public bool isEditOrUserMode;

    // TODO What script do we need or should we implement our own?
    public Instructions(string text,string activityName)
    {
        instructionText = text;
        visible = true;
        isEditOrUserMode = false;
        this.reInitializer(activityName);
    }

    public Instructions()
    {

    }

    public void reInitializer(string activityName)
    {
        GameObject cube;
        //cube = Object.Instantiate(Resources.Load("Arrow") as GameObject);
        cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        cube.name = activityName + this.instructionText;
        //cube.AddComponent<TapToPlace>();
        //cube.AddComponent<SolverSurfaceMagnetism>().MagneticSurface = GameObject.Find("SpatialMapping").layer;
        //cube.GetComponent<SolverSurfaceMagnetism>().MaxDistance = 0.5f;
        cube.AddComponent<IndicatorBehaviour>().instruction = this;
        indicator = cube;
        indicator.transform.parent = GameObject.Find("Indicators").transform;
        indicator.SetActive(false);
        
    }

    public void removeIndicator()
    {
        Object.Destroy(indicator);
        if(WorldAnchorManager.Instance != null)
        {
            WorldAnchorManager.Instance.RemoveAnchor(indicator); 
        }
    }

    public void setInstructionName(string text, string acitivity)
    {
        WorldAnchorManager.Instance.RemoveAnchor(indicator);
        instructionText = text;
        indicator.name = acitivity + instructionText;
        WorldAnchorManager.Instance.AttachAnchor(indicator);

    }

    ~Instructions()
    {
       // Debug.Log("The instruction \"" + instructionText + "\" have been destroyed");
    }

}