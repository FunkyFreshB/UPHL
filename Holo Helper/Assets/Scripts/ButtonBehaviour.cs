using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.UI.Keyboard;
using TMPro;
using HoloToolkit.Unity.SpatialMapping;

public class ButtonBehaviour : MonoBehaviour, IInputClickHandler, IFocusable /*, ISpeechHandler*/ {

    // Other objects
    public GameObject actMan;                       // activity manager gameObject
    private GameObject storedActs;                  // Stored Activities gameObject
    private GameObject storedInstruction;           // Stored Instructions gameObject
    private GameObject[] menus = new GameObject[5]; // array containing menus
    private Material[] materials = new Material[3]; // array containing materials
    private ActivityManager ams;                    // script of activity manager

    // Page info
    private int visibleActs = 5;                    // number of items per page (basically locked to 5)
    private Vector3[] activityPos;                  // array of button positions
    private Vector3 buttonSize = new Vector3(0.3f, 0.03f, 0.03f);

    private GameObject obj;                         // used when creating buttons
    private GameObject gazedAtObj;                  // currently gazed at object

    public Instructions connectedInstruction;
    public Activity connectedAct;

    // bools that set this button's status
    // determines which actions it can perform depending on current menu
    public bool isAdmin = false;
    public bool isActivity = false;
    public bool isInstruction = false;
    public bool isCreate = false;
    public bool isEdit = false;
    public bool isChangeActivityName = false;
    public bool isDelete = false;
    public bool isReturn = false;
    public bool isPageLeft = false;
    public bool isPageRight = false;
    public bool isExit = false;
    public bool isSave = false;

    int keyboardCase = -1;                          // variable for switch/case
    bool isKeyboard = false;                        // determines if keyboard is active
    Keyboard keyboard;                              // the keyboard
    string keyboardText = "";                       // the text of the keyboard

    /* ------------------------------------ */
    /* General Functions */
    /* ------------------------------------ */

    // Use this for initialization
    void Start ()
    {
        ams = actMan.GetComponent<ActivityManager>();

        menus = ams.menus;
        materials = ams.materials;
        storedActs = ams.storedAct;
        storedInstruction = ams.storedIns;
        activityPos = ams.GetActivityPos();

        keyboard = Keyboard.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (this.gameObject == ams.GetSelectedObject())
        {
            ams.GetSelectedObject().GetComponent<Renderer>().material = materials[2];
        }
        else if (this.gameObject.name == "EXITBUTTON")
         {
             this.gameObject.GetComponent<Renderer>().material = materials[3];
         }
        else if (this.gameObject != gazedAtObj)
        {
            this.gameObject.GetComponent<Renderer>().material = materials[0];
        }

        if (isKeyboard)
        {
            if (!keyboard.isActiveAndEnabled)
            {
                switch (keyboardCase)
                {
                    case 1: // when creating activity
                        InstantiateActivityButton(keyboardText, null);
                        menus[1].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetActivityPageAmount() + 1);
                        menus[2].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetActivityPageAmount() + 1);
                        break;

                    case 2: // when creating instruction
                        InstantiateInstructionButton(keyboardText, null);
                        menus[3].transform.GetChild(1).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetInstructionPageAmount() + 1);
                        break;

                    case 3: // when editing instructions
                        ams.SetNameInstruction(keyboardText);
                        ams.GetSelectedInstruction().indicator.SetActive(true);
                        menus[3].SetActive(false);
                        storedInstruction.SetActive(false);
                        actMan.transform.GetChild(5).gameObject.SetActive(true);
                        break;

                    case 4: // when change activity name
                        actMan.GetComponent<ActivityManager>().SetNameActivity(keyboardText);
                        menus[3].transform.GetChild(0).GetComponent<TextMesh>().text = keyboardText;
                        for(int i = 2; i < storedActs.transform.childCount; i++)
                        {
                            if(storedActs.transform.GetChild(i).GetComponent<ButtonBehaviour>().connectedAct.name == keyboardText)
                            {
                                storedActs.transform.GetChild(i).name = keyboardText;

                                if (keyboardText.Length > 20)
                                {
                                    storedActs.transform.GetChild(i).GetChild(0).GetComponent<TextMesh>().text = keyboardText.Remove(20) + "...";
                                }
                                else
                                {
                                    storedActs.transform.GetChild(i).GetChild(0).GetComponent<TextMesh>().text = keyboardText;
                                }
                                break;
                            }
                        }
                        break;

                    default:
                        break;
                }

                // prevent potential misfire of switch/case
                keyboardCase = -1;
                
                keyboardText = "";
                isKeyboard = false;
            }

            keyboardText = keyboard.InputField.text;
        }
    }

    /* ------------------------------------ */
    /* Hololens Functions */
    /* ------------------------------------ */

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Unhighlight the highlighted button
        this.gameObject.GetComponent<Renderer>().material = materials[0];

        actMan.GetComponent<AudioSource>().PlayOneShot(ams.tap);

        // 0: Main Menu
        if (menus[0].activeSelf)
        {
            MainMenu(eventData);
        }

        // 1: Administrator Menu
        else if (menus[1].activeSelf)
        {
            AdminMenu(eventData);
        }

        // 2: User Menu
        else if (menus[2].activeSelf)
        {
            UserMenu();
        }

        // 3: Edit Menu
        else if (menus[3].activeSelf)
        {
            ActivityEditMenu(eventData);
        }

        // 4: Activity Menu
        else if (menus[4].activeSelf)
        {
            ActivityEventMenu(eventData);
        }

        // ---------------------------------------------


        if (ams.GetSelectedObject() != null)
        {
            menus[1].transform.GetChild(3).gameObject.SetActive(true);
            menus[1].transform.GetChild(4).gameObject.SetActive(true);

            menus[3].transform.GetChild(3).gameObject.SetActive(true);
            menus[3].transform.GetChild(5).gameObject.SetActive(true);

            if (ams.GetSelectedActivity().instructions.Count > 1)
            {
                menus[3].transform.GetChild(4).gameObject.SetActive(true);
            }
            else
            {
                if (menus[3].transform.GetChild(4).gameObject.activeSelf)
                {
                    menus[3].transform.GetChild(4).gameObject.SetActive(false);
                    menus[3].transform.GetChild(4).gameObject.transform.localScale /= 1.1f;
                }
            }
        }
        else
        {
            menus[1].transform.GetChild(3).gameObject.SetActive(false);
            menus[1].transform.GetChild(4).gameObject.SetActive(false);

            menus[3].transform.GetChild(3).gameObject.SetActive(false);
            menus[3].transform.GetChild(4).gameObject.SetActive(false);
            menus[3].transform.GetChild(5).gameObject.SetActive(false);
        }

        if (isReturn || isDelete && menus[1].activeSelf || isActivity && menus[4].activeSelf)
        {
            this.gameObject.transform.localScale /= 1.1f;
        }
    }

    public void OnFocusEnter()
    {
        gazedAtObj = this.gameObject;

        if (gazedAtObj.GetComponent<ButtonBehaviour>().isPageLeft || gazedAtObj.GetComponent<ButtonBehaviour>().isPageRight)        {        }
        else
        {
            this.gameObject.transform.localScale *= 1.1f;
        }

        // Highlight looked at object
        if (this.gameObject != ams.GetSelectedObject() || ams.GetSelectedObject() == null)
        {
            this.gameObject.GetComponent<Renderer>().material = materials[1];
        }
    }

    public void OnFocusExit()
    {
        if (gazedAtObj.GetComponent<ButtonBehaviour>().isPageLeft || gazedAtObj.GetComponent<ButtonBehaviour>().isPageRight)        {        }
        else
        {
            this.gameObject.transform.localScale /= 1.1f;
        }

        gazedAtObj = null;
        
        // Unhighlight looked at object, so long as it's not the selected object
        if (this.gameObject != ams.GetSelectedObject())
        {
            this.gameObject.GetComponent<Renderer>().material = materials[0];
        }
    }
    
    /* ------------------------------------ */
    /* Menu Functions */
    /* ------------------------------------ */

    public void MainMenu(InputClickedEventData eventData)
    {
        if (isExit)
        {
            ams.menuBg.SetActive(false);
            menus[0].SetActive(false);
            
            Application.Quit();
        }
        else
        {
            if (ams.GetFirstTime())
            {
                foreach (Activity a in ams.container.activities)
                {
                    InstantiateActivityButton(a.name, a);
                }

                ams.SetFirstTime(false);
            }

            ams.SetCurrentPage(0);
            ams.UpdatePageAmount(storedActs);
            ams.SetSelectedObject(null);

            if (isAdmin)
            {
                menus[1].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetActivityPageAmount() + 1);
                menus[1].SetActive(true);
            }
            else
            {
                menus[2].SetActive(true);
            }

            menus[0].SetActive(false);
            storedActs.SetActive(true);

            this.gameObject.transform.localScale /= 1.1f;
        }
    }

    public void AdminMenu(InputClickedEventData eventData)
    {
        if (isCreate)
        {
            CreateKeyboard(1, null);
        }
        else if (isActivity)
        {
            if (ams.GetSelectedObject() != null)
            {
                ams.GetSelectedObject().GetComponent<Renderer>().material = materials[0];
            }

            ams.SetSelectedObject(gazedAtObj);
        }
        else if (isEdit && ams.container.activities.Count != 0)
        {
            storedActs.SetActive(false);
            storedInstruction.SetActive(true);

            ams.SetSelectedActivity(ams.container.activities.Find(x => x.name == ams.GetSelectedObject().name)); //
            menus[3].transform.GetChild(0).GetComponent<TextMesh>().text = ams.GetSelectedObject().name;
            menus[3].transform.GetChild(1).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetInstructionPageAmount() + 1);

            foreach (Instructions i in ams.GetSelectedActivity().instructions)
            {
                InstantiateInstructionButton(i.instructionText, i);
            }

            menus[1].SetActive(false);
            menus[3].SetActive(true);

            ams.SetCurrentPage(0);
            ams.UpdatePageAmount(storedInstruction);

            this.gameObject.transform.localScale /= 1.1f;
            ams.SetSelectedObject(null);
        }
        else if (isDelete)
        {
            ams.DeleteActivity(ams.GetSelectedObject());
            menus[1].transform.GetChild(0).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetActivityPageAmount() + 1);
            
            ams.SetSelectedObject(null);
        }
        else if (isReturn)
        {
            ams.Voice_Return();
        }

        // Change page
        else if (isPageRight)
        {
            ams.Voice_Next();
        }
        else if (isPageLeft)
        {
            ams.Voice_Previous();
        }
    }

    public void UserMenu()
    {
        if (isActivity)
        {
            ams.SetSelectedActivity(gazedAtObj.GetComponent<ButtonBehaviour>().connectedAct);
            menus[4].transform.GetChild(1).GetComponent<TextMesh>().text = "Activity: " + ams.GetSelectedActivity().name;
            menus[4].transform.GetChild(2).GetComponent<TextMeshPro>().text = ams.GetSelectedActivity().instructions[ams.GetSelectedActivity().currentStep].instructionText;
            menus[4].transform.GetChild(0).GetComponent<TextMesh>().text = "Instruction " + (ams.GetSelectedActivity().currentStep + 1) + " / " + ams.GetSelectedActivity().instructions.Count;
            
            if (ams.GetSelectedActivity().instructions.Count == 1)
            {
                menus[4].transform.GetChild(3).gameObject.SetActive(false);
                menus[4].transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (ams.GetSelectedActivity().currentStep == 0)
            {
                menus[4].transform.GetChild(3).gameObject.SetActive(false);
                menus[4].transform.GetChild(4).gameObject.SetActive(true);
            }
            else if (ams.GetSelectedActivity().currentStep == (ams.GetSelectedActivity().instructions.Count - 1))
            {
                menus[4].transform.GetChild(3).gameObject.SetActive(true);
                menus[4].transform.GetChild(4).gameObject.SetActive(false);
            }
            else
            {
                menus[4].transform.GetChild(3).gameObject.SetActive(true);
                menus[4].transform.GetChild(4).gameObject.SetActive(true);
            }

            ams.GetSelectedActivity().RepeatStep();
            ams.GetSelectedActivity().instructions[ams.GetSelectedActivity().currentStep].indicator.SetActive(true);
            storedActs.SetActive(false);
            menus[2].SetActive(false);
            menus[4].SetActive(true);
        }
        else if (isReturn)
        {
            ams.Voice_Return();
        }

        // Change page
        else if (isPageRight)
        {
            ams.Voice_Next();
        }
        else if (isPageLeft)
        {
            ams.Voice_Previous();
        }
    }

    /** 3: When editing activities */
    public void ActivityEditMenu(InputClickedEventData eventData)
    {
        if (isCreate)
        {
            CreateKeyboard(2, null);
        }
        else if (isInstruction)
        {
            if (ams.GetSelectedObject() != null)
            {
                ams.GetSelectedObject().GetComponent<Renderer>().material = materials[0];
            }

            ams.SetSelectedObject(gazedAtObj);
        }
        else if (isEdit)
        {
            ams.SetSelectedInstruction(ams.GetSelectedActivity().instructions.Find(x => x.instructionText == ams.GetSelectedObject().name));
            CreateKeyboard(3, ams.GetSelectedObject().name);
            ams.GetSelectedObject().GetComponent<Renderer>().material = materials[0];

            ams.GetSelectedInstruction().isEditMode = true;
            ams.GetSelectedInstruction().indicator.GetComponent<TapToPlace>().enabled = true;
        }
        else if (isDelete)
        {
            ams.DeleteInstruction(ams.GetSelectedObject());
            menus[3].transform.GetChild(1).GetComponent<TextMesh>().text = "Page " + (ams.GetCurrentPage() + 1) + " / " + (ams.GetInstructionPageAmount() + 1);

            ams.SetSelectedInstruction(null);
        }
        else if (isChangeActivityName)
        {
            CreateKeyboard(4, null);
        }
        else if (isReturn)
        {
            ams.Voice_Return();
        }
        else if (isPageRight)
        {
            ams.Voice_Next();
        }
        else if (isPageLeft)
        {
            ams.Voice_Previous();
        }

        if(ams.GetSelectedActivity().instructions.Count == 1)
        {
            menus[4].transform.GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            menus[4].transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    /** 4: When an activity is active */
    public void ActivityEventMenu(InputClickedEventData eventData)
    {
        if (isPageLeft && isPageRight)
        {
            ams.Voice_Repeat();
        }
        else if (isPageLeft)
        {
            ams.Voice_Previous();
        }
        else if (isPageRight)
        {
            ams.Voice_Next();
        }
        else if (isReturn && isSave)
        {
            ams.Save();
            ams.Voice_Return();
        }
        else if (isReturn && !isSave)
        {
            ams.Voice_Return();
        }
    }

    /* ------------------------------------ */
    /* Create Functions */
    /* ------------------------------------ */

    /** Create a new button with the correct initialization. */
    public void InstantiateActivityButton(string name, Activity act)
    {
        obj = ams.CreateActivity(name, act);
        obj.GetComponent<ButtonBehaviour>().storedActs = storedActs;
        obj.GetComponent<ButtonBehaviour>().menus = menus;
        obj.GetComponent<ButtonBehaviour>().isActivity = true;
        obj.transform.localScale = buttonSize;  
        obj.transform.GetChild(0).localScale = new Vector3(0.07f, 0.7f, 1);
        obj.transform.SetParent(storedActs.transform);

        // position of menu based on amount of activities
        obj.transform.localPosition = activityPos[(ams.GetActivityAmount() - 1) % visibleActs];
    }

    /** Create a new button with the correct initialization. */
    public void InstantiateInstructionButton(string text, Instructions instruct)
    {
        obj = ams.CreateInstruction(text, instruct);
        obj.GetComponent<ButtonBehaviour>().storedInstruction = storedInstruction;
        obj.GetComponent<ButtonBehaviour>().menus = menus;
        obj.GetComponent<ButtonBehaviour>().isInstruction = true;
        obj.transform.localScale = buttonSize;
        obj.transform.GetChild(0).localScale = new Vector3(0.07f, 0.7f, 1);
        obj.transform.SetParent(storedInstruction.transform);

        // position of menu based on amount of activities
        obj.transform.localPosition = activityPos[(ams.GetInstructionAmount() - 1) % visibleActs];
    }

    /* ------------------------------------ */
    /* Keyboard Functions */
    /* ------------------------------------ */

    /** Instantiates a keyboard, its function determined by kCase */
    public void CreateKeyboard(int kCase, string text)
    {
        keyboardCase = kCase;
        isKeyboard = true;

        if (text == null)
        {
            keyboardText = "";
        }
        else
        {
            keyboardText = text;
        }

        keyboard.PresentKeyboard(keyboardText, Keyboard.LayoutType.Alpha);
    }
}
