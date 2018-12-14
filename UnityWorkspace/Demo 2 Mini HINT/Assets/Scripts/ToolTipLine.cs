using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipLine : MonoBehaviour {


    LineRenderer line;
    [Tooltip("Target GameObject the tooltip should be connected to. (if =null target is parent GameObject")]
    public GameObject target;

    // Use this for initialization
    void Start () {
        if (target == null)
            target = gameObject.transform.parent.parent.gameObject;
        line = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null) {
            line.SetPosition(0, gameObject.transform.position);
            line.SetPosition(1, target.transform.position);
        }
    }
}
