using UnityEngine;
using System.Collections;

public abstract class PlayerControls2 : Actor
{
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
            Run(-1.0f);
        }
        if (right)
        {
            if (this.runState == RUN_STATE_TEMP.LEFT)
            {
                base.Run(0.0f);
            }
            else
            {
                base.Run(1.0f);
            }
        }
        if (!left && !right)
        {
            base.Run(0.0f);
        }
    }
}
