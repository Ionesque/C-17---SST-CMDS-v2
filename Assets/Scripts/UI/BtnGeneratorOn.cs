using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnGeneratorOn : MonoBehaviour
{
    public CmdsManager cmdsPanel;
    public AudioSource sndClick;
    public GameObject obj_GenOn;
    public GameObject obj_GenOff;

    public Vector3 initialSize;
    public Vector3 clickedSize;

    Transform t_Gen;

    bool powerState = false;
    


    private void Start()
    {
        t_Gen = this.GetComponent<Transform>();
    }

    private void OnMouseDown()
    {
        sndClick.Play();
        powerState = !powerState;

        if (powerState)
        {
            obj_GenOff.SetActive(false);
            obj_GenOn.SetActive(true);
            cmdsPanel.TogglePower(true);
            t_Gen.localScale = clickedSize;
        }
        else
        {
            obj_GenOff.SetActive(true);
            obj_GenOn.SetActive(false);
            cmdsPanel.TogglePower(false);
            t_Gen.localScale = clickedSize;
        }
    }

    private void OnMouseUp()
    {
        t_Gen.localScale = initialSize;

    }

    public void Reset()
    {
        powerState = false;
        obj_GenOff.SetActive(true);
        obj_GenOn.SetActive(false);
        t_Gen.localScale = initialSize;
    }
}
