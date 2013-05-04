using UnityEngine;
using System.Collections;

public class SightDetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        PlayerControls2 player = other.GetComponent<PlayerControls2>();
        if (player)
            gameObject.SendMessageUpwards("PlayerEnteredSight", player, SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerControls2 player = other.GetComponent<PlayerControls2>();
        if (player)
            gameObject.SendMessageUpwards("PlayerExitedSight", player, SendMessageOptions.DontRequireReceiver);
    }
}
