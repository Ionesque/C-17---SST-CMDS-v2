using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CmdsManager : MonoBehaviour
{
    // Display Group
    public TextMesh[] Status_NOGO = new TextMesh[2];
    public TextMesh Status_GO;
    public TextMesh Status_DispenseReady;

    public TextMesh Disp_LCD;

    public SpriteRenderer lcd_TestPattern;

    // Switches and other interactive elements
    public Switch_Jettison sw_Jettison;
    public Switch_Program sw_Program;


    public bool External_Power = false;
    public int Switch_Program = 1;
    public int Switch_Program_Last = 1;
    public int Switch_Mode = 0;

    public AudioSource Avionics_On;
    public AudioSource Avionics_Off;
    public AudioSource Flare_Launch;

    public GameObject Power_on;
    public GameObject Power_off;

    
    

    int failed = 0;

    int[] Flare_Ammo = new int[4]
    {
        40,
        40,
        120,
        40
    };

    int Flare_DamagedBucket = 0;
    int Mispolls = 0;
    const int Dropouts = 0;

    string[] BadFlarePrefix = new string[12]
    {
        " SQ1A A ",
        " SQ1A B ",
        " SQ2A A ",
        " SQ2A B ",
        " SQ3A A ",
        " SQ3A B ",
        " SQ1B A ",
        " SQ1B B ",
        " SQ2B A ",
        " SQ2B B ",
        " SQ3B A ",
        " SQ3B B ",
    };

    string[] BadFlareLocations = new string[12]
    {
        "E18",
        "E9 ",
        "E17",
        "E18",
        "E21",
        "E3 ",
        "E20",
        "E2 ",
        "E16",
        "E7 ",
        "E19",
        "E1 "
    };

    

    string[] MispollStrings = new string[8];
    string[] DropoutStrings = new string[8];

    public enum mode
    {
        off,
        BIT1,   // ALL ON 5 Sec
        BIT2,   // Blank 1 Sec
        BIT3,   // Versions 5 sec
        BIT4,   // RPT Flare ID 5 sec
        Flare_Count, // Flare counts main menu
        Main_OFP,
        Main_StatTRN,
        Main_OptFTI,
        Main_OptTRN,
        Main_MaintOp,
        Main_SquibData,
        Main_SystemCheck,
        Squib_Data,
        Squib_Dropout,
        Squib_Mispoll,
        Squib_Clear,
        Squib_MispollListing,
        Squib_DropoutListing,
        Not_Sim
    }

    public float screenTimer = 0.0f;
    public float programTimer = 0.0f;
    public float flareTimer = 0.0f;

    public mode CurrentMode = mode.off;

    int currentMispoll = 0;
    int currentDropout = 0;

    bool bitSelected = false;
    bool twoSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }



    // Update is called once per frame
    void Update()
    {
        ProcessFailure();
        if (failed > 0) return;

        readProgramSwitch();

        ProcessButtons();       // Check if powered
        if (CurrentMode != 0 || External_Power)
        {
            ProcessDisplay();
        }

        ProcessTimer();
        

        ProcessProgram();
        ProcessMode();
    }

    /// <summary>
    /// Processes failure sequences
    /// </summary>
    void ProcessFailure()
    {
        if (failed == 1)
        {
            if (!Avionics_On.isPlaying)
            {
                failed = 2;
            }

        }
        else if (failed == 2)
        {
            if (!Flare_Launch.isPlaying) Flare_Launch.Play();
            failed = 3;

        }
        else if (failed == 3)
        {
            if (!Flare_Launch.isPlaying) failed = 4;

        }
        else if (failed == 4)
        {
            SceneManager.LoadScene("Fail Screen");
        }
    }
    
    void ProcessDisplay()
    {
        if (CurrentMode == mode.off)
        {
            Status_NOGO[0].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_NOGO[1].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_GO.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Disp_LCD.text = "";
            lcd_TestPattern.enabled = false;
        }
        else if (CurrentMode == mode.BIT1)
        {
            Status_NOGO[0].color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            Status_NOGO[1].color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            Status_DispenseReady.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Status_GO.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Disp_LCD.text = "";
            lcd_TestPattern.enabled = true;
        }
        else if (CurrentMode == mode.BIT2)
        {
            Status_NOGO[0].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_NOGO[1].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_DispenseReady.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_GO.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Disp_LCD.text = "";
            lcd_TestPattern.enabled = false;
        }
        else if (CurrentMode == mode.BIT3)
        {
            Status_NOGO[0].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_NOGO[1].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_GO.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Disp_LCD.text = "OFP#1032MDF#6023";
        }
        else if (CurrentMode == mode.BIT4) Disp_LCD.text = "?RPT MAG IDS";
        else if (CurrentMode == mode.Flare_Count)
        {
            Disp_LCD.text = "  " + Flare_Ammo[0] + "  " + Flare_Ammo[1] + " " + Flare_Ammo[2] + "  " + Flare_Ammo[3];
        }
        else if (CurrentMode == mode.Main_OFP) Disp_LCD.text = "OFP#1032MDF#6023";
        else if (CurrentMode == mode.Main_StatTRN) Disp_LCD.text = "TRN OFF FTI OFF";
        else if (CurrentMode == mode.Main_OptFTI) Disp_LCD.text = "?TURN ON FTI";
        else if (CurrentMode == mode.Main_OptTRN) Disp_LCD.text = "?TURN ON TRN";
        else if (CurrentMode == mode.Main_MaintOp) Disp_LCD.text = "?MAINTENANCE OP";
        else if (CurrentMode == mode.Main_SquibData) Disp_LCD.text = "?RPT SQUIB DATA";
        else if (CurrentMode == mode.Main_SystemCheck) Disp_LCD.text = "?SYSTEM CHECKOUT";
        else if (CurrentMode == mode.Squib_Data) Disp_LCD.text = "Squib Data";
        else if (CurrentMode == mode.Squib_Mispoll) Disp_LCD.text = "?RPT Mispolls";
        else if (CurrentMode == mode.Squib_Dropout) Disp_LCD.text = "?RPT Dropouts";
        else if (CurrentMode == mode.Squib_MispollListing)
        {
            Disp_LCD.text = MispollStrings[currentMispoll];
        }
        else if (CurrentMode == mode.Squib_DropoutListing)
        {
            Disp_LCD.text = DropoutStrings[currentDropout];
        }
        else if (CurrentMode == mode.Squib_Clear) Disp_LCD.text = "?CLR Squib Data";

        else if (CurrentMode == mode.Not_Sim) Disp_LCD.text = "NOT SIMULATED";

    }

    void ProcessTimer()
    {
        if (Switch_Program == 1) screenTimer += Time.deltaTime;
        else if (CurrentMode == mode.BIT1) screenTimer += Time.deltaTime;
        else if (CurrentMode == mode.BIT2) screenTimer += Time.deltaTime;
        else if (CurrentMode == mode.BIT3) screenTimer += Time.deltaTime;
        else if (CurrentMode == mode.Not_Sim) screenTimer += Time.deltaTime;
    }
    void ProcessProgram()
    {
        /*if (Switch_Program == 0)
        {
            if(programTimer > 0.1f)
            {
                Switch_Program = 1;
                programTimer = 0.0f;
            }
            programTimer += Time.deltaTime;
        }*/
        
    }
    
    /// <summary>
    /// Ran only during reset, generates the strings for mispolls/dropouts
    /// </summary>
    void ProcessFlareStrings()
    {
        int i = 0;
        int j = 0;
        MispollStrings[i] = "0" + Mispolls + " Mispolls (M)";
        i++;
        for (i=i;  i < Mispolls + 1; i++)
        {
            MispollStrings[i] = "M" + BadFlarePrefix[Flare_DamagedBucket] + BadFlareLocations[j] + " P24";
            j++;
        }
        i = 0;
        
        DropoutStrings[i] = "0" + Dropouts + " Dropouts (D)";
        i++;
        for (i = i; i < Mispolls + 1; i++)
        {
            DropoutStrings[i] = "D" + BadFlarePrefix[Flare_DamagedBucket] + BadFlareLocations[j] + " P24";
            j++;
        }
    }


    /// <summary>
    /// This processes the cycling of all menus
    /// </summary>
    void ProcessMode()
    {
        // Run the BIT sequence
        if (CurrentMode == mode.BIT1 && screenTimer > 5.0f) ChangeMode(mode.BIT2);                  // Display digit test
        else if (CurrentMode == mode.BIT2 && screenTimer > 1.0f) ChangeMode(mode.BIT3);             // Display blank
        else if (CurrentMode == mode.BIT3 && screenTimer > 5.0f) ChangeMode(mode.BIT4);             // Display menus option
        else if (CurrentMode == mode.BIT4 && screenTimer > 5.0f) ChangeMode(mode.Flare_Count);      // Display Flare Count
        else if (CurrentMode == mode.Not_Sim && screenTimer > 1.0f) ChangeMode(mode.Flare_Count);


        if(twoSelected)
        {
            twoSelected = false;
            if (CurrentMode == mode.BIT4) ChangeMode(mode.Not_Sim);
            else if (CurrentMode == mode.Main_OptFTI) ChangeMode(mode.Not_Sim);
            else if (CurrentMode == mode.Main_OptTRN) ChangeMode(mode.Not_Sim);
            else if (CurrentMode == mode.Main_MaintOp) ChangeMode(mode.Not_Sim);
            else if (CurrentMode == mode.Main_SquibData) ChangeMode(mode.Squib_Data);
            else if (CurrentMode == mode.Squib_Data) ChangeMode(mode.Squib_Mispoll);
            else if (CurrentMode == mode.Squib_Mispoll) ChangeMode(mode.Squib_MispollListing);
            else if (CurrentMode == mode.Squib_Dropout) ChangeMode(mode.Squib_DropoutListing);
            else if (CurrentMode == mode.Main_SystemCheck) ChangeMode(mode.Not_Sim);
            else if (CurrentMode == mode.Squib_Mispoll) ChangeMode(mode.Squib_MispollListing);
            else if (CurrentMode == mode.Squib_Dropout) ChangeMode(mode.Squib_DropoutListing);
            else if (CurrentMode == mode.Squib_Clear) ChangeMode(mode.Main_SquibData);
        }
        // Process diplay mode if bit program is selected
        if (bitSelected && Switch_Program != Switch_Program_Last)
        {
            bitSelected = false;
            if(CurrentMode == mode.BIT4) ChangeMode(mode.Not_Sim);
            else if (CurrentMode == mode.Flare_Count) ChangeMode(mode.Main_OFP);
            else if (CurrentMode == mode.Main_OFP) ChangeMode(mode.Main_StatTRN);
            else if (CurrentMode == mode.Main_StatTRN) ChangeMode(mode.Main_OptFTI);
            else if (CurrentMode == mode.Main_OptFTI) ChangeMode(mode.Main_OptTRN);
            else if (CurrentMode == mode.Main_OptTRN) ChangeMode(mode.Main_MaintOp);
            else if (CurrentMode == mode.Main_MaintOp) ChangeMode(mode.Main_SquibData);
            else if (CurrentMode == mode.Main_SquibData) ChangeMode(mode.Main_SystemCheck);
            else if (CurrentMode == mode.Main_SystemCheck) ChangeMode(mode.Flare_Count);
            else if (CurrentMode == mode.Squib_Data) ChangeMode(mode.Squib_Mispoll);
            else if (CurrentMode == mode.Squib_Mispoll) ChangeMode(mode.Squib_Dropout);
            else if (CurrentMode == mode.Squib_Dropout) ChangeMode(mode.Squib_Clear);
            else if (CurrentMode == mode.Squib_MispollListing)
            {
                if (currentMispoll < Mispolls)
                {
                    currentMispoll++;
                    screenTimer = 0.0f;
                }
                else
                {
                    currentMispoll = 0;
                    ChangeMode(mode.Squib_Mispoll);
                }
            }
            else if (CurrentMode == mode.Squib_DropoutListing)
            {
                if (currentDropout < Dropouts)
                {
                    currentDropout++;
                    screenTimer = 0.0f;
                }
                else
                {
                    currentDropout = 0;
                    ChangeMode(mode.Squib_Dropout);
                }
            }
            else if (CurrentMode == mode.Squib_Clear) ChangeMode(mode.Flare_Count);

        }

        if (screenTimer > 5.0f)
        {
            if (CurrentMode == mode.Main_OFP) ChangeMode(mode.Main_StatTRN);
            else if (CurrentMode == mode.Main_StatTRN) ChangeMode(mode.Main_OptFTI);
            else if (CurrentMode == mode.Main_OptFTI) ChangeMode(mode.Main_OptTRN);
            else if (CurrentMode == mode.Main_OptTRN) ChangeMode(mode.Main_MaintOp);
            else if (CurrentMode == mode.Main_MaintOp) ChangeMode(mode.Main_SquibData);
            else if (CurrentMode == mode.Main_SquibData) ChangeMode(mode.Main_SystemCheck);
            else if (CurrentMode == mode.Main_SystemCheck) ChangeMode(mode.Flare_Count);
            else if (CurrentMode == mode.Squib_Data) ChangeMode(mode.Squib_Mispoll);
            else if (CurrentMode == mode.Squib_Mispoll) ChangeMode(mode.Squib_Dropout);
            else if (CurrentMode == mode.Squib_Dropout) ChangeMode(mode.Squib_Clear);
            else if (CurrentMode == mode.Squib_MispollListing)
            {
                if (currentMispoll < Mispolls)
                {
                    currentMispoll++;
                    screenTimer = 0.0f;
                }
                else
                {
                    currentMispoll = 0;
                    ChangeMode(mode.Squib_Mispoll);
                }
            }
            else if (CurrentMode == mode.Squib_DropoutListing)
            {
                if (currentDropout < Dropouts)
                {
                    currentDropout++;
                    screenTimer = 0.0f;
                }
                else
                {
                    currentDropout = 0;
                    ChangeMode(mode.Squib_Dropout);
                }
            }
            else if (CurrentMode == mode.Squib_Clear) ChangeMode(mode.Flare_Count);
        }

        Switch_Program_Last = Switch_Program;
    }


    void ProcessButtons()
    {
        if (Switch_Mode == 0 || !External_Power)
        {
            Status_NOGO[0].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_NOGO[1].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_GO.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_DispenseReady.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Disp_LCD.text = "";

            ChangeMode(mode.off);
        }
        else if (Switch_Mode == 1 && CurrentMode == mode.off) ChangeMode(mode.BIT1);
        
        if (sw_Jettison.SwitchPosition() && External_Power) failed = 1;         // Jettison switch has 5% chance of being on, defined in Switch_Jettison.cs
        else if (Switch_Mode == 2 && External_Power) failed = 1;
    }

    #region public button functions
    

    public void ExitGame()
    {
        Application.Quit();
    }

    public void TogglePower()
    {
        External_Power = !External_Power;
        if (External_Power)
            {
            Avionics_On.Play();
            Avionics_Off.Stop();
        }
        else
        { 
            Avionics_Off.Play();
            Avionics_On.Stop();

            // Blank out display
            Status_NOGO[0].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_NOGO[1].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Status_GO.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            Disp_LCD.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Disp_LCD.text = "";
        }
    }

    public void readProgramSwitch()
    {
        
        Switch_Program = sw_Program.SwitchPosition();
        if (Switch_Program == 0) bitSelected = true;
        else if (Switch_Program == 2) twoSelected = true;
        else
        {
            bitSelected = false;
            twoSelected = false;
        }
        programTimer = 0.0f;
    }

    public void TogglePrgmBit()
    {
        /*if (Switch_Program == 2) Switch_Program = 1;
        else
        {
            Switch_Program = 0;
            programTimer = 0.0f;
            bitSelected = true;
        }*/
    }

    public void TogglePrgm2()
    {
        /*if (Switch_Program == 1)
        {
            Switch_Program = 2;
            twoSelected = true;
        }
        else Switch_Program = 1;
            
        programTimer = 0.0f;
        */
    }
    public void toggleModeSwitch()
    {
        if (Switch_Mode == 2) Switch_Mode = 0;
        else if (Switch_Mode == 1) Switch_Mode = 0;
        else if (Switch_Mode == 0) Switch_Mode = 1;
    }
    #endregion

    void ChangeMode(mode DesiredMode)
    {
        CurrentMode = DesiredMode;
        screenTimer = 0.0f;
    }

    /// <summary>
    /// Resets the CMDS panel to default settings
    /// </summary>
    public void Reset()
    {
        Flare_DamagedBucket = (int)Random.RandomRange(0.0f, 11.0f);
        Mispolls = (int)Random.RandomRange(2.0f, 7.0f);
        //Dropouts = (int)Random.RandomRange(0.0f, 5.0f);       // Random function disabled at request of Justin Higginbotham, fixed to 0

        int i = Flare_DamagedBucket % 4;

        Flare_Ammo[0] = 40;
        Flare_Ammo[1] = 40;
        Flare_Ammo[2] = 120;
        Flare_Ammo[3] = 40;

        Flare_Ammo[i] -= Mispolls + Dropouts;

        Status_NOGO[0].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Status_NOGO[1].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Status_GO.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Status_DispenseReady.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Disp_LCD.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        Disp_LCD.text = "";

        External_Power = false;

        ProcessFlareStrings();
        Switch_Program = 1;

        Power_on.SetActive(false);
        Power_off.SetActive(true);
        
        if (CoinFlip(0.05f)) Switch_Mode = 2;
        else Switch_Mode = 0;
    }

    #region Common Functions
    /// <summary>
    /// Common coin flip function with 50% odds
    /// </summary>
    /// <returns></returns>
    bool CoinFlip()
    {
        if (Random.Range(0.0f, 1.0f) > 0.5f) return true;
        else return false;
    }

    bool CoinFlip(float odds)
    {
        if (odds < 0.0f || odds > 1.0f)
        {
            Debug.Log("ERROR: Coinflip performed with odds less than 0% (0.0f) or greater than 100% (1.0f)\n" +
                "Value defaulted to 50% (0.5f)");
            odds = 0.5f;
        }
        if (Random.Range(0.0f, 1.0f) < odds) return true;
        else return false;
    }
    #endregion
}
