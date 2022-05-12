using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Program : MonoBehaviour
{
    // External objects
    // Switches are two objects, and not animated
    public GameObject ParentObject;
    public Transform t_KnobModel;
    
    public MeshRenderer Collision_Hintbox_CCW;
    public SpriteRenderer UI_Hint_CCW;

    public MeshRenderer Collision_Hintbox_CW;
    public SpriteRenderer UI_Hint_CW;

    public AudioSource snd_Switched;       // Switch Sounds

    // Switch variables
    public bool debugMode = false;

    int Position = 1;               // Current switch position to off
    int oldPosition = 1;            // Track last known position to play sounds

    // Highlight variables
    Color restingColor = new Vector4(0.0f, 1.0f, 0.0f, 0.7f);
    Color clickedColor = new Vector4(1.0f, 1.0f, 0.0f, 0.7f);
    Color disabledColor = new Vector4(0.3f, 0.3f, 0.3f, 0.7f);      // Program switch upper limit is position 2

    const float fadeTimeTotal = 0.5f;
    float fadeTimerCW = 0.5f;
    float fadeTimerCCW = 0.5f;


    Vector3[] knobPositions = new Vector3[3]
    {
        new Vector3(180.0f, 0.0f, 180.0f),          // Bit
        new Vector3(180.0f, 0.0f, 218.0f),          // 1
        new Vector3(180.0f, 0.0f, 250.0f)           // 2
    };


    void Start()
    {
        if (debugMode)
        {
            string dbgString = "Switch: " + ParentObject.gameObject.name + " debugging enabled!";
            Debug.Log(dbgString);
        }
        else
        {
            Collision_Hintbox_CCW.enabled = false;
            Collision_Hintbox_CW.enabled = false;
        }


    }

    void Update()
    {
        UIFade();           // Update color for attached UI Hints

        if (Position == oldPosition) return;

        t_KnobModel.eulerAngles = knobPositions[Position];

        snd_Switched.Play();

        oldPosition = Position;

        if (debugMode)
        {
            string dbgString = "Switch: " + ParentObject.gameObject.name + " is " + Position + "!";
            Debug.Log(dbgString);

        }
    }

    /// <summary>
    /// Manages the timing and color of all UI hint elements
    /// </summary>
    private void UIFade()
    {
        float adjFadeTime = fadeTimerCCW * (1.0f / fadeTimeTotal);
        if (adjFadeTime > fadeTimeTotal) fadeTimerCCW = fadeTimeTotal;
        UI_Hint_CCW.color = Vector4.Lerp(clickedColor, restingColor, adjFadeTime);

        if (Position != 2)
        {
            adjFadeTime = fadeTimerCW * (1.0f / fadeTimeTotal);

            if (adjFadeTime > fadeTimeTotal) fadeTimerCW = fadeTimeTotal;
            UI_Hint_CW.color = Vector4.Lerp(clickedColor, restingColor, adjFadeTime);
        }
        else
        {
            UI_Hint_CW.color = disabledColor;
        }

        fadeTimerCCW += Time.deltaTime;
        fadeTimerCW += Time.deltaTime;
    }

    public int SwitchPosition()
    {
        return Position;
    }

    /// <summary>
    /// Collision boxes register raycasted mouse clicks, and set next position here.
    /// </summary>
    public void SetPosition(int i)
    {
        Position = i;
        if (Position < oldPosition)
        {
            fadeTimerCCW = 0.0f;
        }
        else if (Position == 2 && oldPosition == 1)
        {
            fadeTimerCW = 0.0f;
        }
            
    }
}
