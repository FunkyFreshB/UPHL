using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Menu : MonoBehaviour, I_TappedInterface {

    public GameObject lookAtObj;
    bool buttonsShown = false;

    // Use this for initialization
    void Start () {

    }

    public void OnTapped()
    {
        if (!buttonsShown)
        {
            foreach (Transform child in this.transform)
            {
                if(child != this.transform.GetChild(0))
                    child.gameObject.SetActive(true);
            }
            buttonsShown = true;
            return;
        }
        else if(buttonsShown && this.tag == "Menu_Main")
        {
            foreach (Transform child in this.transform)
            {
                if (child != this.transform.GetChild(0))
                    child.gameObject.SetActive(false);
            }
            buttonsShown = false;
            return;
        }
    }

    // Update is called once per frame
    void Update () {
        lookAtObj.transform.LookAt(Camera.main.transform);
	}
}
