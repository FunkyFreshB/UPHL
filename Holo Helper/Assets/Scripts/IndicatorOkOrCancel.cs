using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class IndicatorOkOrCancel : MonoBehaviour, IInputClickHandler, IFocusable {

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
        if (okOrCancel)
        {
            if (WorldAnchorManager.Instance == null)
            {
                WorldAnchorManager.Instance.RemoveAnchor(ams.GetSelectedInstruction().indicator);
                WorldAnchorManager.Instance.AttachAnchor(ams.GetSelectedInstruction().indicator);
            }
        }

        else
        {
            ams.GetSelectedInstruction().indicator.transform.position = tempPos;
        }
        ams.menus[3].SetActive(true);
        storedInstructions.SetActive(true);
        ams.GetSelectedInstruction().indicator.SetActive(false);
        this.gameObject.GetComponent<Renderer>().material = materials[0];

        this.gameObject.GetParentRoot().transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.GetParentRoot().transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.GetParentRoot().transform.GetChild(2).gameObject.SetActive(false);

    }

    void OnEnable()
    {
        tempPos = ams.GetSelectedInstruction().indicator.transform.position;
    }
}
