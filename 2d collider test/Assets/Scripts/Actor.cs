using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

    private float lastVel = 0f;
    private float cumulativeCurrentJumpHeight = 0f;

    // new jump tweakable variables
    [SerializeField]
    private float minJumpHeight = 1.0f;
    [SerializeField]
    private float maxJumpHeight = 3;
    [SerializeField]
    private float termFallVelHeight = 5;
    [SerializeField]
    private float maxJumpTime = 0.5f;
    [SerializeField]
    private float terminalFallVel = -25;

    private float jumpTime;
    private bool breakJump = false;

    [SerializeField]
    private float walkSpeed = 0;
    [SerializeField]
    private float maxRunSpeed;

    public float timeToMaxWalkSpeed;
    public float timeToMaxRunSpeed;
    public float timeToDecelerateToWalk;
    public float timeToDecelerateToStop;

    protected enum JUMP_STATE { ON_GROUND, JUMPING_START, JUMPING_UP, FALLING_DOWN_ACCEL, FALLING_DOWN_TERMINAL, DISABLED };
    // stationary - not moving
    // accel walk - could be speed up from stopped or slowing down from running
    // max walk - not running, moving at max walk speed
    // accel run - accelerating from stationary or walk
    // max run - running at max speed
    //private enum RUN_STATE { STATIONARY, ACCEL_WALK, MAX_WALK, ACCEL_RUN, MAX_RUN };
    protected enum RUN_STATE_TEMP { STATIONARY, LEFT, RIGHT };
    protected RUN_STATE_TEMP runState = RUN_STATE_TEMP.STATIONARY;
    // jumping should use a table.  There are two jump heights.

    protected Rigidbody body;
    protected JUMP_STATE jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;

    // Use this for initialization
    protected void Awake()
    {
        this.body = GetComponent<Rigidbody>();
    }

    protected void JumpStart()
    {
        this.jumpState = JUMP_STATE.JUMPING_START;
        this.jumpTime = 0f;
        this.breakJump = false;
    }

    protected void BreakJump()
    {
        this.breakJump = true;
    }

    protected void DisableActor()
    {
        this.jumpState = JUMP_STATE.DISABLED;
    }

    protected RaycastHit[] castForward(Vector3 direction, float distance = Mathf.Infinity, int mask = Physics.kDefaultRaycastLayers)
    {
        BoxCollider box = GetComponent<BoxCollider>();
        Vector3 origin = transform.position;
        origin.y -= box.size.y / 4.0f;
        return Physics.RaycastAll(origin, direction, distance, mask);
    }

    protected void Run(float right)
    {
        if (right == 0.0f)
        {
            this.runState = RUN_STATE_TEMP.STATIONARY;
        }
        else if (right < 0.0f)
        {
            this.runState = RUN_STATE_TEMP.LEFT;
        }
        else
        {
            this.runState = RUN_STATE_TEMP.RIGHT;
        }
    }

    protected void FixedUpdate()
    {
        // DEBUG STUFF
        string objectName = this.name;
        //Debug.Log();

        if (jumpState == JUMP_STATE.DISABLED)
            return;

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
                accelUp = -2f * this.maxJumpHeight / this.maxJumpTime / this.maxJumpTime;
                newVelocity = (this.maxJumpHeight / this.maxJumpTime) - accelUp * this.maxJumpTime / 2f;
                deltaUp = newVelocity * Time.deltaTime;
                this.jumpState = JUMP_STATE.JUMPING_UP;
                break;
            case JUMP_STATE.JUMPING_UP:
                accelUp = -2f * this.maxJumpHeight / this.maxJumpTime / this.maxJumpTime;
                newVelocity = this.lastVel + accelUp * Time.deltaTime;
                deltaUp = newVelocity * Time.deltaTime;
                if (newVelocity < 0 || this.breakJump)
                {
                    this.jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;
                    this.breakJump = false;
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
            //if (this.body.SweepTest(new Vector3(remainingVector.x > 0 ? 1.0f : -1.0f, 0, 0), out rayHit, Mathf.Abs(remainingVector.x)))
            if (this.body.SweepTest(new Vector3(1, 0, 0), out rayHit, deltaSide))
            {
                //this.transform.Translate(new Vector3(rayHit.distance - Utils.MOVE_PADDING * deltaSide > 0 ? 1 : -1, 0, 0));
                float partialMoveXDist = rayHit.distance - Utils.MOVE_PADDING * Mathf.Sign(rayHit.distance);
                this.transform.Translate(new Vector3(partialMoveXDist, 0, 0));
                //if (remainingVector.x < 0)
                if (deltaSide < 0)
                    onHitLeft(rayHit.collider);
                else
                    onHitRight(rayHit.collider);

                Object[] colliders = FindObjectsOfType(typeof(Collider));
                foreach (Object colliderObject in colliders)
                {
                    Collider collider = colliderObject as Collider;
                    if (collider != this.collider && !collider.isTrigger && this.collider.bounds.Intersects(collider.bounds))
                    {
                        this.transform.Translate(new Vector3(-partialMoveXDist, 0, 0));
                    }
                }
            }
            else
            {
                //this.transform.Translate(new Vector3(remainingVector.x, 0, 0));
                this.transform.Translate(new Vector3(deltaSide, 0, 0));

                // yes no maybe?
                Object[] colliders = FindObjectsOfType(typeof(Collider));
                foreach (Object colliderObject in colliders)
                {
                    Collider collider = colliderObject as Collider;
                    if (collider != this.collider && !collider.isTrigger && this.collider.bounds.Intersects(collider.bounds))
                    {
                        //this.transform.Translate(new Vector3(-remainingVector.x, 0, 0));
                        this.transform.Translate(new Vector3(-deltaSide, 0, 0));
                    }
                }
            }

            //if (this.body.SweepTest(new Vector3(0, remainingVector.y > 0 ? 1.0f : -1.0f, 0), out rayHit, Mathf.Abs(remainingVector.y)))
            if (this.body.SweepTest(new Vector3(0, 1, 0), out rayHit, deltaUp))
            {
                float partialMoveYDist = rayHit.distance - Utils.MOVE_PADDING * Mathf.Sign(rayHit.distance);
                this.transform.Translate(new Vector3(0, partialMoveYDist, 0));
                if (deltaUp >= 0)
                {
                    this.jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;
                    this.lastVel = 0f;
                }
                else
                {
                    this.jumpState = JUMP_STATE.ON_GROUND;
                }

                // yes no maybe?
                Object[] colliders = FindObjectsOfType(typeof(Collider));
                foreach (Object colliderObject in colliders)
                {
                    Collider collider = colliderObject as Collider;
                    if (collider != this.collider && !collider.isTrigger && this.collider.bounds.Intersects(collider.bounds))
                    {
                        this.transform.Translate(new Vector3(0, -partialMoveYDist, 0));
                        this.cumulativeCurrentJumpHeight -= partialMoveYDist;
                    }
                }
            }
            else
            {
                //this.transform.Translate(new Vector3(0, remainingVector.y, 0));
                this.transform.Translate(new Vector3(0, deltaUp, 0));
                //this.cumulativeCurrentJumpHeight += remainingVector.y;
                this.cumulativeCurrentJumpHeight += deltaUp;

                // yes no maybe?
                Object[] colliders = FindObjectsOfType(typeof(Collider));
                foreach (Object colliderObject in colliders)
                {
                    Collider collider = colliderObject as Collider;
                    if (collider != this.collider && !collider.isTrigger && this.collider.bounds.Intersects(collider.bounds))
                    {
                        this.transform.Translate(new Vector3(0, -remainingVector.y, 0));
                        this.cumulativeCurrentJumpHeight -= remainingVector.y;
                    }
                }
            }
        }
        else
        {
            if (this.jumpState == JUMP_STATE.ON_GROUND && !this.body.SweepTest(new Vector3(0, -1, 0), out rayHit, 0.1f))
            {
                this.jumpState = JUMP_STATE.FALLING_DOWN_ACCEL;
                this.lastVel = 0f;
            }
            this.transform.Translate(movementVector);

            // yes no maybe?
            Object[] colliders = FindObjectsOfType(typeof(Collider));
            foreach (Object colliderObject in colliders)
            {
                Collider collider = colliderObject as Collider;
                if (collider != this.collider && !collider.isTrigger && this.collider.bounds.Intersects(collider.bounds))
                {
                    this.transform.Translate(-movementVector);
                }
            }
        }
    }

    public virtual void onWalkedOffEdge(string side)
    {
    }

    public virtual void onHitLeft(Collider other)
    {
    }

    public virtual void onHitRight(Collider other)
    {
    }
}
