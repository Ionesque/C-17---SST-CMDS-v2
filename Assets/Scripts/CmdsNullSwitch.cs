using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CmdsNullSwitch : MonoBehaviour
{

    public GameObject Mdl_Switch_On;
    public GameObject Mdl_Switch_Off;

    bool Position = false;               // Current switch position to off
    bool oldPosition = false;            // Track last known position to play sounds

    AudioSource snd_Switched;       // Switch Sounds

    void Start()
    {
        snd_Switched = GameObject.Find("Snd - Switch").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Position == oldPosition) return;

        if (Position)
        {
            Mdl_Switch_On.SetActive(true);
            Mdl_Switch_Off.SetActive(false);
        }
        else
        {
            Mdl_Switch_On.SetActive(false);
            Mdl_Switch_Off.SetActive(true);
        }

        snd_Switched.Play();

        oldPosition = Position;
    }

    public void toggleSwitch()
    {
        Position = !Position;
    }
}
