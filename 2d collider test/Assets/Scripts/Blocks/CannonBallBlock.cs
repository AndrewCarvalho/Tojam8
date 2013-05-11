using UnityEngine;
using System.Collections;

public class CannonBallBlock : ThrowBlock
{
    public PlayerControls2 ignorePlayer = null;

	// Use this for initialization
    protected void Start()
    {
        base.Start();
        shouldStopInFirstEmptySpace = false;
		damageMultiplier = 4.0f;
	}
	
	// Update is called once per frame
    protected void Update()
    {
        base.Update();
	}

    protected void FixedUpdate()
    {
        base.FixedUpdate();
        Collider collidedWith = collidedWithSomething(true);
        if (collidedWith)
        {
            Debug.Log("colliding with " + collidedWith.name);
            PlayerControls2 player = collidedWith.GetComponent<PlayerControls2>();
            if (player && ignorePlayer != player)
            {
                player.knockBackByBlock(true, damageMultiplier);
            }
        }
    }


}
