using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public enum CAMERACONTROL_STATE { NORMAL, ALERT, TRANSITION_TO_SINGLESCREEN, SINGLESCREEN };

    private PlayerCamera cameraOne;
    private PlayerCamera cameraTwo;
    private Camera princessParallaxCamera;
    private ProgressBar progBar;

    private GameObject playerOne;
    private GameObject playerTwo;

    public float distanceForAlertMode = 50;

    private CAMERACONTROL_STATE state;

    public float transitionTime = 3f;
    private float transitionTimer = 0f;

    private tk2dSprite transitionCard;

    private Vector3 savedBottomParallaxCameraPosition;

    //private Player

	void Awake () 
	{
        this.cameraOne = GetComponentInChildren<PlayerOneCamera>();
        this.cameraTwo = GetComponentInChildren<PlayerTwoCamera>();

        this.playerOne = GameObject.Find("Player1");
        this.playerTwo = GameObject.Find("Player2");

        this.transitionCard = GameObject.Find("TransitionCard").gameObject.GetComponent<tk2dSprite>();
	}
	
	void Update () 
	{
        // camera is 24 tiles wide.  players are each 1 tile.  They need to be 23 tiles away for both to be visible (they're centered!)
        //float playerDistance = Vector2.Distance(this.playerOne.transform.position, this.playerTwo.transform.position);
        float playerDistance;
        switch(this.state) 
        {
            case CAMERACONTROL_STATE.NORMAL:
                playerDistance = Vector2.Distance(this.cameraOne.transform.position, this.cameraTwo.transform.position);
                if (playerDistance < this.distanceForAlertMode)
                {
                    this.state = CAMERACONTROL_STATE.ALERT;
                }
                break;
            case CAMERACONTROL_STATE.ALERT:
                //cameraDistance = Vector2.Distance(this.cameraOne.transform.position, this.cameraTwo.transform.position);
                playerDistance = Vector2.Distance(this.playerOne.transform.position, this.playerTwo.transform.position);
                if (playerDistance < 22) // sceen is 24, -1 for player width, -1 for not putting them at the edge because that's ugly
                {
                    this.progBar = GameObject.Find("Cameras").GetComponent<ProgressBar>();
                    this.princessParallaxCamera = this.cameraTwo.transform.FindChild("ParallaxLayersPrincess").GetComponentInChildren<Camera>();
                    this.savedBottomParallaxCameraPosition = this.princessParallaxCamera.transform.position;
                    this.princessParallaxCamera.transform.position = new Vector3(0, -60, -10);
                    this.progBar.enabled = false;
                    this.cameraOne.gameObject.SetActive(false);
                    this.cameraTwo.enabled = false;
                    this.cameraTwo.GetComponent<Camera>().enabled = false;
                    this.state = CAMERACONTROL_STATE.TRANSITION_TO_SINGLESCREEN;

                    this.transitionTimer = Time.realtimeSinceStartup;

                    Time.timeScale = 0;
                }
                break;
            case CAMERACONTROL_STATE.TRANSITION_TO_SINGLESCREEN:
                // either make a new camera or hijack the one that is calling clear.... latter is easier
                // camera bottom is the one clearing
                if (Time.realtimeSinceStartup - this.transitionTimer > this.transitionTime)
                {
                    //this.princessParallaxCamera = this.cameraTwo.transform.FindChild("ParallaxLayersPrincess").GetComponentInChildren<Camera>();
                    this.princessParallaxCamera.transform.position = this.savedBottomParallaxCameraPosition;
                    this.cameraTwo.enabled = true;
                    this.cameraTwo.GetComponent<Camera>().enabled = true;
                    this.progBar.enabled = true;
                    this.progBar.ChangeToSingleScreenPosition();
                    float playerDistanceX = this.playerOne.transform.position.x + (this.playerTwo.transform.position.x - this.playerOne.transform.position.x) / 2f;
                    this.cameraTwo.transform.position = new Vector3(playerDistanceX, this.cameraTwo.transform.position.y - 4.5f, -10);
                    this.cameraTwo.transform.FindChild("PlayerTwoBarrier").Translate(new Vector3(0, 4.5f, 0));
                    this.princessParallaxCamera.transform.Translate(0, -4.5f, 0);

                    Time.timeScale = 1;

                    this.state = CAMERACONTROL_STATE.SINGLESCREEN;
                    GameManager manager = FindObjectOfType(typeof(GameManager)) as GameManager;
                    if(manager)
                        manager.notifySingleScreen();
                }
                break;
            case CAMERACONTROL_STATE.SINGLESCREEN:
                break;
        }
	}
}