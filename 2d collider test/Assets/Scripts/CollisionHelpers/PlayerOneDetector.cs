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
        Debug.Log("name is: " + other.name);
        if (other.name == "Player1")
        {
            // end the game!!
            GameManager.PrincessWinGame();
            Debug.Log("WON STATE!!!!");
        }
    }
}