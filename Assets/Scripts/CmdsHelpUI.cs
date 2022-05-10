using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CmdsHelpUI : MonoBehaviour {

    public GUIStyle big;
    public GUIStyle normal;
    public SpriteRenderer background;

    public CmdsManager cmds;

    public GameObject[] buttons= new GameObject[16];
    public GameObject[] lines = new GameObject[3];

    bool display = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        big.fontSize = (int)(Screen.width * 0.045f);
        normal.fontSize = (int)(Screen.width * 0.0125f);

    }

    public void ShowHelp()
    {
        for(int i = 0; i < 16; i++)
        {
            buttons[i].SetActive(false);
        }
        display = true;
        background.enabled = true;
        lines[0].SetActive(true);
        lines[1].SetActive(true);
        lines[2].SetActive(true);
    }

    void HideHelp()
    {
        for (int i = 0; i < 16; i++)
        {
            buttons[i].SetActive(true);
            if (i == 14 && !cmds.External_Power) buttons[i].SetActive(false);
            if (i == 15 && cmds.External_Power) buttons[i].SetActive(false);
        }
        display = false;
        background.enabled = false;
        lines[0].SetActive(false);
        lines[1].SetActive(false);
        lines[2].SetActive(false);
    }

    private void OnGUI()
    {
        if (!display) return;
        GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(0.2f)), "Help - CMDS Panel\n", big);
        GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.1f), ScaleX(0.970f), ScaleY(0.9f)), 
            "This simulator is designed to\n" +
            "replicate CMDS function for\n" +
			"checking flare counts only.\n" +
            "\n" +
            "\n" +
            "MODE Switch:\n" +
            "Selects mode of panel operation\n" +
            "\n" +
            " OFF - Self-explanitory\n" +
            " STBY- Flare launch inhibited\n" +
            " MAN - Manual Operation\n" +
            " SEMI- Same as AUTO\n" +
            " AUTO- Automated operation via\n" +
            "       MWS & IRCM\n" +
            "\n" +
            "\n" +
            "PRGM Switch:\n" +
            "Used to navigate through\n" +
            "menus on the LED display\n" +
            "\n" +
            "BIT - Skips to next option\n" +
            "  1 - Normal position, menus\n" +
            "      will auto scroll\n" +
            "  2 - Select option, pause\n" +
            "      scrolling\n" +
            "\n" +
            "\n" +
            "JETT Switch:\n" +
            " Do not touch this unless the\n" +
            " TO tells you to! Designed to\n" +
            " release all flares.\n" +
            "\n" +
            "\n" +
            "E-mail devin.bable@us.af.mil\n" +
            "with any questions or issues.", normal);

        if (GUI.Button(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", normal))
        {
            HideHelp();
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
