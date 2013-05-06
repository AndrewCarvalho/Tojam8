using UnityEngine;
using System.Collections;

public class TitlescreenManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick2Button0) || Input.GetKey(KeyCode.A))
        {
            Application.LoadLevel("LevelTest");
        }
	}
}
