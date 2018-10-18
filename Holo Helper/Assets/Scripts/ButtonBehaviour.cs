using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.UI.Keyboard;

public class ButtonBehaviour : MonoBehaviour, IInputClickHandler, IFocusable {

    // Other objects
    public GameObject actMan;
    public GameObject storedActs;
    public GameObject[] menus = new GameObject[5];
    public Material[] materials = new Material[2];
    private ActivityManager ams;
    bool firstTime = true;

    // Page info
    public int visibleActs = 5;
    public Vector3[] activityPos;
    private int currentPage = 0;
    private int activityID = -1;
    private int pageID = -1;

    private GameObject obj;
    public Activity connectedAct;
    public GameObject gazedAtObj;      // currently gazed at object

    public bool isAdmin = false;
    public bool isActivity = false;
    public bool isEdit = false;
    public bool isCreate = false;
    public bool isDelete = false;
    public bool isReturn = false;
    public bool isPageLeft = false;
    public bool isPageRight = false;
    bool isCreateKeyboard = false;

    bool isKeyboard = false;
    Keyboard keyboard;
    string keyboardText = "";

    // Use this for initialization
    void Start ()
    {
        ams = actMan.GetComponent<ActivityManager>();

       /* menus[0].SetActive(true);

        for (int i = 1; i < menus.Length; i++)
        {
            if (menus[i] != null)
            {
                menus[i].SetActive(false);
            }
        }*/

        keyboard = Keyboard.Instance;

        activityPos = new Vector3[visibleActs];

        activityPos[0] = new Vector3(0, 0.10415f, 0);
        activityPos[1] = new Vector3(0, 0.0749f, 0);
        activityPos[2] = new Vector3(0, 0.04565f, 0);
        activityPos[3] = new Vector3(0, 0.0164f, 0);
        activityPos[4] = new Vector3(0, -0.01285f, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(this.gameObject == ams.GetSelectedObject())
        {
            ams.GetSelectedObject().GetComponent<Renderer>().material = materials[1];
        }

        if (isKeyboard)
        {
            if (!keyboard.isActiveAndEnabled)
            {
                if (isCreateKeyboard)
                {
                    InstantiateActivityButton(keyboardText, null);
                    keyboardText = "";
                    isKeyboard = false;
                    menus[1].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetPageAmount() + 1);
                }
                else
                {
                    actMan.GetComponent<ActivityManager>().SetName(keyboardText);
                    keyboardText = "";
                    isKeyboard = false;
                }
            }

            keyboardText = keyboard.InputField.text;
        }
    }

    /** Create a new button with the correct initialization. */
    public void InstantiateActivityButton(string name, Activity act)
    {
        obj = ams.CreateActivity(name, act);
        obj.GetComponent<ButtonBehaviour>().storedActs = storedActs;
        obj.GetComponent<ButtonBehaviour>().isAdmin = true;
        obj.GetComponent<ButtonBehaviour>().isActivity = true;
        obj.transform.localScale = new Vector3(0.23f, 0.0234f, 0.02f);
        obj.transform.GetChild(0).localScale = new Vector3(0.07f, 0.7f, 1);
        obj.transform.SetParent(storedActs.transform);

        // position of menu based on amount of activities
        obj.transform.localPosition = activityPos[(ams.GetActivityAmount() - 1) % visibleActs];
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Unhighlight the highlighted button
        this.gameObject.GetComponent<Renderer>().material = materials[0];

        // 0: Main Menu
        if (menus[0].activeSelf)
        {
            MainMenu(eventData);
            storedActs.SetActive(true);
        }

        // 1: Administrator Menu
        else if (menus[1].activeSelf)
        {
            AdminMenu(eventData);
        }

        // ---------------------------------------------

        // 2: User Menu
        else if (menus[2].activeSelf)
        {
            if (isActivity)
            {

            }
        }

        // ---------------------------------------------

        // 3: Activity Menu
        else if (menus[3].activeSelf)
        {

        }

        // ---------------------------------------------

        // 4: Edit Menu
    }

    public void OnFocusEnter()
    {
        gazedAtObj = this.gameObject;

        // Highlight looked at object
        this.gameObject.GetComponent<Renderer>().material = materials[1];
    }

    public void OnFocusExit()
    {
        gazedAtObj = null;

        // Unhighlight looked at object, so long as it's not the selected object
        if (this.gameObject != actMan.GetComponent<ActivityManager>().GetSelectedObject())
        {
            this.gameObject.GetComponent<Renderer>().material = materials[0];
        }
    }

    public void MainMenu(InputClickedEventData eventData)
    {
        if (isAdmin)
        {
            if (firstTime)
            {
                foreach (Activity a in ams.container.activities)
                {
                    InstantiateActivityButton(a.name, a);
                }

                firstTime = false;
            }
            
            menus[1].SetActive(true);
            menus[0].SetActive(false);
        }
        else
        {
            if (firstTime)
            {
                foreach (Activity a in ams.container.activities)
                {
                    InstantiateActivityButton(a.name, a);
                }

                firstTime = false;
            }

            menus[2].SetActive(true);
            menus[0].SetActive(false);
        }
    }

    public void AdminMenu(InputClickedEventData eventData)
    {

        if (isCreate)
        {
            CreateKeyboard(true);
        }
        else if (isActivity)
        {
            if (ams.GetSelectedObject() != null)
            {
                ams.GetSelectedObject().GetComponent<Renderer>().material = materials[0];
            }

            ams.SetSelectedObject(gazedAtObj);
        }
        else if (isEdit)
        {
            CreateKeyboard(false);
            //menus[3].SetActive(true);
            //menus[1].SetActive(false);
        }
        else if (isDelete)
        {
            ams.DeleteActivity(ams.GetSelectedObject());
            menus[1].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetPageAmount() + 1);
        }
        else if (isReturn)
        {
            storedActs.SetActive(false);
            menus[0].SetActive(true);
            menus[1].SetActive(false);
        }

        // Change page
        else if (isPageRight)
        {
            if (ams.GetCurrentPage() < ams.GetPageAmount())
            {
                ams.SetCurrentPage(ams.GetCurrentPage() + 1);
            }
            else
            {
                ams.SetCurrentPage(ams.GetPageAmount());
            }

            ams.ChangePage();
            menus[1].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetPageAmount() + 1);
        }
        else if (isPageLeft)
        {
            if (ams.GetCurrentPage() > 0)
            {
                ams.SetCurrentPage(ams.GetCurrentPage() - 1);
            }
            else
            {
                ams.SetCurrentPage(0);
            }

            ams.ChangePage();
            menus[1].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetPageAmount() + 1);
        }
    }

    public void CreateKeyboard(bool iC)
    {
        isCreateKeyboard = iC;
        isKeyboard = true;
        keyboard.PresentKeyboard(keyboardText, Keyboard.LayoutType.Alpha);
    }
}
