using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.UI.Keyboard;

public class ButtonBehaviour : MonoBehaviour, IInputClickHandler, IFocusable {

    // Other objects
    public GameObject actMan;
    private GameObject storedActs;
    private GameObject[] menus = new GameObject[5];
    private Material[] materials = new Material[2];
    private ActivityManager ams;

    // Page info
    private int visibleActs = 5;
    private Vector3[] activityPos;
    private int currentPage = 0;
    private int activityID = -1;
    private int pageID = -1;

    private GameObject obj;
    public Activity connectedAct;
    private GameObject gazedAtObj;      // currently gazed at object

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

        menus = ams.menus;
        materials = ams.materials;
        storedActs = ams.storedObj;
        activityPos = ams.GetActivityPos();

        keyboard = Keyboard.Instance;
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
                    menus[2].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetPageAmount() + 1);
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
        obj.GetComponent<ButtonBehaviour>().menus = menus;
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
            UserMenu(eventData);
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
        if (ams.GetFirstTime())
        {
            foreach (Activity a in ams.container.activities)
            {
                InstantiateActivityButton(a.name, a);
            }

            ams.SetFirstTime(false);
        }

        if (isAdmin)
        {
            ams.SetCurrentPage(0);
            ams.ChangePage();
            menus[1].SetActive(true);
            menus[0].SetActive(false);
            menus[2].SetActive(false);
        }
        else
        {
            ams.SetCurrentPage(0);
            ams.ChangePage();
            menus[2].SetActive(true);
            menus[0].SetActive(false);
            menus[1].SetActive(false);
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
            menus[2].SetActive(false);
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

    public void UserMenu(InputClickedEventData eventData)
    {
        if (isActivity)
        {
            if (ams.GetSelectedObject() != null)
            {
                ams.GetSelectedObject().GetComponent<Renderer>().material = materials[0];
            }

            ams.SetSelectedObject(gazedAtObj);
        }
        else if (isReturn)
        {
            storedActs.SetActive(false);
            menus[0].SetActive(true);
            menus[1].SetActive(false);
            menus[2].SetActive(false);
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
            menus[2].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetPageAmount() + 1);
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
            menus[2].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetPageAmount() + 1);
        }
    }

    public void CreateKeyboard(bool iC)
    {
        isCreateKeyboard = iC;
        isKeyboard = true;
        keyboard.PresentKeyboard(keyboardText, Keyboard.LayoutType.Alpha);
    }
}
