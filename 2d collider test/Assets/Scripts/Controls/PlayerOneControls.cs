using UnityEngine;
using System.Collections;

public class PlayerOneControls : PlayerControls2 
{
    public PlayerOneControls()
    {
        this.leftButton = KeyCode.LeftArrow;
        this.rightButton = KeyCode.RightArrow;
        this.jumpButton = KeyCode.UpArrow;
    }
}