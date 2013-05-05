using UnityEngine;
using System.Collections;

public class PlayerOneControls : PlayerControls2 
{
    public PlayerOneControls()
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

    protected override bool DownPressed()
    {
        return Input.GetKey(KeyCode.S);
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

    protected override bool ActionButtonDown()
    {
        return Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick3Button1);
    }

    protected override Vector3 BlockThrowDirection()
    {
        return new Vector3(0.0f, -1.0f, 0.0f);
    }

    protected override PlayerCamera CameraFollowingMe()
    {
        return FindObjectOfType(typeof(PlayerOneCamera)) as PlayerCamera;
    }

    protected override PlayerCamera OtherPlayerCamera()
    {
        return FindObjectOfType(typeof(PlayerTwoCamera)) as PlayerCamera;
    }
}