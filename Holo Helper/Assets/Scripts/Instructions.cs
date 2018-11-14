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

        cube.AddComponent<AudioSource>();
        cube.GetComponent<AudioSource>().playOnAwake = false;
        cube.GetComponent<AudioSource>().spatialize = true;
        cube.GetComponent<AudioSource>().spatialBlend = 1;
        cube.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Custom;

        indicator = cube;
        indicator.transform.parent = GameObject.Find("Indicators").transform;
        indicator.SetActive(false);

        Vector3 positionPlacement;
        Vector3 headPos, gazeDirection;
        Quaternion qtot;
        float DefaultGazeDistance = 1.0f;
        headPos = CameraCache.Main.transform.position;
        gazeDirection = CameraCache.Main.transform.forward;
        positionPlacement = headPos + (gazeDirection * DefaultGazeDistance);
        qtot = CameraCache.Main.transform.localRotation;
        qtot.x = 0;
        qtot.z = 0;
        cube.transform.rotation = qtot;
        cube.transform.position = positionPlacement;

    }

    public void removeIndicator()
    {
        if(WorldAnchorManager.Instance != null)
        {
            WorldAnchorManager.Instance.RemoveAnchor(indicator); 
            Object.Destroy(indicator);
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