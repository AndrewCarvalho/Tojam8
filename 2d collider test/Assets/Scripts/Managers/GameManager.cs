using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private BoxCollider bottomGameBoundary;
    private GameManager instance;
    public GameManager Instance
    {
        get
        {
            if (this.instance == null)
            {
                this.instance = (GameManager)FindObjectOfType(typeof(GameManager));

                if (this.instance == null)
                {
                    this.instance = ((GameObject)Instantiate(Resources.Load("Prefabs/Managers/GameManager"))).GetComponent<GameManager>();
                }
            }
            return this.instance;
        }
    }

    void Start()
    {
        bottomGameBoundary = ((GameObject)Instantiate(Resources.Load("Prefabs/HorizontalBoundary"))).GetComponent<BoxCollider>();
        bottomGameBoundary.transform.parent = transform;

        float boundaryWidth = bottomGameBoundary.bounds.size.x;
        bottomGameBoundary.transform.position = new Vector3(boundaryWidth / 2, 0.0f, 0.0f);

    }

    void onPlayerFell(PlayerControls2 player)
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void notifySingleScreen()
    {
           
    }

    public void notifyPlayerWin(PlayerControls2 winner)
    {
        Debug.Log("Player " + winner.GetComponent<PlayerOneControls>() != null ? " 1 " : " 2 " + "wins!");
        Application.LoadLevel(Application.loadedLevel);
    }
 
	void Awake () 
	{

	}
	
	void Update () 
	{

	}
}