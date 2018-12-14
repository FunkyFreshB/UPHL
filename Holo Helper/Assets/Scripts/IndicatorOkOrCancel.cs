using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

public class IndicatorOkOrCancel : MonoBehaviour, IInputClickHandler, IFocusable{

    // Use this for initialization

    public bool okOrCancel;
    private Vector3 tempPos;
    public GameObject actMan;
    private ActivityManager ams;
    public GameObject storedInstructions;
    public Material[] materials = new Material[2];

    void Awake()
    {
        ams = actMan.GetComponent<ActivityManager>();
    }

    public void OnFocusEnter()
    {
        this.gameObject.GetComponent<Renderer>().material = materials[1];
    }

    public void OnFocusExit()
    {
        this.gameObject.GetComponent<Renderer>().material = materials[0];
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        actMan.GetComponent<AudioSource>().PlayOneShot(ams.tap);

        if (okOrCancel)
        {
            if (WorldAnchorManager.Instance != null)
            {
                WorldAnchorManager.Instance.RemoveAnchor(ams.GetSelectedInstruction().indicator);
                WorldAnchorManager.Instance.AttachAnchor(ams.GetSelectedInstruction().indicator);
            }
        }
        else
        {
            WorldAnchorManager.Instance.RemoveAnchor(ams.GetSelectedInstruction().indicator);
            ams.GetSelectedInstruction().indicator.transform.position = tempPos;
            WorldAnchorManager.Instance.AttachAnchor(ams.GetSelectedInstruction().indicator);
        }

        ams.SetSelectedObject(null);
        ams.GetSelectedInstruction().indicator.GetComponent<TapToPlace>().IsBeingPlaced = false;
        ams.GetSelectedInstruction().indicator.GetComponent<TapToPlace>().enabled = false;
        ams.menus[3].SetActive(true);
        storedInstructions.SetActive(true);
        ams.GetSelectedInstruction().isEditMode = false;
        ams.GetSelectedInstruction().indicator.SetActive(false);

        this.gameObject.GetComponent<Renderer>().material = materials[0];

        this.gameObject.transform.parent.gameObject.SetActive(false);

    }

    void OnEnable()
    {
        tempPos = ams.GetSelectedInstruction().indicator.transform.position;
        ams.GetSelectedInstruction().isEditMode = true;
    }
}
