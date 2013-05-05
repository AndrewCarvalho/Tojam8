using UnityEngine;
using System.Collections;

public class PlayerTwoDetector : MonoBehaviour 
{
	void Awake () 
	{
	}
	
	void Update () 
	{
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player2")
        {
            // fuck modulatiry, end the game!!
            // factor is out later
            GameManager.PrincessWinGame();
        }
    }
}