using UnityEngine;
using System.Collections;

public class PlayerWinCatcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        PlayerControls2 player = other.GetComponent<PlayerControls2>();
        if(player)
        {
            GameManager manager = FindObjectOfType(typeof(GameManager)) as GameManager;
            manager.notifyPlayerWin(player);
        }
    }
}
