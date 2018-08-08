using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : AbstractMech {

    public HitBox hitBox;

    private void Awake()
    {
        base.Awake();

        MechConfig mechConfig;
        mechConfig.maxJumpHeight = 4.0f;
        mechConfig.minJumpHeight = 1.0f;
        mechConfig.jumpTime = 0.5f;
        mechConfig.boostTime = 0.25f;
        mechConfig.canBoost = true;
        mechConfig.canPickup = false;
        mechConfig.canDive = false;
        mechConfig.groundSpeed = 6.0f;
         
        Initialize(mechConfig);
        InitializeHitBox();

        // TEMP
        team = Team.BLUE;
    }

    private void InitializeHitBox()
    {
        hitBox.Initialize(Team.RED);
    }

	private void Update()
	{
        ToggleAttackBox(!controller.collisions.below);
	}

    private void ToggleAttackBox(bool toggle)
    {
        hitBox.gameObject.SetActive(toggle);
    }

    private void OnCollisionEnterAttackBox(Collision2D collision)
    {
        Debug.Log("Something entered our attack collision box!" + collision.gameObject.name);
    }
}
