using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Legacy UI code from older version of Unity, should be replaced at some point or just contained in one scene.
/// </summary>
public class CMDS_FailUI : MonoBehaviour {

    public GUIStyle big;
    public GUIStyle normal;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        big.fontSize = (int)(Screen.width * 0.085f);
        normal.fontSize = (int)(Screen.width * 0.03f);

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", GUIStyle.none))
        {
            SceneManager.LoadScene("CMDS Panel");
        }
    }

    /////////// HIGH USE FUNCTIONS ///////////


    /// <summary>
    /// sX(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">X position from left to right</param>
    /// <returns></returns>    
    public float ScaleX(float i)
    {
        i = (float)Screen.width * i;
        return i;
    }


    /// <summary>
    /// sY(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">Y position from top to bottom</param>
    /// <returns></returns>    
    public float ScaleY(float i)
    {
        i = (float)Screen.height * i;
        return i;
    }
}
