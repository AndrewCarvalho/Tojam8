using UnityEngine;
using System.Collections;

public class PlayerOneDetector : MonoBehaviour 
{
	void Awake () 
	{
	}
	
	void Update () 
	{
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player1")
        {
            // end the game!!
        }
    }
}