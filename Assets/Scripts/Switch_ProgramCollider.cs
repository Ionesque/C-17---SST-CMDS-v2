using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_ProgramCollider : MonoBehaviour
{
    public bool isSwitchClockwise = false;
    bool inBitPosition = false;

    public Switch_Program switch_Program;

    private void OnMouseDown()
    {
        if(!isSwitchClockwise)
        {
            TurnCCW();
        }
        else
        {
            TurnCW();
        }
    }

    private void OnMouseUp()
    {
        
        if(inBitPosition) switch_Program.SetPosition(1);
        inBitPosition = false;
    }

    private void TurnCCW()
    { 
        if(switch_Program.SwitchPosition() == 1)
        {
            switch_Program.SetPosition(0);
            inBitPosition = true;
        }
        else if(switch_Program.SwitchPosition() == 2)
        {
            switch_Program.SetPosition(1);
            inBitPosition = false;
        }
    }

    private void TurnCW()
    {
        if (switch_Program.SwitchPosition() == 1)       // Other positions not simulated, not needed
        {
            switch_Program.SetPosition(2);
        }
    }
}
