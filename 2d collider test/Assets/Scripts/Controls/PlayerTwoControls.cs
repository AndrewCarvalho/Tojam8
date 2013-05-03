using UnityEngine;
using System.Collections;

public class PlayerTwoControls : PlayerControls2
{
	public PlayerTwoControls()
    {
        this.leftButton = KeyCode.A;
        this.rightButton = KeyCode.D;
        this.jumpButton = KeyCode.W;
    }

}