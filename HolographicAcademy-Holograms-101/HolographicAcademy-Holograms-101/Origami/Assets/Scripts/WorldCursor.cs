using UnityEngine;

public class WorldCursor : MonoBehaviour {

    public MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        var headPos = Camera.main.transform.position;
        var gazePos = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if(Physics.Raycast(headPos,gazePos,out hitInfo))
        {
            meshRenderer.enabled = true;

            this.transform.position = hitInfo.point;
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }

        else
        {
            meshRenderer.enabled = false;
        }
	}
}
