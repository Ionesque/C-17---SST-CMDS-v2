using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic on/off toggle switch
/// </summary>
public class Switch_Generic : MonoBehaviour
{
    // External objects
    // Switches are two objects, and not animated
    public GameObject ParentObject;
    public GameObject Mdl_Switch_On;
    public GameObject Mdl_Switch_Off;
    public MeshRenderer Collision_Hintbox;
    public SpriteRenderer UI_Hint;

    public AudioSource snd_Switched;       // Switch Sounds

    // Switch variables
    public bool debugMode = false;

    bool Position = false;               // Current switch position to off
    bool oldPosition = false;            // Track last known position to play sounds

    // Highlight variables
    Color restingColor = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
    Color clickedColor = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);

    const float fadeTimeTotal = 0.5f;
    float fadeTimer = 0.5f;


    void Start()
    {
        if(debugMode)
        {
            string dbgString = "Switch: " + ParentObject.gameObject.name + " debugging enabled!";
            Debug.Log(dbgString);
        }
        else
        {
            Collision_Hintbox.enabled = false;
        }


    }

    void Update()
    {
        if(fadeTimer >= fadeTimeTotal)
        {
            UI_Hint.color = restingColor;
        }
        else
        {
            UI_Hint.color = Vector4.Lerp(clickedColor, restingColor, fadeTimer * (1.0f / fadeTimeTotal));
            fadeTimer += Time.deltaTime;
            string dbgString = "Fade Timer: " + fadeTimer;
            Debug.Log(dbgString);
        }
        
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
        
        if (debugMode)
        {
            string dbgString = "Switch: " + ParentObject.gameObject.name + " is " + Position + "!";
            Debug.Log(dbgString);
            
        }
    }

    public bool SwitchPosition()
    {
        return Position;
    }

    public void setPosition(bool i)
    {
        Position = i;
    }
    
    /// <summary>
    /// Collision boxes register raycasted mouse clicks, and set next position here.
    /// </summary>
    private void OnMouseDown()
    {
        Position = !Position;
        fadeTimer = 0.0f;
    }

}
