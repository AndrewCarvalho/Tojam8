using UnityEngine;
using System.Collections;

public class ThrowBlock : Actor {

    [SerializeField]
    private float blockThrowSpeed = 20.0f;

    protected Vector3? floatDirection = null;
    Camera originCamera = null;
    Camera destinationCamera = null;
    bool passedThroughSomething = false;

    enum TransitionState { NOT_TRANSITIONING, TRANSITIONING_FROM_BOTTOM, TRANSITIONING_FROM_TOP, LOOKING_FOR_FREE_SPACE_GOING_UP, LOOKING_FOR_FREE_SPACE_GOING_DOWN };
    TransitionState transitionState = TransitionState.NOT_TRANSITIONING;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (floatDirection != null)
        {
            Vector3 moveDelta = new Vector3(floatDirection.Value.x * Time.deltaTime, floatDirection.Value.y * Time.deltaTime, floatDirection.Value.z * Time.deltaTime);

            transform.Translate(moveDelta.x, moveDelta.y, moveDelta.z);
            if (originCamera != null)
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

                    // snap to closest tile
                    float divider = 4.0f;
                    float remainder = position.x % divider;
                    if (remainder >= (divider / 2))
                    {
                        Debug.Log("left " + remainder);
                        position.x -= (remainder);
                    }
                    else
                    {
                        Debug.Log("right " + remainder);
                        position.x += remainder;
                    }

                    position.z = 0.0f;

                    if (goingUp) transitionState = TransitionState.TRANSITIONING_FROM_BOTTOM;
                    else transitionState = TransitionState.TRANSITIONING_FROM_TOP;

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
                                        hitPlayer.knockBackByBlock();
                                }

                                floatDirection = null;
                                collider.isTrigger = false;

                                if(passedThroughSomething) // keep it suspected in air if it didn't pass through a floor
                                    jumpState = JUMP_STATE.ON_GROUND;
                            }
                            else
                                passedThroughSomething = true;
                        }
                        break;

                    case TransitionState.LOOKING_FOR_FREE_SPACE_GOING_DOWN:
                        {
                            transform.Translate(moveDelta.x, moveDelta.y, moveDelta.z);

                            /*RaycastHit[] hits = body.SweepTestAll(new Vector3(1.0f, 0.0f, 0.0f));
                            Debug.DrawRay(transform.position, moveDelta * 50, Color.black, 0, false);
                            RaycastHit? lowest = null;
                            foreach (RaycastHit hit in hits)
                            {
                                Gizmos.DrawSphere(hit.point, 1);
                                if (lowest == null || hit.point.y < lowest.Value.point.y)
                                    lowest = hit;
                            }

                            if(lowest != null)
                                Debug.Log("lowest " + lowest.Value.collider.gameObject.name);*/

                            //if (lowest != null && lowest.Value.collider.bounds.Intersects(collider.bounds))

                            if (collidedWithSomething() != null)
                            {
                                transform.Translate(-moveDelta.x, -moveDelta.y, -moveDelta.z);

                                floatDirection = null;
                                collider.isTrigger = false;
                                passedThroughSomething = true;
                                jumpState = JUMP_STATE.ON_GROUND;
                            }
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

    Collider collidedWithSomething()
    {
        Object[] colliders = FindObjectsOfType(typeof(Collider));
        foreach (Object colliderObject in colliders)
        {
            Collider current = colliderObject as Collider;
            if (current != this.collider && this.collider.bounds.Intersects(current.bounds))
            {
                return current;
            }
        }
        return null;
    }

    public void Throw(Vector3 direction, PlayerCamera camera, PlayerCamera otherCamera)
    {
        if (direction != new Vector3(0.0f, 1.0f, 0.0f) && direction != new Vector3(0.0f, -1.0f, 0.0f))
            throw new System.Exception();

        DisableActor();
        floatDirection = direction * blockThrowSpeed;
        originCamera = camera.camera;
        destinationCamera = otherCamera.camera;
        collider.isTrigger = true;
        passedThroughSomething = false;
    }
}
