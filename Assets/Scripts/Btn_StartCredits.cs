using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_StartCredits : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject CreditsCamera;
    public SST_Credits Credits;

    private void OnMouseDown()
    {
        MainCamera.SetActive(false);
        CreditsCamera.SetActive(true);
        Credits.Reset();
        Credits.RollCredits();
    }
}
