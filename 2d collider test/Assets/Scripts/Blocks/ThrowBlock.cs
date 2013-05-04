using UnityEngine;
using System.Collections;

public class ThrowBlock : Actor {

    protected Vector3? floatDirection = null;
    new Camera camera = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (floatDirection != null)
        {
            transform.Translate(floatDirection.Value.x * Time.deltaTime, floatDirection.Value.y * Time.deltaTime, floatDirection.Value.z * Time.deltaTime);

            bool collidedWithSomething = false;
            Object[] colliders = FindObjectsOfType(typeof(Collider));
            foreach (Object colliderObject in colliders)
            {
                Collider collider = colliderObject as Collider;
                if (collider != this.collider && this.collider.bounds.Intersects(collider.bounds))
                {
                    collidedWithSomething = true;
                    break;
                }
            }

            if(!collidedWithSomething)
            {
                floatDirection = null;
            }
        }
    }

    public void Throw(Vector3 direction, Camera _camera)
    {
        if (direction != new Vector3(0.0f, 1.0f, 0.0f) && direction != new Vector3(0.0f, -1.0f, 0.0f))
            throw new System.Exception();

        DisableActor();
        floatDirection = direction * 5.0f;
        camera = _camera;
    }
}
