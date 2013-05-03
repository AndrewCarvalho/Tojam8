using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    private PlayerCamera cameraOne;
    private PlayerCamera cameraTwo;

    //private Player

	void Awake () 
	{
        this.cameraOne = GetComponentInChildren<PlayerOneCamera>();
        this.cameraTwo = GetComponentInChildren<PlayerTwoCamera>();
	}
	
	void Update () 
	{
        
	}
}