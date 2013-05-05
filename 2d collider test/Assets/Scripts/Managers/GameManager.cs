using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public enum GAME_STATE { SPLITSCREEN, TRANSITIONING, SINGLESCREEN, FREEZEFRAME, WINSCREEN };

    private GAME_STATE state = GAME_STATE.SPLITSCREEN;
    private float timer = 0;
    private float songTime;

    private static float princessScore;
    private static bool didPrincessWin;

    private AudioSource fantasyFrolick;
    private AudioSource fantasyFrenzy;
    private AudioSource aJobWellDone;
    private AudioSource allHopeLost;

    public static float PrincessScore 
    {
        get
        {
            return GameManager.princessScore;
        }
        //set 
        //{
        //    GameManager.princessScore = value;
        //}
    }

    public static bool DidPrincessWin
    {
        get
        {
            return GameManager.didPrincessWin;
        }
        //set
        //{
        //    GameManager.didPrincessWin = value;
        //}
    }

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

        this.fantasyFrolick = this.transform.FindChild("FantasyFrolick").GetComponent<AudioSource>();
        this.fantasyFrenzy = this.transform.FindChild("FantasyFrenzy").GetComponent<AudioSource>();
        this.aJobWellDone = this.transform.FindChild("AJobWellDone").GetComponent<AudioSource>();
        this.allHopeLost = this.transform.FindChild("AllHopeLost").GetComponent<AudioSource>();

        this.fantasyFrolick.Play();

        this.fantasyFrenzy.Stop();
        this.aJobWellDone.Stop();
        this.allHopeLost.Stop();
    }

    void Update()
    {
        switch (this.state)
        {
            case GAME_STATE.SPLITSCREEN:

            case GAME_STATE.TRANSITIONING:
            case GAME_STATE.SINGLESCREEN:
            case GAME_STATE.FREEZEFRAME:
                this.timer += Time.deltaTime;
                if (this.timer > this.songTime)
                {
                    Time.timeScale = 1;

                    //Application.LoadLevel("WinScreen");
                }
                break;
            case GAME_STATE.WINSCREEN:
                break;
        }
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

    public static void PrincessWinGame()
    {
        GameManager.princessScore = GameObject.Find("Cameras").GetComponent<ProgressBar>().GetPrincessPercent();
        GameManager.didPrincessWin = true;

        // pause the game
        Time.timeScale = 0;

        // play the audio?
        //AudioSource.PlayClipAtPoint(new AudioClip(), transform.position);
    }

    public static void KnightWinGame()
    {

    }
}