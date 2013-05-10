using UnityEngine;
using System.Collections;

public class BombBlock : ThrowBlock {

    bool ignited = false;
    bool exploded = false;
    float igniteCountdown = 0.0f;
    float explodeCountdown = 0.0f;
    float igniteDuration = 2.25f;

    float explosionRadius = 3;
	float explosionDamageModifier = 6;

	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();

        if (!ignited)
            return;

        if (jumpState == JUMP_STATE.ON_GROUND)
            igniteCountdown -= Time.deltaTime;

        explodeCountdown -= Time.deltaTime;

        if (exploded == false && igniteCountdown < 0.0f && explodeCountdown < 0.0f)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            PlayAnimation("Explode");

            PlayerControls2[] players = FindObjectsOfType(typeof(PlayerControls2)) as PlayerControls2[];
            foreach (PlayerControls2 player in players)
            {
                Vector3 diff = player.transform.position - transform.position;
                if (diff.magnitude < igniteDuration)
                    player.Hurt(explosionDamageModifier);
            }

            explodeCountdown = getAnimationDuration("Explode") - 0.1f;
            exploded = true;
			DisableActor();
        }
        else if (exploded && explodeCountdown < 0.0f)
        {
            Destroy(gameObject);
        }
	}

    protected override void onHitGround()
    {
        if (!ignited)
        {
            PlayAnimation("Ignite");
            igniteCountdown = igniteDuration;
            ignited = true;
        }
    }
}
