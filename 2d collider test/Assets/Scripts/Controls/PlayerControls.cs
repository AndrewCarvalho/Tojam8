using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    public float minJumpClimb = 25f;
    public float maxJumpClimb = 75f;

    public float shortestJumpUpGrav = -3f;
    public float maxJumpUpGrav = -5f;
    public float fallGrav = -10f;

    public float lastVel = 0f;
    public float jumpStartVel = 250f;
    public float cumulativeCurrentJumpHeight = 0f;
    public float jumpSpeedFactorOnJumpRelease = 3f;
    public float downwardPush = -50;

    public float maxRunSpeed = 10f;

    private enum JUMP_STATE { ON_GROUND, JUMPING_UP_FIXED, JUMPING_UP_VAR, FALLING_DOWN };
    // stationary - not moving
    // accel walk - could be speed up from stopped or slowing down from running
    // max walk - not running, moving at max walk speed
    // accel run - accelerating from stationary or walk
    // max run - running at max speed
    private enum RUN_STATE { STATIONARY, ACCEL_WALK, MAX_WALK, ACCEL_RUN, MAX_RUN };
    // jumping should use a table.  There are two jump heights.

    private Rigidbody body;
    private JUMP_STATE jumpState = JUMP_STATE.FALLING_DOWN;

	// Use this for initialization
	void Start () 
    {
        //this.characterObject = GameObject.Find("Character").gameObject;
        //if (this.characterObject == null)
        //{
        //    Debug.Log("WTF");
        //}
        this.body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        //RaycastHit rayHit;
        //float stepMoveDistance = this.moveDistancePerSecond * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.jumpState = JUMP_STATE.JUMPING_UP_FIXED;
            this.cumulativeCurrentJumpHeight = 0f;
            this.lastVel = this.jumpStartVel;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (this.jumpState == JUMP_STATE.ON_GROUND)
            {
                // duck.  I guess I'll need to see how animating works with mesh colliders
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && this.jumpState != JUMP_STATE.ON_GROUND && this.jumpState != JUMP_STATE.FALLING_DOWN)
        {
            this.jumpState = JUMP_STATE.FALLING_DOWN;
            this.lastVel /= this.jumpSpeedFactorOnJumpRelease;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //if (this.body.SweepTest(new Vector3(-1, 0, 0), out rayHit, stepMoveDistance))
            //{
            //    this.transform.Translate(new Vector3(-rayHit.distance + Utils.MOVE_PADDING, 0, 0));
            //}
            //else
            //{
            //    this.transform.Translate(new Vector3(-stepMoveDistance, 0, 0));
            //}
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //if (this.body.SweepTest(new Vector3(1, 0, 0), out rayHit, stepMoveDistance))
            //{
            //    this.transform.Translate(new Vector3(rayHit.distance - Utils.MOVE_PADDING, 0, 0));
            //}
            //else
            //{
            //    this.transform.Translate(new Vector3(stepMoveDistance, 0, 0));
            //}
        }

	}

    void FixedUpdate()
    {
        // v = at
        // d = vt
        // d = at^2
        // d= v1t * (0.5)at^2

        // up movement
        float deltaUp = 0f;
        float lastVelBeforeUpdate = this.lastVel;
        RaycastHit rayHit;

        switch (this.jumpState)
        {
            case JUMP_STATE.JUMPING_UP_FIXED:
                if (this.cumulativeCurrentJumpHeight > this.minJumpClimb)
                {
                    this.jumpState = JUMP_STATE.JUMPING_UP_VAR;
                }
                break;
            case JUMP_STATE.JUMPING_UP_VAR:
                if (this.cumulativeCurrentJumpHeight > this.maxJumpClimb)
                {
                    this.jumpState = JUMP_STATE.FALLING_DOWN;
                    this.lastVel /= this.jumpSpeedFactorOnJumpRelease;
                }
                break;
        }

        switch (this.jumpState)
        {
            case JUMP_STATE.ON_GROUND:
                deltaUp = 0f;
                break;
            case JUMP_STATE.JUMPING_UP_FIXED:
                Debug.Log("111111");
                deltaUp = this.lastVel * Time.deltaTime + this.shortestJumpUpGrav * Time.deltaTime * Time.deltaTime / 2f;
                this.lastVel = deltaUp / Time.deltaTime;
                break;
            case JUMP_STATE.JUMPING_UP_VAR:
                Debug.Log("222222");
                deltaUp = this.lastVel * Time.deltaTime + this.maxJumpUpGrav * Time.deltaTime * Time.deltaTime / 2f;
                this.lastVel = deltaUp / Time.deltaTime;
                break;
            case JUMP_STATE.FALLING_DOWN:
                Debug.Log("333333");
                deltaUp = this.lastVel * Time.deltaTime + this.fallGrav * Time.deltaTime * Time.deltaTime / 2f;
                this.lastVel = deltaUp / Time.deltaTime;
                break;
        }

        if (lastVelBeforeUpdate > 0 && this.lastVel < 0)
        {
            // PUSH THIS FUCKER DOWN
            this.lastVel = this.downwardPush;
        }

        //Debug.Log("the delta up is: " + deltaUp);
        //Debug.Log("The position is: " + this.transform.position.y);

        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    if (this.body.SweepTest(new Vector3(-1, 0, 0), out rayHit, -this.maxRunSpeed * Time.deltaTime))
        //    {
        //        this.transform.Translate(new Vector3(-rayHit.distance + Utils.MOVE_PADDING, 0, 0));
        //    }
        //    else
        //    {
        //        this.transform.Translate(new Vector3(-this.maxRunSpeed * Time.deltaTime, 0, 0));
        //    }
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    if (this.body.SweepTest(new Vector3(1, 0, 0), out rayHit, this.maxRunSpeed * Time.deltaTime))
        //    {
        //        this.transform.Translate(new Vector3(rayHit.distance - Utils.MOVE_PADDING, 0, 0));
        //    }
        //    else
        //    {
        //        this.transform.Translate(new Vector3(this.maxRunSpeed * Time.deltaTime, 0, 0));
        //    }
        //}

        // vertical sweep
        if (this.body.SweepTest(new Vector3(0, 1, 0), out rayHit, deltaUp))
        {
            this.transform.Translate(new Vector3(0, rayHit.distance - Utils.MOVE_PADDING, 0));
            Debug.Log("There was a hit and I am moving: " + rayHit.distance);
            this.jumpState = JUMP_STATE.ON_GROUND;
        }
        else
        {
            this.transform.Translate(new Vector3(0, deltaUp, 0));
            this.cumulativeCurrentJumpHeight += deltaUp;
        }
    }
}
