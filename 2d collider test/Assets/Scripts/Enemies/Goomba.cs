using UnityEngine;
using System.Collections;
using System;

public class Goomba : Enemy {
    float direction = -1.0f;

	// Use this for initialization
	void Start () {
        Run(this.direction);

        /*var left = GameObject.Find("LeftCollider");
        left.GetComponent(BoxCollider).DoSomething();

        var right = GameObject.Find("RightCollider");
        right.GetComponent(BoxCollider).DoSomething();*/
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    protected void switchDirection()
    {
        this.direction = this.direction > 0 ? -1.0f : 1.0f;
        Run(this.direction);
    }

    public override void onWalkedOffEdge(string id)
    {
        switchDirection();
    }

    public override void onHitLeft(Collider other)
    {
        switchDirection();
    }

    public override void onHitRight(Collider other)
    {
        switchDirection();
    }
}
