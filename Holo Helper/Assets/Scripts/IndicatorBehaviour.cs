using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class IndicatorBehaviour : MonoBehaviour,IInputClickHandler{
    public Instructions instruction { get; set; }
    private bool isMoving = false;
    private int countTilPop = 0;

    Vector3 positionPlacement;
    Vector3 headPos, gazeDirection;
    Quaternion qtot;
    float DefaultGazeDistance = 1.0f;

    public void OnInputClicked(InputClickedEventData eventData)
    {
      /*  if (instruction.isEditOrUserMode)
        {
            isMoving = !isMoving;

            this.GetComponent<AudioSource>().clip = Resources.Load("sfx_pop") as AudioClip;
            this.GetComponent<AudioSource>().volume = 1;

            if (isMoving)
            {
                this.GetComponent<AudioSource>().pitch = 0.5f;
            }
            else
            {
                this.GetComponent<AudioSource>().pitch = 0.3f;
            }

            this.GetComponent<AudioSource>().Play();
        }*/
    }
        
    public void Update()
    {
      /*  if (isMoving)
        {
            headPos = CameraCache.Main.transform.position;
            gazeDirection = CameraCache.Main.transform.forward;
            positionPlacement = headPos + (gazeDirection * DefaultGazeDistance);
            qtot = CameraCache.Main.transform.localRotation;
            qtot.x = 0;
            qtot.z = 0;
            this.gameObject.transform.rotation = qtot;
            this.gameObject.transform.position = positionPlacement;
        }*/
    }

    public void FixedUpdate()
    {
        if (!instruction.isEditMode)
        {

            float dist = Vector3.Distance(this.gameObject.transform.position, CameraCache.Main.transform.position);

            if (dist >= 1 && dist < 6)
            {
                this.GetComponent<AudioSource>().volume = (dist - 1) / 5;
            }
            else if (dist >= 6)
            {
                this.GetComponent<AudioSource>().volume = 1;
            }

            countTilPop++;

            if (countTilPop >= 300 && !isMoving)
            {
                this.GetComponent<AudioSource>().clip = Resources.Load("sfx_ping") as AudioClip;
                this.GetComponent<AudioSource>().pitch = 0.3f;

                this.GetComponent<AudioSource>().Play();
                countTilPop = 0;
            }
        }
    }
}
