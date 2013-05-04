using UnityEngine;
using System.Collections;

public class Knight : Enemy {
    protected float confusedTimestamp = 0.0f;
    protected bool confused = false;
    protected float direction = 0.0f;
    protected PlayerControls2 chasingPlayer;

    [SerializeField]
    private float confuseDuration = 5.0f;

	// Use this for initialization
	new void Start()
    {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update()
    {
        base.Update();
        if (confused)
        {
            float difference = Time.realtimeSinceStartup - confusedTimestamp;
            if (difference > confuseDuration)
            {
                confused = false;

                if (this.chasingPlayer)
                {
                    PlayerControls2 player = chasingPlayer;
                    this.chasingPlayer = null;
                    PlayerEnteredSight(player);
                }
            }
        }
        else if (chasingPlayer)
        {
            PlayerControls2 player = chasingPlayer;
            bool chasingLeft = direction < 0.0;
            if ((chasingLeft && player.transform.position.x > transform.position.x) || (!chasingLeft && player.transform.position.x < transform.position.x))
            {
                confusedTimestamp = Time.realtimeSinceStartup;
                confused = true;

                Run(0.0f);
            }
        }
	}

    void PlayerEnteredSight(PlayerControls2 player)
    {
        if (this.chasingPlayer == null)
        {
            this.chasingPlayer = player;
            this.direction = player.transform.position.x < transform.position.x ? -1.0f : 1.0f;

            if(confused == false)
                Run(this.direction);
        }
    }

    void PlayerExitedSight(PlayerControls2 player)
    {
        if (confused && chasingPlayer == player)
            chasingPlayer = null;
    }
}
