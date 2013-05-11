using UnityEngine;
using System.Collections;

public abstract class PlayerControls2 : Actor
{
    [SerializeField]
    float actionableDistance = 5.0f;

    [SerializeField]
    bool actionIsGrab = false;

    GameManager gameManager;

    protected float facingDirection = 1.0f;
    protected float hurtCountdown = 0.0f;
    protected float dodgeRecoverCountdown = 0.0f;

    public bool dodging = false;

    float doingActionCountDown = 0.0f;
    string actionName;

    // Use this for initialization
    new protected void Awake()
    {
        base.Awake();
        this.body = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    protected abstract float LeftPressed();
    protected abstract float RightPressed();
    protected abstract bool DownPressed();
    protected abstract bool JumpButtonDown();
    protected abstract bool JumpButtonUp();
    protected abstract bool JumpButton();
    protected abstract bool ActionButtonDown();
    protected abstract Vector3 BlockThrowDirection();
    protected abstract PlayerCamera CameraFollowingMe();
    protected abstract PlayerCamera OtherPlayerCamera();

    protected override bool canDodge(GameObject dodgee)
    {
        return dodging && dodgee.GetComponent<PlayerControls2>();
    }

    public void knockBackByBlock(bool hurt, float hurtMultiplier)
    {
        base.knockToNearestTile(-facingDirection);

        if (hurt)
        {
            Hurt(hurtMultiplier);
        }
    }

    public void Hurt(float multiplier)
    {
		Debug.Log ("multiplier " + multiplier);
        Run(0.0f);
        PlayAnimation("Hit");
        hurtCountdown = getAnimationDuration("Hit") * multiplier;
    }

    override public void onHitCollider(Collider collider)
    {
		SpikeBlock spikes = collider.GetComponent<SpikeBlock>();
        if (spikes)
        {
            knockBackByBlock(true, spikes.damageMultiplier);
        }
    }

    // Update is called once per frame
    void Update()
    {
        doingActionCountDown -= Time.deltaTime;
        dodgeRecoverCountdown -= Time.deltaTime;

        if (hurtCountdown > 0.0f && hurtCountdown - Time.deltaTime < 0)
        {
            Run(0.0f);
        }
        hurtCountdown -= Time.deltaTime;

        if ((doingActionCountDown > 0 && dodging == false) || hurtCountdown > 0 || dodgeRecoverCountdown > 0)
        {
            return; // in the middle of an animation. Do nothing
        }
        else if (dodging && doingActionCountDown < 0)
        {
            dodging = false;
            Run(0.0f);

            dodgeRecoverCountdown =  getAnimationDuration("Dizzy");
            PlayAnimation("Dizzy");
            return;
        }

        if (this.jumpState == JUMP_STATE.ON_GROUND)
        {
            if (JumpButtonDown())
            {
                base.JumpStart();
            }
        }
        else
        {
            if (JumpButtonUp())
                BreakJump();
        }

        if (JumpButton() && this.jumpState == JUMP_STATE.ON_GROUND)
        {
            base.JumpStart();
        }

        bool left = (LeftPressed() != 0.0f);
        bool right = (RightPressed() != 0.0f);

        if (left)
        {
            if (facingDirection != -1.0f)
            {
                facingDirection = -1.0f;
                FlipAnimationX();
            }
            Run(facingDirection, DownPressed(), !dodging);
        }
        if (right)
        {
            if (this.runState == RUN_STATE_TEMP.LEFT)
            {
                base.Run(0.0f, DownPressed(), !dodging);
            }
            else
            {
                if (facingDirection != 1.0f)
                {
                    facingDirection = 1.0f;
                    FlipAnimationX();
                }

                Run(facingDirection, DownPressed(), !dodging);
            }
        }
        if (!left && !right)
        {
            Run(0.0f, DownPressed(), !dodging);
        }

        if (Application.loadedLevelName != "WinScene" && ActionButtonDown() && dodging == false)
        {
            string animationName = actionAnimationName;
            bool hitBlock = false;

            {
                ThrowBlock block = closestThrowBlock();
                if (block)
                {
                    block.Throw(BlockThrowDirection(), CameraFollowingMe(), OtherPlayerCamera(), 0.2f);
                    if (block.GetComponent<CannonBallBlock>())
                        block.GetComponent<CannonBallBlock>().ignorePlayer = this;

                    hitBlock = true;
                }
            }

            if (hitBlock)
            {
                animationName = actionAnimationName;
                Run(0.0f, DownPressed(), !dodging);
            }
            else if (GameManager.isSingleScreen())
            {
                if (action2AnimationName == "Dodge")
                {
                    dodging = true;
                }
                else
                {
                    RaycastHit[] hits = castForward(new Vector3(facingDirection, 0.0f, 0.0f), actionableDistance / 3f);
                    foreach (RaycastHit hit in hits)
                    {
                        PlayerControls2 otherPlayer = hit.collider.GetComponent<PlayerControls2>();
                        if (otherPlayer && otherPlayer.dodging == false)
                        {
                            //gameManager.notifyPlayerWin(this);
                            GameManager.KnightWinGame();
                            break;
                        }
                    }

                    Run(0.0f);
                }
                animationName = action2AnimationName;
            }
            else
            {
                Run(0.0f, DownPressed(), !dodging);
            }

            doingActionCountDown = getAnimationDuration(animationName) - 0.1f;
            PlayAnimation(animationName);
        }
    }
	
	ThrowBlock closestThrowBlock()
	{
		float closestDistance = Mathf.Infinity;
		ThrowBlock closest = null;
		RaycastHit[] hits = castForward(new Vector3(facingDirection, 0.0f, 0.0f), actionableDistance);
        foreach (RaycastHit hit in hits)
        {
            ThrowBlock block = hit.collider.GetComponent<ThrowBlock>();
            if (block)
            {
				float distance = (transform.position - block.transform.position).magnitude;
				if(distance < closestDistance)
				{
					closestDistance = distance;
					closest = block;
				}
            }
        }
		
		return closest;
	}
}
