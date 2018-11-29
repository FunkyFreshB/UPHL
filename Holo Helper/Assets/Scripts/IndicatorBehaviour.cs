using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

public class IndicatorBehaviour : MonoBehaviour {

    public Instructions instruction { get; set; }

    public void Update(){ }

    public void Start()
    {
        if (!instruction.isEditMode)
        {
            this.GetComponent<AudioSource>().clip = Resources.Load("sfx_ping") as AudioClip;
            this.GetComponent<AudioSource>().pitch = 0.3f;

            this.GetComponent<AudioSource>().playOnAwake = true;
            this.GetComponent<AudioSource>().loop = true;
            this.GetComponent<AudioSource>().Play();

        }
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
        }
    }
}
