
using UnityEngine;

public class WorldCursor : MonoBehaviour {

    private MeshRenderer cursor;

	// Use this for initialization
	void Start () {
        cursor = this.gameObject.GetComponentInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        var headPos = Camera.main.transform.position;
        var gazeDir = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if(Physics.Raycast(headPos,gazeDir, out hitInfo))
        {
            cursor.enabled = true;

            this.transform.position = hitInfo.point;

            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }

        else
        {
            cursor.enabled = false;
        }
	}
}
