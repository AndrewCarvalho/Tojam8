using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

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
 
	void Awake () 
	{

	}
	
	void Update () 
	{

	}
}