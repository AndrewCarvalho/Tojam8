using UnityEngine;
using System.Collections;

public class EndScreenProgressBar : MonoBehaviour 
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private float levelWidth;
    public Texture2D knightHeadTexture;
    public Texture2D princessHeadTexture;
    private float knightStartX;
    private float princessStartX;
    private float verticalOffset = 0f;
    public Texture2D progressBarEnd;
    public Texture2D progressBarKnight;
    public Texture2D progressBarMiddle;
    public Texture2D progressBarPrincess;




    private float timer = 0f;
    private float princessPercentage;
    private bool didPrincessWin;
    private float lastPrincessDistance;
    private float lastKnightDistance;
    private float princessTarget;
    private float knightTarget;
    private bool doneLinearInterpolation = false;
    private bool showingText = false;
    private AudioSource finalSong;

    private TextMesh finalText;
    private TextMesh aToContinue;

    void Awake()
    {
        this.playerOne = GameObject.Find("Player1");
        this.playerTwo = GameObject.Find("Player2");

        this.levelWidth = this.playerTwo.transform.position.x - this.playerOne.transform.position.x;
        this.knightStartX = this.playerOne.transform.position.x;
        this.princessStartX = this.playerTwo.transform.position.x;

        this.knightHeadTexture.filterMode = FilterMode.Point;
        this.princessHeadTexture.filterMode = FilterMode.Point;
        this.progressBarEnd.filterMode = FilterMode.Point;
        this.progressBarKnight.filterMode = FilterMode.Point;
        this.progressBarMiddle.filterMode = FilterMode.Point;
        this.progressBarPrincess.filterMode = FilterMode.Point;

        //this.knightHeadTexture = (Texture)Resources.Load("");
        //this.princessHeadTexture = (Texture)Resources.Load("");

        // new stuff!!!
        this.princessPercentage = GameManager.PrincessScore;
        //this.princessPercentage = .5f;
        this.didPrincessWin = GameManager.DidPrincessWin;

        this.lastPrincessDistance = 0;
        this.lastKnightDistance = 0;

        this.princessTarget = this.princessPercentage * Screen.width;
        this.knightTarget = (1f - this.princessPercentage) * Screen.width;

        // just in case!!

        Time.timeScale = 1;

        this.finalSong = this.transform.FindChild("FinalSong").GetComponent<AudioSource>();
        this.finalText = GameObject.Find("WinnerText").GetComponent<TextMesh>();
        this.aToContinue = GameObject.Find("PressAToContinue").GetComponent<TextMesh>();
    }

    void Update()
    {
        if (this.timer < 1f)
        {
            this.timer += Time.deltaTime / 60.0f;
            if (this.timer > 1f)
                this.timer = 1f;
        }

        if (this.doneLinearInterpolation && !this.showingText)
        {
            this.showingText = true;
            this.finalSong.Play();
            if (this.princessPercentage >= 0.5f)
            {
                this.finalText.text = "Damsel Wins!";
            }
            else
            {
                this.finalText.text = "Knight Wins";
            }
            this.aToContinue.text = "Press Y To Play Again";
        }

        if (this.doneLinearInterpolation)
        {
            if (Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.Joystick2Button3) || Input.GetKey(KeyCode.Y))
            {
                Application.LoadLevel("Titlescreen");
            }
        }
    }

    void OnGUI()
    {
        float currentKnightPerc = Mathf.Lerp(this.lastKnightDistance, 1 - this.princessPercentage, this.timer);
        this.lastKnightDistance = currentKnightPerc;
        float currentKnightWidth = currentKnightPerc * Screen.width;
        GUI.DrawTexture(
            new Rect(
                    0,
                    Screen.height / 2f - this.progressBarKnight.height * Utils.VIC2PIX / 2f,
                    currentKnightWidth,
                    this.progressBarKnight.height * Utils.VIC2PIX
                ),
                this.progressBarKnight
            );

        float currentPrincessPerc = Mathf.Lerp(this.lastPrincessDistance, this.princessPercentage, this.timer);
        this.lastPrincessDistance = currentPrincessPerc;
        float currentPrincessWidth = currentPrincessPerc * Screen.width;
        GUI.DrawTexture(
            new Rect(
                    Screen.width - currentPrincessWidth,
                    Screen.height / 2f - this.progressBarPrincess.height * Utils.VIC2PIX / 2f,
                    currentPrincessWidth,
                    this.progressBarPrincess.height * Utils.VIC2PIX
                ),
                this.progressBarPrincess
            );

        GUI.DrawTexture(
            new Rect(
                    0,
                    Screen.height / 2f - this.progressBarEnd.height * Utils.VIC2PIX / 2f,
                    this.progressBarEnd.width * Utils.VIC2PIX,
                    this.progressBarEnd.height * Utils.VIC2PIX
                ),
                this.progressBarEnd
            );

        GUI.DrawTexture(
            new Rect(
                    Screen.width - this.progressBarEnd.width * Utils.VIC2PIX,
                    Screen.height / 2f - this.progressBarEnd.height * Utils.VIC2PIX / 2f,
                    this.progressBarEnd.width * Utils.VIC2PIX,
                    this.progressBarEnd.height * Utils.VIC2PIX
                ),
                this.progressBarEnd
            );

        GUI.DrawTexture(
            new Rect(
                    Mathf.Max(0, currentKnightWidth - this.knightHeadTexture.width * Utils.VIC2PIX),
                    Screen.height / 2f - this.knightHeadTexture.height * Utils.VIC2PIX / 2f,
                    this.knightHeadTexture.width * Utils.VIC2PIX,
                    this.knightHeadTexture.height * Utils.VIC2PIX
                ),
                this.knightHeadTexture
            );

        GUI.DrawTexture(
            new Rect(
                    Mathf.Min(Screen.width - this.princessHeadTexture.width * Utils.VIC2PIX, Screen.width - currentPrincessWidth),
                    Screen.height / 2f - this.princessHeadTexture.height * Utils.VIC2PIX / 2f,
                    this.princessHeadTexture.width * Utils.VIC2PIX,
                    this.princessHeadTexture.height * Utils.VIC2PIX
                ),
                this.princessHeadTexture
            );


        if (Mathf.Abs(currentKnightPerc - (1 - this.princessPercentage)) < Utils.MOVE_PADDING)
        {
            this.doneLinearInterpolation = true;
        }





        ////float knightDistance = this.playerOne.transform.position.x - this.knightStartX;
        //float knightDistance = Mathf.Lerp(this.lastKnightDistance, 1 - this.princessPercentage, this.timer);
        //if (knightDistance < 0) { knightDistance = 0; }
        //float knightHeadDist = Screen.width * knightDistance;
        ////float knightHeadDist = Screen.width * knightDistance / this.levelWidth;
        ////float princessDistance = -(this.playerTwo.transform.position.x - this.princessStartX);
        //float princessDistance = Mathf.Lerp(this.lastPrincessDistance, this.princessPercentage, this.timer);
        //if (princessDistance < 0) { princessDistance = 0; }
        //float princessHeadDist = Screen.width * princessDistance;
        ////float princessHeadDist = Screen.width * princessDistance / this.levelWidth;

        //this.lastKnightDistance = knightDistance;
        //this.lastPrincessDistance = princessDistance;

        //GUI.DrawTexture(
        //    new Rect(
        //        0,
        //        (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        this.progressBarEnd.width * Utils.VIC2PIX,
        //        this.progressBarEnd.height * Utils.VIC2PIX),
        //    this.progressBarEnd);

        //GUI.DrawTexture(
        //    new Rect(
        //        Screen.width - this.progressBarEnd.width * Utils.VIC2PIX,
        //        (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        this.progressBarEnd.width * Utils.VIC2PIX,
        //        this.progressBarEnd.height * Utils.VIC2PIX),
        //    this.progressBarEnd);

        //GUI.DrawTexture(
        //    new Rect(
        //        this.progressBarEnd.width * Utils.VIC2PIX - this.knightHeadTexture.width * Utils.VIC2PIX / 2.0f,
        //        (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        this.knightHeadTexture.width + knightHeadDist,
        //        this.progressBarKnight.height * Utils.VIC2PIX),
        //    this.progressBarKnight);

        //float middleBarStart = this.progressBarEnd.width * Utils.VIC2PIX - this.knightHeadTexture.width * Utils.VIC2PIX / 2.0f + this.knightHeadTexture.width + knightHeadDist;
        //float middleBarWidth = Screen.width - knightHeadDist - princessHeadDist - this.progressBarEnd.width * Utils.VIC2PIX - this.princessHeadTexture.width * Utils.VIC2PIX;

        //GUI.DrawTexture(
        //    new Rect(
        //        middleBarStart,
        //        (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        middleBarWidth,
        //        this.progressBarMiddle.height * Utils.VIC2PIX),
        //    this.progressBarMiddle);

        //float princessBarStart = middleBarStart + middleBarWidth;

        //GUI.DrawTexture(
        //    new Rect(
        //        princessBarStart,
        //        (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        princessHeadDist + this.princessHeadTexture.width,
        //        this.progressBarMiddle.height * Utils.VIC2PIX),
        //    this.progressBarPrincess);

        //GUI.DrawTexture(
        //    new Rect(
        //        Screen.width - this.progressBarEnd.width * Utils.VIC2PIX,
        //        (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        this.progressBarEnd.width * Utils.VIC2PIX,
        //        this.progressBarMiddle.height * Utils.VIC2PIX),
        //    this.progressBarEnd);

        //GUI.DrawTexture(
        //    new Rect(
        //        //knightHeadDist - this.knightHeadTexture.width * Utils.VIC2PIX,
        //        this.progressBarEnd.width * Utils.VIC2PIX - this.knightHeadTexture.width * Utils.VIC2PIX / 2.0f + this.knightHeadTexture.width + knightHeadDist,
        //        (Screen.height - this.knightHeadTexture.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        this.knightHeadTexture.width * Utils.VIC2PIX,
        //        this.knightHeadTexture.height * Utils.VIC2PIX),
        //    this.knightHeadTexture);

        //GUI.DrawTexture(
        //    new Rect(
        //        //Screen.width - this.princessHeadTexture.width * Utils.VIC2PIX - princessHeadDist,
        //        princessBarStart,
        //        (Screen.height - this.princessHeadTexture.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
        //        this.princessHeadTexture.width * Utils.VIC2PIX,
        //        this.princessHeadTexture.height * Utils.VIC2PIX),
        //    this.princessHeadTexture);
    }

    public void ChangeToSingleScreenPosition()
    {
        this.verticalOffset = /* 16 * Utils.VIC2PIX + */ (float)Screen.height / 4.0f;
    }

    public void ChangeToSplitScreenPosition()
    {
        this.verticalOffset = 0f;
    }

    public float GetPrincessPercent()
    {
        return Mathf.Max(0f, Mathf.Min(Screen.width * -(this.playerTwo.transform.position.x - this.princessStartX) / this.levelWidth, 100f));
    }
}