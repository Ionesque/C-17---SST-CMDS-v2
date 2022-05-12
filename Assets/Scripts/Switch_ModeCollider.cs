using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_ModeCollider : MonoBehaviour
{
    public bool isSwitchClockwise = false;
    bool inBitPosition = false;


    public Switch_Mode switch_Mode;

    private void OnMouseDown()
    {
        if (!isSwitchClockwise)
        {
            TurnCCW();
        }
        else
        {
            TurnCW();
        }
    }

    private void TurnCCW()
    {
        if (switch_Mode.SwitchPosition() > 0)
        {
            switch_Mode.Decrease();
        }
    }

    private void TurnCW()
    {
        if (switch_Mode.SwitchPosition() < 4)       // Other positions not simulated, not needed
        {
            switch_Mode.Increase();
        }
    }
}
