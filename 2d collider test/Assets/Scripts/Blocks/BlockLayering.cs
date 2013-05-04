using UnityEngine;
using System.Collections;

public class BlockLayering : MonoBehaviour 
{
	void Awake () 
	{
        this.transform.Translate(0, 0, (this.transform.position.x + this.transform.position.y) * Utils.ZINC);
	}
}