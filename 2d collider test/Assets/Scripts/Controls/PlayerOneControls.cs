using UnityEngine;
using System.Collections;

public class PlayerOneControls : PlayerControls2 
{
    public PlayerOneControls()
    {
    }

    protected override float LeftPressed()
    {
        return Input.GetKey(KeyCode.LeftArrow) ? 1.0f : 0.0f;
    }

    protected override float RightPressed()
    {
        return Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f;
    }

    protected override bool JumpButtonDown()
    {
        return Input.GetKey(KeyCode.UpArrow);
    }

    protected override bool JumpButtonUp()
    {
        return Input.GetKeyUp(KeyCode.UpArrow); 
    }

    protected override bool JumpButton()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }
}