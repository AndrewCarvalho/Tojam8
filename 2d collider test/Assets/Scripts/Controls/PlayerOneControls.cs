using UnityEngine;
using System.Collections;

public class PlayerOneControls : PlayerControls2 
{
    public PlayerOneControls()
    {
    }

    protected override float LeftPressed()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal1") < 0 ? 1.0f : 0.0f;
    }

    protected override float RightPressed()
    {
        return Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal1") > 0 ? 1.0f : 0.0f;
    }

    protected override bool JumpButtonDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button0);
    }

    protected override bool JumpButtonUp()
    {
        return Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Joystick1Button0);
    }

    protected override bool JumpButton()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Joystick1Button0);
    }
}