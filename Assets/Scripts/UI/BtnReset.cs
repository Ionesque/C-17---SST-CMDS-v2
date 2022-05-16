using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnReset : MonoBehaviour
{
    public CmdsManager cmdsPanel;
    public AudioSource sndClick;
    public Switch_Mode swMode;
    public BtnGeneratorOn obj_GenBtn;

    public Vector3 initialSize;
    public Vector3 clickedSize;

    Transform t_Button;

    private void Start()
    {
        t_Button = this.GetComponent<Transform>();
    }

    private void OnMouseDown()
    {
        sndClick.Play();
        swMode.Reset();
        cmdsPanel.Reset();
        t_Button.localScale = clickedSize;
        obj_GenBtn.Reset();
    }

    private void OnMouseUp()
    {
        t_Button.localScale = initialSize;
    }
}
