using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class IndicatorBehaviour : MonoBehaviour,IInputClickHandler{
    public Instructions instruction { get; set; }
    private bool isMoving = false;
    private int countTilPop = 0;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (instruction.isEditOrUserMode)
        {
            isMoving = !isMoving;
        }
    }
        
    public void Update()
    {
        if (!isMoving) { return; }
        Vector3 positionPlacement;
        Vector3 headPos, gazeDirection;
        Quaternion qtot;
        float DefaultGazeDistance = 1.0f;
        headPos = CameraCache.Main.transform.position;
        gazeDirection = CameraCache.Main.transform.forward;
        positionPlacement =  headPos + (gazeDirection * DefaultGazeDistance);
        qtot = CameraCache.Main.transform.localRotation;
        qtot.x = 0;
        qtot.z = 0;
        this.gameObject.transform.rotation = qtot;
        this.gameObject.transform.position = positionPlacement;
    }

    public void FixedUpdate()
    {
        countTilPop++;
        if (countTilPop >= 150 && Vector3.Distance(this.gameObject.transform.position, CameraCache.Main.transform.position) < 5)
        {
            this.GetComponent<AudioSource>().Play();
            Debug.Log("pop");
            countTilPop = 0;
        }
    }
}
