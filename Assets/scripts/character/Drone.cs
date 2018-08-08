using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : AbstractMech {

    Vector3 pickupBox;
    public LayerMask mineralLayer;
    public LayerMask mineralHeldLayer;

    private bool holdingMineral = false;

	private void Awake()
	{
        base.Awake();

        MechConfig mechConfig;
        mechConfig.maxJumpHeight = 1.5f;
        mechConfig.minJumpHeight = 0.3f;
        mechConfig.jumpTime = 0.6f;
        mechConfig.boostTime = 0.0f;
        mechConfig.canBoost = false;
        mechConfig.canPickup = true;
        mechConfig.canDive = false;
        mechConfig.groundSpeed = 2.0f;

        this.Initialize(mechConfig);

        // TEMP
        team = Team.RED;

        pickupBox = new Vector3(1.0f, 1.0f, 0.0f);
	}

    private void FixedUpdate()
    {
        Collider2D c = Physics2D.OverlapBox(gameObject.transform.position, pickupBox, 0f, mineralLayer);
        if (c != null)
        {

            Debug.Log("collided with minerals");
            c.gameObject.transform.parent = gameObject.transform;
            c.gameObject.transform.localPosition = new Vector3(0f, 1f, 0f);
            c.gameObject.layer = LayerMask.NameToLayer("MineralHeld");
        }
    }
}
