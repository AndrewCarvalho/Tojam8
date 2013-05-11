using UnityEngine;
using System.Collections;

public class ThrowBlock : Actor {

    [SerializeField]
    private float blockThrowSpeed = 20.0f;

    [SerializeField]
    private string throwAnimation = null;
	
    protected Vector3? floatDirection = null;
    Camera originCamera = null;
    Camera destinationCamera = null;
    bool passedThroughSomething = false;
	
	public float damageMultiplier = 1.0f;

    protected float throwDelayCountdown = 0.0f;
    protected Vector3 delayedDirection;
    protected PlayerCamera delayedCamera;
    protected PlayerCamera delayedOtherCamera;

    protected bool shouldStopInFirstEmptySpace = true;

    enum TransitionState { NOT_TRANSITIONING, TRANSITIONING_FROM_BOTTOM, TRANSITIONING_FROM_TOP, LOOKING_FOR_FREE_SPACE_GOING_UP, LOOKING_FOR_FREE_SPACE_GOING_DOWN, FLOATING_AWAY };
	[SerializeField]
    TransitionState transitionState = TransitionState.NOT_TRANSITIONING;

	// Use this for initialization
	protected void Start () 
    {
		
	}
	
	// Update is called once per frame
    protected void Update() 
    {
        
	}

    protected virtual void onHitGround()
    {

    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
		
		if (throwDelayCountdown > 0.0f && throwDelayCountdown - Time.deltaTime < 0)
        {
            Throw(delayedDirection, delayedCamera, delayedOtherCamera, 0);
        }
        throwDelayCountdown -= Time.deltaTime;
		
        if (floatDirection != null)
        {
            Vector3 moveDelta = new Vector3(floatDirection.Value.x * Time.deltaTime, floatDirection.Value.y * Time.deltaTime, floatDirection.Value.z * Time.deltaTime);

            transform.Translate(moveDelta.x, moveDelta.y, moveDelta.z);
			
			if(GameManager.isSingleScreen()) return;
			
            if (originCamera != null && destinationCamera != null)
            {
                // move up or down until we're not longer in view of the origin camera
                Plane[] plane = GeometryUtility.CalculateFrustumPlanes(destinationCamera);
                Vector3 cameraSpacePosition = originCamera.WorldToScreenPoint(transform.position);
                bool goingUp = (floatDirection.Value.y > 0);
                float pixelHeight = originCamera.pixelHeight * 0.5f;
                //Debug.Log("cameraSpacePosition " + cameraSpacePosition + ": " + originCamera.pixelWidth);
                //Debug.Log("pixelHeight " + pixelHeight + " " + originCamera.pixelWidth);
                if ((goingUp && cameraSpacePosition.y > pixelHeight) || (!goingUp && cameraSpacePosition.y < pixelHeight))
                {
                    //Debug.Log("cameraSpacePosition.x " + cameraSpacePosition.x);
                    Vector3 position = destinationCamera.ScreenToWorldPoint(new Vector3(cameraSpacePosition.x, goingUp ? pixelHeight : pixelHeight, 0.0f));
                    position.x = Mathf.Round(position.x);
                    position.z = 0.0f;
					
                    if (shouldStopInFirstEmptySpace)
                    {
                        if (goingUp) transitionState = TransitionState.TRANSITIONING_FROM_BOTTOM;
                        else transitionState = TransitionState.TRANSITIONING_FROM_TOP;
                    }
                    else
                    {
                        transitionState = TransitionState.FLOATING_AWAY;
                    }

                    transform.position = position;

                    originCamera = null;
                    return;
                }
            }
            else
            {
                switch(transitionState)
                {
                    case TransitionState.TRANSITIONING_FROM_BOTTOM:
                        {
                            Vector3 bottom = transform.position;
                            bottom.y -= collider.bounds.size.y * 0.5f;
                            if (pointIsInCamera(bottom, destinationCamera))
                                transitionState = TransitionState.LOOKING_FOR_FREE_SPACE_GOING_UP;
                        }
                        break;

                    case TransitionState.TRANSITIONING_FROM_TOP:
                        {
                            Vector3 top = transform.position;
                            top.y += collider.bounds.size.y * 1.0f;
                            if (pointIsInCamera(top, destinationCamera))
                                transitionState = TransitionState.LOOKING_FOR_FREE_SPACE_GOING_DOWN;
                        }
                        break;

                    case TransitionState.LOOKING_FOR_FREE_SPACE_GOING_UP:
                        {
                            Collider collidedWith = collidedWithSomething();
                            if (collidedWith == null || collidedWith.GetComponent<PlayerControls2>())
                            {
                                if (collidedWith != null)
                                {
                                    PlayerControls2 hitPlayer = collidedWith.GetComponent<PlayerControls2>();
                                    if (hitPlayer != null)
                                        hitPlayer.knockBackByBlock(true, damageMultiplier);
                                }

                                floatDirection = null;
                                collider.isTrigger = false;

                                //if(passedThroughSomething) // keep it suspected in air if it didn't pass through a floor
                                    jumpState = JUMP_STATE.ON_GROUND;

                                onHitGround();
                            }
                            else
                                passedThroughSomething = true;
                        }
                        break;

                    case TransitionState.LOOKING_FOR_FREE_SPACE_GOING_DOWN:
                        {
                            //transform.Translate(moveDelta.x, moveDelta.y, moveDelta.z);

                            /*int count = 0;
                            Ray ray = new Ray(new Vector3(transform.position.x, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f));
                            RaycastHit lastHit = new RaycastHit();
                            while(count++ < 50)
                            {
                                RaycastHit hit;
                                bool didHit = collider.Raycast(ray, out hit, Mathf.Infinity);
                                Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.blue, 10);
                                if (didHit && lastHit.collider != null && hit.collider == lastHit.collider)
                                {
                                    break;
                                }

                                lastHit = hit;

                                ray.origin = new Vector3(ray.origin.x, ray.origin.y + 1.0f);
                            }

                            if (count >= 50.0f)
                                Debug.Log("failed " + count);

                            Collider collidedWith = collidedWithSomething();

                            if(collider.gameObject != null)
                                Debug.Log("collidedWith", collider.gameObject);

                            if(lastHit.collider.gameObject != null)
                                Debug.Log("lastHit ", lastHit.collider.gameObject);

                            if (collidedWith == lastHit.collider)*/
					
							Collider collidedWith = collidedWithSomething();
                            if(collidedWith != null)
                            {
								if (collidedWith != null)
                                {
                                    PlayerControls2 hitPlayer = collidedWith.GetComponent<PlayerControls2>();
                                    if (hitPlayer != null)
                                        hitPlayer.knockBackByBlock(true, damageMultiplier);
                                }
						
                                transform.Translate(-moveDelta.x, -moveDelta.y, -moveDelta.z);

                                floatDirection = null;
                                collider.isTrigger = false;
                                passedThroughSomething = true;
                                jumpState = JUMP_STATE.ON_GROUND;
                                onHitGround();
                            }
                        }
                        break;

                    case TransitionState.FLOATING_AWAY:
                        if (!renderer.isVisible)
                        {
                            Destroy(this);
                        }
                        break;
                }
            }
        }
    }

    bool pointIsInCamera(Vector3 point, Camera camera)
    {
        float halfPixelWidth = camera.pixelWidth / 2.0f;
        float halfPixelHeight = camera.pixelHeight / 2.0f;
        Vector3 cameraPosition = camera.transform.position;
        Vector3 screenPoint = camera.WorldToScreenPoint(point);
        return screenPoint.x < cameraPosition.x - halfPixelWidth ||
            screenPoint.x > cameraPosition.x + halfPixelWidth ||
            screenPoint.y > (cameraPosition.y - halfPixelHeight) ||
            screenPoint.y < (cameraPosition.y + halfPixelHeight);
    }

    public void Throw(Vector3 direction, PlayerCamera camera, PlayerCamera otherCamera, float delay)
    {
        if (direction != new Vector3(0.0f, 1.0f, 0.0f) && direction != new Vector3(0.0f, -1.0f, 0.0f))
            throw new System.Exception();

        if (delay > 0.0f)
        {
            delayedDirection = direction;
            delayedCamera = camera;
            delayedOtherCamera = otherCamera;
            throwDelayCountdown = delay;
        }
        else
        {
            DisableActor();
            floatDirection = direction * blockThrowSpeed;

            if(camera != null)
                originCamera = camera.camera;

            if(otherCamera != null)
                destinationCamera = otherCamera.camera;

            if (throwAnimation.Length != 0)
                PlayAnimation(throwAnimation);

            collider.isTrigger = true;
            passedThroughSomething = false;
        }
    }
}
