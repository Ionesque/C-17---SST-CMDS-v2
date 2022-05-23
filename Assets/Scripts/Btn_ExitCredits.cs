using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_ExitCredits : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject CreditsCamera;
    public SST_Credits Credits;

    private void OnMouseDown()
    {
        Credits.Reset();
        CreditsCamera.SetActive(false);
        MainCamera.SetActive(true);
        
    }
}
