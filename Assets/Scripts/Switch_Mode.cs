using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Mode : MonoBehaviour
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

    public Switch_Generic[] sw_Generic = new Switch_Generic[4];

    // Switch variables
    public bool debugMode = false;
    public bool demoMode = true;

    int Position = 0;               // Current switch position to off
    int oldPosition = 0;            // Track last known position to play sounds

    // Highlight variables
    Color restingColor = new Vector4(0.0f, 1.0f, 0.0f, 0.7f);
    Color clickedColor = new Vector4(1.0f, 1.0f, 0.0f, 0.7f);
    Color disabledColor = new Vector4(0.3f, 0.3f, 0.3f, 0.7f);      // Program switch upper limit is position 2

    const float fadeTimeTotal = 0.5f;
    float fadeTimerCW = 0.5f;
    float fadeTimerCCW = 0.5f;


    Vector3[] knobPositions = new Vector3[5]
    {
        new Vector3(180.0f, 0.0f, 180.0f),      // Off
        new Vector3(180.0f, 0.0f, 211.0f),      // Standby
        new Vector3(180.0f, 0.0f, 234.0f),      // Manual
        new Vector3(180.0f, 0.0f, 263.0f),      // Semi
        new Vector3(180.0f, 0.0f, 290.0f),      // Auto
    };


    void Start()
    {
        float diceRoll = Random.Range(0.0f, 1.0f);
        
        // Q: Wait why is this code public isn't this and OPSEC concern?
        // A: This panel is not only not classified, but physical simulated variants also can be purchased
        //    It's been disected, built, and documented in numerous flight sims, additionally the information
        //    here does not honestly depict how to cause a safety incidient. It is more to provide correct
        //    training specifically on how to load flares and maintain annual compentancy without need for a
        //    aircraft/training device should it not be available.
        if (!demoMode)
        {

            if (diceRoll < 0.05) Position = 3;           // 5% chance of switch being set to semi-auto
            if (diceRoll < 0.1) Position = 4;           // additional 5% more of switch in auto
            if (Position >= 2)
            {
                sw_Generic[(int)(Random.Range(0.0f, 3.0f))].setPosition(true);
            }
        }

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

        if (Position != 0) { 
            if (adjFadeTime > fadeTimeTotal) fadeTimerCCW = fadeTimeTotal;
            UI_Hint_CCW.color = Vector4.Lerp(clickedColor, restingColor, adjFadeTime);
        }
        else
        {
            UI_Hint_CCW.color = disabledColor;
        }

        int maxPosition = 4; 
        
        if(demoMode)
        {
            maxPosition = 1;
        }

        if (Position != maxPosition)
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

    public void Decrease()
    {
        Position--;
        fadeTimerCCW = 0.0f;
    }
    public void Increase()
    {
        if (demoMode && Position >= 1) return;
        Position++;
        fadeTimerCW = 0.0f;
    }

    public void Reset()
    {
        Position = 0;
        Start();
    }
}
