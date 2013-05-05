using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public enum GAME_STATE { SPLITSCREEN, TRANSITIONING, SINGLESCREEN, FREEZEFRAME, WINSCREEN };

    private GAME_STATE state;
    private float timeStamp;
    private float songTime;

    private static float princessScore;
    private static bool didPrincessWin;

    private AudioSource fantasyFrolick;
    private AudioSource fantasyFrenzy;
    private AudioSource aJobWellDone;
    private AudioSource allHopeLost;
    private AudioSource outtatime;

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
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameManager)FindObjectOfType(typeof(GameManager));

                if (instance == null)
                {
                    instance = ((GameObject)Instantiate(Resources.Load("Prefabs/Managers/GameManager"))).GetComponent<GameManager>();
                }
            }
            return instance;
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
        this.outtatime = this.transform.FindChild("Outtatime").GetComponent<AudioSource>();

        //this.SetGameState(GAME_STATE.SPLITSCREEN);
        this.StopAllAudio();
        this.fantasyFrolick.Play();
    }

    void Update()
    {
        switch (this.state)
        {
            case GAME_STATE.SPLITSCREEN:
                break;
            case GAME_STATE.TRANSITIONING:
                if (Time.realtimeSinceStartup - this.timeStamp > this.songTime)
                {
                    GameManager.SetGameState(GAME_STATE.SINGLESCREEN);
                }
                break;
            case GAME_STATE.SINGLESCREEN:
                break;
            case GAME_STATE.FREEZEFRAME:
                if (Time.realtimeSinceStartup - this.timeStamp > this.songTime)
                {
                    GameManager.SetGameState(GAME_STATE.WINSCREEN);
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

    public bool isSingleScreen()
    {
        return state == GAME_STATE.SINGLESCREEN;
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

        GameManager.SetGameState(GAME_STATE.FREEZEFRAME);

        // play the audio?
        //AudioSource.PlayClipAtPoint(new AudioClip(), transform.position);
    }

    public static void KnightWinGame()
    {

    }

    public static void SetGameState(GAME_STATE state)
    {
        if (GameManager.Instance.state != state)
        {
            GameManager.Instance.state = state;
            GameManager.Instance.OnStateChange(state);
        }
    }

    private void OnStateChange(GAME_STATE newState)
    {
        switch (state)
        {
            case GAME_STATE.SPLITSCREEN:
                this.StopAllAudio();
                this.fantasyFrolick.Play();
                break;
            case GAME_STATE.TRANSITIONING:
                this.StopAllAudio();
                this.songTime = this.outtatime.clip.length;
                this.outtatime.Play();
                this.timeStamp = Time.realtimeSinceStartup;
                Time.timeScale = 0;
                break;
            case GAME_STATE.SINGLESCREEN:
                GameObject.Find("Cameras").GetComponent<CameraController>().ExitTransitionState();
                this.StopAllAudio();
                this.fantasyFrenzy.Play();
                break;
            case GAME_STATE.FREEZEFRAME:
                this.timeStamp = Time.realtimeSinceStartup;
                this.songTime = this.allHopeLost.clip.length;
                this.StopAllAudio();
                this.allHopeLost.Play();
                Time.timeScale = 0;
                break;
            case GAME_STATE.WINSCREEN:
                this.StopAllAudio();
                Application.LoadLevel("WinScene");
                break;
        }
    }

    private void StopAllAudio()
    {
        this.fantasyFrenzy.Stop();
        this.fantasyFrolick.Stop();
        this.outtatime.Stop();
        this.allHopeLost.Stop();
        this.aJobWellDone.Stop();
    }
}