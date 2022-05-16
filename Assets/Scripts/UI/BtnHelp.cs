using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not using Unity's UI interface because it is unknown what device this will be ran on.
/// This button opens the shareware/help screens
/// </summary>
public class BtnHelp : MonoBehaviour
{
    public GameObject SharewareScreen;
    public AudioSource sndClick;

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
        t_Button.localScale = clickedSize;
        SharewareScreen.SetActive(true);
    }

    private void OnMouseUp()
    {
        t_Button.localScale = initialSize;
    }

    



}
