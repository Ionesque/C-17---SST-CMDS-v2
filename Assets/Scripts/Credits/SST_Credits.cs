using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rolls the credits
/// </summary>
public class SST_Credits : MonoBehaviour
{
    public Transform CreditsScroller;

    public AudioSource Muzak;
    
    Vector3 StartPos = new Vector3(0.0f, -1.482149f, 0.5f);
    Vector3 endPos = new Vector3(0.0f, 46.34328f, -3.657131f);
    bool rolling = false;
    float timer = 0.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartPos = CreditsScroller.localPosition;
        RollCredits();
    }

    // Update is called once per frame
    void Update()
    {
        if (rolling)
        {
            if (!Muzak.isPlaying) Muzak.Play();
            float currentLerp = timer / 70.0f;
            if (currentLerp > 1.0f) currentLerp = 1.0f;
            CreditsScroller.localPosition = Vector3.Lerp(StartPos, endPos, currentLerp);
            //CreditsScroller.Translate(Vector3.up * Time.deltaTime);   // Used to determine position
            timer += Time.deltaTime;
        }
        
    }

    /// <summary>
    /// Stops and resets credits
    /// </summary>
    public void Reset()
    {
        Muzak.Stop();
        rolling = false;
        CreditsScroller.localPosition = StartPos;
        timer = 0.0f;
    }

    /// <summary>
    /// Rolls Credits 
    /// </summary>
    public void RollCredits()
    {
        Reset();
        rolling = true;
    }
}
