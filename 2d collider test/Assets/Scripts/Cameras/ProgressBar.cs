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

        //GUI.DrawTexture(
        //    );
        GUI.DrawTexture(
            new Rect(
                0,
                (Screen.height - this.progressBarEnd.height * 2.0f) / 2.0f,
                this.progressBarEnd.width * 2.0f,
                this.progressBarEnd.height * 2.0f),
            this.progressBarEnd);

        GUI.DrawTexture(
            new Rect(
                Screen.width - this.progressBarEnd.width * 2.0f,
                (Screen.height - this.progressBarEnd.height * 2.0f) / 2.0f,
                this.progressBarEnd.width * 2.0f,
                this.progressBarEnd.height * 2.0f),
            this.progressBarEnd);

        GUI.DrawTexture(
            new Rect(
                this.progressBarEnd.width * 2.0f,
                (Screen.height - this.progressBarEnd.height * 2.0f) / 2.0f,
                this.knightHeadTexture.width + knightHeadDist,
                this.progressBarKnight.height * 2.0f),
            this.progressBarKnight);

        float middleBarStart = this.progressBarEnd.width * 2.0f + this.knightHeadTexture.width + knightHeadDist;
        float middleBarWidth = Screen.width - knightHeadDist - princessHeadDist - this.progressBarEnd.width * 2.0f - this.princessHeadTexture.width * 2.0f;

        GUI.DrawTexture(
            new Rect(
                middleBarStart,
                (Screen.height - this.progressBarEnd.height * 2.0f) / 2.0f,
                middleBarWidth,
                this.progressBarMiddle.height * 2.0f),
            this.progressBarMiddle);

        float princessBarStart = middleBarStart + middleBarWidth;

        GUI.DrawTexture(
            new Rect(
                princessBarStart,
                (Screen.height - this.progressBarEnd.height * 2.0f) / 2.0f,
                princessHeadDist + this.princessHeadTexture.width,
                this.progressBarMiddle.height * 2.0f),
            this.progressBarPrincess);

        GUI.DrawTexture(
            new Rect(
                knightHeadDist, 
                (Screen.height - this.knightHeadTexture.height * 2.0f) / 2.0f, 
                this.knightHeadTexture.width * 2.0f, 
                this.knightHeadTexture.height * 2.0f), 
            this.knightHeadTexture);

        GUI.DrawTexture(
            new Rect(
                Screen.width - this.princessHeadTexture.width * 2.0f - princessHeadDist,
                (Screen.height - this.princessHeadTexture.height * 2.0f) / 2.0f,
                this.princessHeadTexture.width * 2.0f,
                this.princessHeadTexture.height * 2.0f),
            this.princessHeadTexture);
    }
}