using UnityEngine;
using System.Collections;

public class PlayerTwoControls : PlayerControls2
{
	public PlayerTwoControls()
    {
    }

    protected override float LeftPressed()
    {
        return Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal2") < 0 ? 1.0f : 0.0f;
    }

    protected override float RightPressed()
    {
        return Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal2") > 0 ? 1.0f : 0.0f;
    }

    protected override bool JumpButtonDown()
    {
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Joystick3Button0);
    }

    protected override bool JumpButtonUp()
    {
        return Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Joystick3Button0);
    }

    protected override bool JumpButton()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Joystick3Button0);
    }
}