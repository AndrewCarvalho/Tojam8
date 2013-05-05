using UnityEngine;
using System.Collections;

public abstract class PlayerControls2 : Actor
{
    [SerializeField]
    float actionableDistance = 5.0f;

    protected float facingDirection = 1.0f;
    protected float hurtCountdown = 0.0f;

    float doingActionCountDown = 0.0f;

    // Use this for initialization
    new protected void Awake()
    {
        base.Awake();
        this.body = GetComponent<Rigidbody>();
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

    public void knockBackByBlock(bool hurt)
    {
        base.knockToNearestTile(-facingDirection);

        if (hurt)
        {
            Hurt();
        }
    }

    public void Hurt()
    {
        PlayAnimation("Hit");
        hurtCountdown = getAnimationDuration("Hit");
    }

    // Update is called once per frame
    void Update()
    {
        doingActionCountDown -= Time.deltaTime;
        hurtCountdown -= Time.deltaTime;
        if (doingActionCountDown > 0 || hurtCountdown > 0)
        {
            return; // in the middle of an animation. Do nothing
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
            Run(facingDirection, DownPressed());
        }
        if (right)
        {
            if (this.runState == RUN_STATE_TEMP.LEFT)
            {
                base.Run(0.0f);
            }
            else
            {
                if (facingDirection != 1.0f)
                {
                    facingDirection = 1.0f;
                    FlipAnimationX();
                }

                Run(facingDirection, DownPressed());
            }
        }
        if (!left && !right)
        {
            Run(0.0f, DownPressed());
        }

        if (ActionButtonDown())
        {
            RaycastHit[] hits = castForward(new Vector3(facingDirection, 0.0f, 0.0f), actionableDistance);
            foreach (RaycastHit hit in hits)
            {
                ThrowBlock block = hit.collider.GetComponent<ThrowBlock>();
                if (block)
                {
                    block.Throw(BlockThrowDirection(), CameraFollowingMe(), OtherPlayerCamera());
                    break;
                }
            }

            Run(0.0f);
            doingActionCountDown = getAnimationDuration(actionAnimationName) - 0.1f;
            PlayAnimation(actionAnimationName);
        }
    }
}
