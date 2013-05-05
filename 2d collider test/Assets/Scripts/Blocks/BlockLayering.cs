using UnityEngine;
using System.Collections;

public class BlockLayering : MonoBehaviour 
{
	void Awake () 
	{
        this.transform.Translate(0, 0, (this.transform.position.x + this.transform.position.y) * Utils.ZINC);
	}

    /*new void Update()
    {
        Vector3 left = transform.position;
        Vector3 right = transform.position;
        float size = collider.bounds.size.x / 2;
        left.x -= size;
        right.x += size;

        Debug.DrawRay(left, new Vector3(0, 10.0f, 0));
        Debug.DrawRay(right, new Vector3(0, 10.0f, 0));
    }*/
}