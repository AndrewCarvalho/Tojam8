using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
    private Camera camera;
    protected GameObject player;
    protected Vector2 direction;

    public float cameraMoveThreshold;

	void Awake () 
	{
        this.camera = this.GetComponent<Camera>();
        this.OnAwake(); // sets the other members

        this.transform.position = new Vector3(this.player.transform.position.x, this.transform.position.y, this.transform.position.z);
	}
	
	void Update () 
	{
        Vector2 diffVector = new Vector2(this.player.transform.position.x - this.transform.position.x, this.player.transform.position.y - this.transform.position.y);
        float xDiff = Vector2.Dot(diffVector, this.direction);
        if (xDiff > 0 && Mathf.Abs(diffVector.x) > this.cameraMoveThreshold)
        {
            this.transform.Translate(new Vector3(this.direction.x * xDiff - this.cameraMoveThreshold, 0, 0));
        }
	}

    protected virtual void OnAwake()
    {
    }
}