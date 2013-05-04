using UnityEngine;
using System.Collections;

public abstract class PlayerControls2 : Actor
{
    [SerializeField]
    float actionableDistance = 5.0f;

    protected float facingDirection = 1.0f;

    // Use this for initialization
    new protected void Awake()
    {
        base.Awake();
        this.body = GetComponent<Rigidbody>();
    }

    protected abstract float LeftPressed();
    protected abstract float RightPressed();
    protected abstract bool JumpButtonDown();
    protected abstract bool JumpButtonUp();
    protected abstract bool JumpButton();
    protected abstract bool ActionButtonDown();
    protected abstract Vector3 BlockThrowDirection();
    protected abstract Camera CameraFollowingMe();

    // Update is called once per frame
    void Update()
    {
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
            facingDirection = -1.0f;
            Run(facingDirection);
        }
        if (right)
        {
            if (this.runState == RUN_STATE_TEMP.LEFT)
            {
                base.Run(0.0f);
            }
            else
            {
                facingDirection = 1.0f;
                base.Run(facingDirection);
            }
        }
        if (!left && !right)
        {
            base.Run(0.0f);
        }

        if (ActionButtonDown())
        {
            RaycastHit[] hits = castForward(new Vector3(facingDirection, 0.0f, 0.0f), actionableDistance);
            foreach(RaycastHit hit in hits)
            {
                ThrowBlock block = hit.collider.GetComponent<ThrowBlock>();
                if (block)
                {
                    block.Throw(BlockThrowDirection(), CameraFollowingMe());
                    break;
                }
            }
        }
    }
}
