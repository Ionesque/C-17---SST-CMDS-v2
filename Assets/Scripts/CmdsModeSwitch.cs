using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CmdsModeSwitch : MonoBehaviour {

    Transform t;
    CmdsManager cmdsMgr;     // CMDS Manager will handle all swicht positions

    int Position = 1;               // Current switch position, set by CMDS Manager
    int oldPosition = 1;            // Track last known position to play sounds

    AudioSource snd_Switched;       // Switch Sounds

    Vector3[] knobPositions = new Vector3[3]
    {
        new Vector3(180.0f, 0.0f, 180.0f),
        new Vector3(180.0f, 0.0f, 211.0f),
        new Vector3(180.0f, 0.0f, 290.0f)
    };


    void Start()
    {
        t = GetComponent<Transform>();
        cmdsMgr = GameObject.Find("CMDS Panel").GetComponent<CmdsManager>();
        snd_Switched = GameObject.Find("Snd - Switch").GetComponent<AudioSource>();
    }

    void Update()
    {
        Position = cmdsMgr.Switch_Mode;
        t.eulerAngles = knobPositions[Position];

        if (Position != oldPosition) snd_Switched.Play();
        oldPosition = Position;

    }

}
