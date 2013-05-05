using UnityEngine;
using System.Collections;

public class CannonBallBlock : ThrowBlock
{

	// Use this for initialization
    protected void Start()
    {
        base.Start();
        shouldStopInFirstEmptySpace = false;
	}
	
	// Update is called once per frame
    protected void Update()
    {
        base.Update();
	}


}
