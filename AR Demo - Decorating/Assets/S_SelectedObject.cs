using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SelectedObject : MonoBehaviour, I_TappedInterface {

    public GameObject menu;
    public GameObject menuClone;
    GameObject fo;
    GameObject co;
    bool hasSpawned = false;
    
    public void OnTapped()
    {
        fo = Camera.main.GetComponent<S_UserInput>().fo;
        co = Camera.main.GetComponent<S_UserInput>().co;

        if (fo == this.gameObject && !hasSpawned)
        {
            Vector3 spawnPos = new Vector3(transform.position.x,
                transform.position.y + 0.3f,
                transform.position.z);
            menuClone = Instantiate(menu, spawnPos, Quaternion.Euler(0,0,0));
            hasSpawned = true;
        }
    }

    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update ()
    {
		
        if (co == null)
        {
            hasSpawned = false;
            Destroy(menuClone);
        }
    }
}
