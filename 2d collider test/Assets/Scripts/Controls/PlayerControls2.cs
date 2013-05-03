using UnityEngine;
using System.Collections;

public class PlayerControls2 : MonoBehaviour
{

    protected KeyCode leftButton;
    protected KeyCode rightButton;
    protected KeyCode jumpButton;

    //public float minJumpClimb = 25f;
    //public float maxJumpClimb = 75f;

    //public float shortestJumpUpGrav = -3f;
    //public float maxJumpUpGrav = -5f;
    //public float fallGrav = -10f;

    private float lastVel = 0f;
    //public float jumpStartVel = 250f;
    private float cumulativeCurrentJumpHeight = 0f;
    //public float jumpSpeedFactorOnJumpRelease = 3f;
    //public float downwardPush = -50;



    // new jump tweakable variables
    [SerializeField]
    private float minJumpHeight;
    [SerializeField]
    private float maxJumpHeight;
    [SerializeField]
    private float termFallVelHeight;
    [SerializeField]
    private float maxJumpTime;
    [SerializeField]
    private float terminalFallVel;

    private float jumpTime;
    private bool breakJump = false;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float maxRunSpeed;

    public float timeToMaxWalkSpeed;
    public float timeToMaxRunSpeed;
    public float timeToDecelerateToWalk;
    public float timeToDecelerateToStop;

    private enum JUMP_STATE { ON_GROUND, JUMPING_START, JUMPING_UP, FALLING_DOWN_ACCEL, FALLING_DOWN_TERMINAL };
    // stationary - not moving
    // accel walk - could be speed up from stopped or slowing down from running
    // max walk - not running, moving at max walk speed
    // accel run - accelerating from stationary or walk
    // max run - running at max speed
    private enum RUN_STATE { STATIONARY, ACCEL_WALK, MAX_WALK, ACCEL_RUN, MAX_RUN };
    private enum RUN_STATE_TEMP { STATIONARY, LEFT, RIGHT };
    private RUN_STATE_TEMP runState = RUN_STATE_TEMP.STATIONARY;
    // jumping should use a table.  There are two jump heights.

    private Rigidbody body;
    private JUMP_STATE jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;

    // Use this for initialization
    void Awake()
    {
        //this.characterObject = GameObject.Find("Character").gameObject;
        //if (this.characterObject == null)
        //{
        //    Debug.Log("WTF");
        //}
        this.body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //RaycastHit rayHit;
        //float stepMoveDistance = this.moveDistancePerSecond * Time.deltaTime;
        if (this.jumpState == JUMP_STATE.ON_GROUND)
        {
            if (Input.GetKeyDown(this.jumpButton))
            {
                this.jumpState = JUMP_STATE.JUMPING_START;
                this.jumpTime = 0f;
            }
        }
        else
        {
            //if (!Input.GetKey(this.jumpButton))
            //{
            //    Debug.Log("fhdsjkfhlsda");
            //    this.breakJump = true;
            //}
            this.breakJump = !Input.GetKey(this.jumpButton);
        }

        if (Input.GetKeyDown(this.jumpButton) && this.jumpState == JUMP_STATE.ON_GROUND)
        {
            //float initialVelocity = this.maxJumpHeight / this.maxJumpTime + this.jumpAccelUp * Mathf.Pow(this.maxJumpTime, 2) / 2f;
            this.jumpState = JUMP_STATE.JUMPING_START;
            this.jumpTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (this.jumpState == JUMP_STATE.ON_GROUND)
            {
                // duck.  I guess I'll need to see how animating works with mesh colliders
            }
        }
        if (Input.GetKeyUp(this.jumpButton) && this.jumpState != JUMP_STATE.ON_GROUND && this.jumpState != JUMP_STATE.FALLING_DOWN_ACCEL)
        {
            //this.jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;
            //this.lastVel /= this.jumpSpeedFactorOnJumpRelease;
        }

        bool left = Input.GetKey(this.leftButton);
        bool right = Input.GetKey(this.rightButton);

        if (left)
        {
            //if (this.body.SweepTest(new Vector3(-1, 0, 0), out rayHit, stepMoveDistance))
            //{
            //    this.transform.Translate(new Vector3(-rayHit.distance + Utils.MOVE_PADDING, 0, 0));
            //}
            //else
            //{
            //    this.transform.Translate(new Vector3(-stepMoveDistance, 0, 0));
            //}
            this.runState = RUN_STATE_TEMP.LEFT;
        }
        if (right)
        {
            //if (this.body.SweepTest(new Vector3(1, 0, 0), out rayHit, stepMoveDistance))
            //{
            //    this.transform.Translate(new Vector3(rayHit.distance - Utils.MOVE_PADDING, 0, 0));
            //}
            //else
            //{
            //    this.transform.Translate(new Vector3(stepMoveDistance, 0, 0));
            //}
            if (this.runState == RUN_STATE_TEMP.LEFT)
            {
                this.runState = RUN_STATE_TEMP.STATIONARY;
            }
            else
            {
                this.runState = RUN_STATE_TEMP.RIGHT;
            }
        }
        if (!left && !right)
        {
            this.runState = RUN_STATE_TEMP.STATIONARY;
        }

    }

    void FixedUpdate()
    {
        if (this.jumpState != JUMP_STATE.ON_GROUND) 
        {
            this.jumpTime += Time.deltaTime;
        }

        // calculate every update because FUCK YEAH
        float accelDown = -Mathf.Pow(this.terminalFallVel, 2f) / (2f * this.termFallVelHeight);

        // up movement
        float deltaUp = 0f;
        RaycastHit rayHit;

        float newVelocity = 0f;
        float accelUp;

        switch (this.jumpState)
        {
            case JUMP_STATE.ON_GROUND:
                deltaUp = 0f;
                break;
            case JUMP_STATE.JUMPING_START:
                //accelUp = 2f * (this.maxJumpHeight - (this.initialJumpVel * this.maxJumpTime)) / Mathf.Pow(this.maxJumpTime, 2);
                accelUp = -2f * this.maxJumpHeight / this.maxJumpTime / this.maxJumpTime;
                Debug.Log("intial up accel " + accelUp.ToString());
                newVelocity = (this.maxJumpHeight / this.maxJumpTime) - accelUp * this.maxJumpTime / 2f;
                //newVelocity = this.initialJumpVel;
                Debug.Log("my initial velocity is " + newVelocity.ToString());
                deltaUp = newVelocity * Time.deltaTime;
                this.jumpState = JUMP_STATE.JUMPING_UP;
                break;
            case JUMP_STATE.JUMPING_UP:
                //accelUp = 2f * (this.maxJumpHeight - (this.initialJumpVel * this.maxJumpTime)) / Mathf.Pow(this.maxJumpTime, 2);
                accelUp = -2f * this.maxJumpHeight / this.maxJumpTime / this.maxJumpTime;                
                Debug.Log("accel up " + accelUp.ToString());
                newVelocity = this.lastVel + accelUp * Time.deltaTime;
                deltaUp = newVelocity * Time.deltaTime;
                //if (this.breakJump) 
                //{
                //    Debug.Log("11111");
                //}
                //if (newVelocity < 0 || (this.breakJump && this.cumulativeCurrentJumpHeight > this.minJumpHeight))
                if (newVelocity < 0 || this.breakJump)
                {
                    //Debug.Log("2222");
                    this.jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;
                    this.breakJump = false;
                    Debug.Log("The jump took climb " + this.jumpTime + " seconds");
                }
                break;
            case JUMP_STATE.FALLING_DOWN_ACCEL:
                newVelocity = this.lastVel + accelDown * Time.deltaTime;
                if (newVelocity < this.terminalFallVel)
                {
                    newVelocity = this.terminalFallVel;
                    this.jumpState = JUMP_STATE.FALLING_DOWN_TERMINAL;
                }
                deltaUp = newVelocity * Time.deltaTime;
                break;
            case JUMP_STATE.FALLING_DOWN_TERMINAL:
                deltaUp = this.terminalFallVel * Time.deltaTime;
                break;
        }

        this.lastVel = newVelocity;


        float deltaSide = 0f;
        switch (this.runState)
        {
            case RUN_STATE_TEMP.STATIONARY:
                break;
            case RUN_STATE_TEMP.LEFT:
                deltaSide = this.walkSpeed * Time.deltaTime * -1;
                break;
            case RUN_STATE_TEMP.RIGHT:
                deltaSide = this.walkSpeed * Time.deltaTime;
                break;
        }

        // vertical sweep
        //Debug.Log("my delta up is " + deltaUp.ToString());
        //if (this.body.SweepTest(new Vector3(0, 1, 0), out rayHit, deltaUp))
        Vector3 movementVector = new Vector3(deltaSide, deltaUp, 0);
        Vector3 movementVectorNormalized = movementVector.normalized;
        float movementVectorMagnitude = movementVector.magnitude;
        if (this.body.SweepTest(movementVectorNormalized, out rayHit, movementVectorMagnitude))
        {
            // there was a hit... okay, need to handle this!
            // move as far as we can along this vector and then figure out which direction we're blocked in
            this.transform.Translate(movementVectorNormalized * (rayHit.distance - Utils.MOVE_PADDING));

            // need to sweep the remainder of vertcal and horizonal movement
            Vector3 remainingVector = movementVectorNormalized * (movementVectorMagnitude - rayHit.distance);
            if (this.body.SweepTest(new Vector3(1, 0, 0), out rayHit, remainingVector.x))
            {
                // don't bother moving, just make the sideways accel (none right now) 0 and handle vertical
            }
            else
            {
                this.transform.Translate(new Vector3(remainingVector.x, 0, 0));
            }

            if (this.body.SweepTest(new Vector3(0, 1, 0), out rayHit, remainingVector.y))
            {
                if (deltaUp >= 0)
                {
                    this.jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;
                    this.lastVel = 0f;
                }
                else
                {
                    Debug.Log("There was a hit and I am moving: " + rayHit.distance, gameObject);
                    this.jumpState = JUMP_STATE.ON_GROUND;
                }
            }
            else
            {
                this.transform.Translate(new Vector3(0, remainingVector.y, 0));
                this.cumulativeCurrentJumpHeight += remainingVector.y;
            }
        }
        else
        {
            if (this.jumpState == JUMP_STATE.ON_GROUND && this.body.SweepTest(new Vector3(0, -1, 0), out rayHit, 0.1f))
            {
                this.jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;
                this.lastVel = 0f;
            }
            this.transform.Translate(movementVector);
        }
    }
}
