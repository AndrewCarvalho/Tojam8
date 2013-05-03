using UnityEngine;
using System.Collections;

public class PlayerOneCamera : PlayerCamera 
{
    protected override void OnAwake()
    {
        this.player = GameObject.Find("Player1");
        this.direction = new Vector3(1, 0, 0);
    }
}