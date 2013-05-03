using UnityEngine;
using System.Collections;

public class PlayerTwoControls : PlayerControls2
{
	public PlayerTwoControls()
    {
    }

    protected override float LeftPressed()
    {
        return Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;
    }

    protected override float RightPressed()
    {
        return Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;
    }

    protected override bool JumpButtonDown()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    protected override bool JumpButtonUp()
    {
        return Input.GetKeyUp(KeyCode.W);
    }

    protected override bool JumpButton()
    {
        return Input.GetKey(KeyCode.W);
    }
}