using UnityEngine;
using System.Collections;

public class ParallaxLayer : MonoBehaviour 
{
    public float layer0ParallaxPercent = 0.125f;
    public float layer1ParallaxPercent = 0.25f;
    public float layer2ParallaxPercent = 0.5f;
    public float layer3ParallaxPercent = 1.0f;

    private GameObject layer0;
    private GameObject layer1;
    private GameObject layer2;
    private GameObject layer3;

	void Awake () 
	{
        this.layer0 = this.transform.FindChild("Layer0").gameObject;
        this.layer1 = this.transform.FindChild("Layer1").gameObject;
        this.layer2 = this.transform.FindChild("Layer2").gameObject;
        this.layer3 = this.transform.FindChild("Layer3").gameObject;
	}
	
	void Update () 
	{
	}

    public void OnCameraTranslate(float amount)
    {
        this.layer0.transform.Translate(-amount * this.layer0ParallaxPercent, 0, 0);
        this.layer1.transform.Translate(-amount * this.layer1ParallaxPercent, 0, 0);
        this.layer2.transform.Translate(-amount * this.layer2ParallaxPercent, 0, 0);
        this.layer3.transform.Translate(-amount * this.layer3ParallaxPercent, 0, 0);
    }
}