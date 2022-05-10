using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CmdsJettisonSwitch : MonoBehaviour {

    // Public objects    
    public GameObject Mdl_Switch_On;        // Models for toggle switch
    public GameObject Mdl_Switch_Off;

    CmdsManager cmdsMgr;                    // CMDS Manager handles all switch positions

    bool oldPosition = true;                // Track last known position to play sounds & efficiency

    AudioSource snd_Switched;       // Switch Sounds

    Vector3[] knobPositions = new Vector3[2]
    {
        new Vector3(0.0f, 0.0f, 20.0f),
        new Vector3(0.0f, 0.0f, -20.0f),
};


    void Start()
    {
        cmdsMgr = GameObject.Find("CMDS Panel").GetComponent<CmdsManager>();
        snd_Switched = GameObject.Find("Snd - Switch").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (cmdsMgr.Switch_Jettison == oldPosition) return;

        if (cmdsMgr.Switch_Jettison)
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

        oldPosition = cmdsMgr.Switch_Jettison;

    }
}
