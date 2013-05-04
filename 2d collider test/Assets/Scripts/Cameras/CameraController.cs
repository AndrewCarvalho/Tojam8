using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public enum CAMERACONTROL_STATE { NORMAL, ALERT, TRANSITION_TO_SINGLESCREEN, SINGLESCREEN };

    private PlayerCamera cameraOne;
    private PlayerCamera cameraTwo;

    private GameObject playerOne;
    private GameObject playerTwo;

    public float distanceForAlertMode = 50;

    private CAMERACONTROL_STATE state;

    //private Player

	void Awake () 
	{
        this.cameraOne = GetComponentInChildren<PlayerOneCamera>();
        this.cameraTwo = GetComponentInChildren<PlayerTwoCamera>();

        this.playerOne = GameObject.Find("Player1");
        this.playerTwo = GameObject.Find("Player2");
	}
	
	void Update () 
	{
        // camera is 24 tiles wide.  players are each 1 tile.  They need to be 23 tiles away for both to be visible (they're centered!)
        //float playerDistance = Vector2.Distance(this.playerOne.transform.position, this.playerTwo.transform.position);
        float cameraDistance;
        switch(this.state) 
        {
            case CAMERACONTROL_STATE.NORMAL:
                cameraDistance = Vector2.Distance(this.cameraOne.transform.position, this.cameraTwo.transform.position);
                if (cameraDistance < this.distanceForAlertMode)
                {
                    this.state = CAMERACONTROL_STATE.ALERT;
                }
                break;
            case CAMERACONTROL_STATE.ALERT:
                cameraDistance = Vector2.Distance(this.cameraOne.transform.position, this.cameraTwo.transform.position);
                if (cameraDistance < 22) // sceen is 24, -1 for player width, -1 for not putting them at the edge because that's ugly
                {
                    this.state = CAMERACONTROL_STATE.TRANSITION_TO_SINGLESCREEN;
                }
                break;
            case CAMERACONTROL_STATE.TRANSITION_TO_SINGLESCREEN:
                // either make a new camera or hijack the one that is calling clear.... latter is easier
                // camera bottom is the one clearing
                ProgressBar progBar = GameObject.Find("Cameras").GetComponent<ProgressBar>();
                progBar.enabled = false;

                float playerDistanceX = this.playerOne.transform.position.x + (this.playerTwo.transform.position.x - this.playerOne.transform.position.x) / 2f;
                this.cameraOne.gameObject.SetActive(false);
                this.cameraTwo.transform.position = new Vector3(playerDistanceX, this.cameraTwo.transform.position.y - 4.5f, -10);

                this.state = CAMERACONTROL_STATE.SINGLESCREEN;
                break;
            case CAMERACONTROL_STATE.SINGLESCREEN:
                break;
        }
	}
}