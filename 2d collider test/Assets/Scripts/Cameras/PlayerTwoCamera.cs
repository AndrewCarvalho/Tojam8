using UnityEngine;
using System.Collections;

public class PlayerTwoCamera : PlayerCamera
{
    protected override void OnAwake()
    {
        this.player = GameObject.Find("Player2");
        this.direction = new Vector3(-1, 0, 0);
    }
}