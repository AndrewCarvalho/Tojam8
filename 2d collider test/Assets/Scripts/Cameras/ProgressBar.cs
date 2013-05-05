using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour 
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

	void Awake () 
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
	}
	
	void Update () 
	{
        
	}

    void OnGUI()
    {
        float knightDistance = this.playerOne.transform.position.x - this.knightStartX;
        if (knightDistance < 0) { knightDistance = 0; }
        float knightHeadDist = Screen.width * knightDistance / this.levelWidth;
        float princessDistance = -(this.playerTwo.transform.position.x - this.princessStartX);
        if (princessDistance < 0) { princessDistance = 0; }
        float princessHeadDist = Screen.width * princessDistance / this.levelWidth;

        GUI.DrawTexture(
            new Rect(
                0,
                (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                this.progressBarEnd.width * Utils.VIC2PIX,
                this.progressBarEnd.height * Utils.VIC2PIX),
            this.progressBarEnd);

        GUI.DrawTexture(
            new Rect(
                Screen.width - this.progressBarEnd.width * Utils.VIC2PIX,
                (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                this.progressBarEnd.width * Utils.VIC2PIX,
                this.progressBarEnd.height * Utils.VIC2PIX),
            this.progressBarEnd);

        GUI.DrawTexture(
            new Rect(
                this.progressBarEnd.width * Utils.VIC2PIX,
                (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                this.knightHeadTexture.width + knightHeadDist,
                this.progressBarKnight.height * Utils.VIC2PIX),
            this.progressBarKnight);

        float middleBarStart = this.progressBarEnd.width * Utils.VIC2PIX + this.knightHeadTexture.width + knightHeadDist;
        float middleBarWidth = Screen.width - knightHeadDist - princessHeadDist - this.progressBarEnd.width * Utils.VIC2PIX - this.princessHeadTexture.width * Utils.VIC2PIX;

        GUI.DrawTexture(
            new Rect(
                middleBarStart,
                (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                middleBarWidth,
                this.progressBarMiddle.height * Utils.VIC2PIX),
            this.progressBarMiddle);

        float princessBarStart = middleBarStart + middleBarWidth;

        GUI.DrawTexture(
            new Rect(
                princessBarStart,
                (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                princessHeadDist + this.princessHeadTexture.width,
                this.progressBarMiddle.height * Utils.VIC2PIX),
            this.progressBarPrincess);

        GUI.DrawTexture(
            new Rect(
                Screen.width - this.progressBarEnd.width * Utils.VIC2PIX,
                (Screen.height - this.progressBarEnd.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                this.progressBarEnd.width * Utils.VIC2PIX,
                this.progressBarMiddle.height * Utils.VIC2PIX),
            this.progressBarEnd);

        GUI.DrawTexture(
            new Rect(
                knightHeadDist,
                (Screen.height - this.knightHeadTexture.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                this.knightHeadTexture.width * Utils.VIC2PIX,
                this.knightHeadTexture.height * Utils.VIC2PIX), 
            this.knightHeadTexture);

        GUI.DrawTexture(
            new Rect(
                Screen.width - this.princessHeadTexture.width * Utils.VIC2PIX - princessHeadDist,
                (Screen.height - this.princessHeadTexture.height * Utils.VIC2PIX) / 2.0f + this.verticalOffset,
                this.princessHeadTexture.width * Utils.VIC2PIX,
                this.princessHeadTexture.height * Utils.VIC2PIX),
            this.princessHeadTexture);
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
        return Mathf.Max(0f, Mathf.Min(Mathf.Abs(this.playerTwo.transform.position.x - this.princessStartX) / this.levelWidth, 100f));
    }
}