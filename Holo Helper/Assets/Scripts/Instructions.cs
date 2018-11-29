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
    
    [XmlIgnore]
    public bool isEditMode;

    public Instructions() { }

    // TODO What script do we need or should we implement our own?
    public Instructions(string text,string activityName)
    {
        instructionText = text;
        isEditMode = false;
        this.reInitializer(activityName);
    }

    public void reInitializer(string activityName)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        cube.name = activityName + this.instructionText;
        cube.AddComponent<TapToPlace>().enabled = true;
        cube.GetComponent<TapToPlace>().DefaultGazeDistance = 1.0f;
     //   cube.AddComponent<SolverSurfaceMagnetism>().MagneticSurface = (1 << 31);
     //   cube.GetComponent<SolverSurfaceMagnetism>().MaxDistance = 0.5f;

        cube.GetComponent<Renderer>().material = (Material)Resources.Load("IndicatorMat");

        cube.AddComponent<IndicatorBehaviour>().instruction = this;

        cube.AddComponent<AudioSource>();
        cube.GetComponent<AudioSource>().playOnAwake = false;
        cube.GetComponent<AudioSource>().spatialize = true;
        cube.GetComponent<AudioSource>().spatialBlend = 1;
        cube.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Custom;

        indicator = cube;
        indicator.transform.parent = GameObject.Find("Indicators").transform;
        indicator.SetActive(false);
    }

    public void removeIndicator()
    {
        if(WorldAnchorManager.Instance != null)
        {
            WorldAnchorManager.Instance.RemoveAnchor(indicator.name); 
            Object.Destroy(indicator);
        }
    }

    public void setInstructionName(string text, string acitivity)
    {
        instructionText = text;
        indicator.name = acitivity + instructionText;

    }
}