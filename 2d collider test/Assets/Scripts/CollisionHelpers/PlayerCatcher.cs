using UnityEngine;
using System.Collections;

public class PlayerCatcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter (Collider other)
    {
        PlayerControls2 player = other.GetComponent<PlayerControls2>();
        if (player)
        {
            gameObject.SendMessageUpwards("onPlayerFell", player);
        }
    }
}
