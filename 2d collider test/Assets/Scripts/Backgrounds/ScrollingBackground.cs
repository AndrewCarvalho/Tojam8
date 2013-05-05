using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour 
{
    private ParallaxLayer parallaxLayer;
    private tk2dSprite sprite0;
    private tk2dSprite sprite1;
    private float spriteWidth;
    private float spriteUVWidth;
    private float uvWidth = 1f;
    private float uvMin;
    private float uvMax;

    private float cameraWidth = 24f;
    private float cameraHeight = 9f;

	void Awake () 
	{
        this.parallaxLayer = this.transform.parent.gameObject.GetComponent<ParallaxLayer>();

        this.sprite0 = this.transform.FindChild("Sprite0").GetComponent<tk2dSprite>();
        this.sprite1 = this.transform.FindChild("Sprite1").GetComponent<tk2dSprite>();

        this.sprite0.transform.position = this.parallaxLayer.transform.position;
        this.sprite1.transform.position = this.sprite0.transform.position + new Vector3(this.sprite0.GetBounds().extents.x + this.sprite0.GetBounds().center.x + this.sprite1.GetBounds().extents.x - this.sprite1.GetBounds().center.x, 0, 0);
	}
	
	void Update () 
	{
        this.sprite1.transform.position = this.sprite0.transform.position + new Vector3(this.sprite0.GetBounds().extents.x + this.sprite0.GetBounds().center.x + this.sprite1.GetBounds().extents.x - this.sprite1.GetBounds().center.x, 0, 0);
	}

    void OnBecameInvisible()
    {
        tk2dSprite temp = this.sprite0;
        this.sprite0 = this.sprite1;
        this.sprite1 = this.sprite0;
    }
}