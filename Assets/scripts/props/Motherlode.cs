using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motherlode : MonoBehaviour {

    public BoxCollider2D droneCollider;
    public LayerMask droneLayer;
    public SpriteRenderer sprite;
    public Transform droneAttachParent;

    private Player.Team ownedBy = Player.Team.Neutral;

    private bool open = true;

    private const float MOVEMENT_SPEED = 0.1f;
    private const float BUFFED_MOVEMENT_SPEED = 0.1f;

    private bool flipX = false;

    private bool isSpeedBuffed = false;

    void FixedUpdate() {
        if (open == false) {

            float move = isSpeedBuffed ? BUFFED_MOVEMENT_SPEED * Time.deltaTime : MOVEMENT_SPEED * Time.deltaTime;
            if (ownedBy == Player.Team.Blue) {
                move *= (-1);
                flipX = false;
            } else if (ownedBy == Player.Team.Red) {
                flipX = true;
            }

            Vector3 newPos = gameObject.transform.position + new Vector3(move, 0.0f);
            gameObject.transform.position = newPos;
            sprite.flipX = flipX;
        }
        
        CheckCollisionWithDrone();
    }

    // TODO, drone can jump off

    private void CheckCollisionWithDrone() {
        if (open) {
            Collider2D c = Physics2D.OverlapBox(droneCollider.transform.position, droneCollider.size, 0f, droneLayer);
            if (c != null) {
                Drone drone = c.gameObject.transform.parent.GetComponent<Drone>();
                if (drone != null && ownedBy == Player.Team.Neutral) {
                    AttachDrone(drone);
                }
            }
        }
    }

    public void AttachDrone(Drone drone) {
        open = false;

        isSpeedBuffed = drone.HasSpeedBuff();

        // attach drone to motherlode

        Debug.Log("Owned by :" + drone.team.ToString());

        ownedBy = drone.team;
        drone.transform.SetParent(droneAttachParent);
        drone.transform.localPosition = Vector3.zero;

        drone.StartRidingMotherlode(this);

        drone.OnDestroyed += HandleDriverKilled;
    }

    public void DettachDrone(Drone drone) {
        open = true;
        ownedBy = Player.Team.Neutral;
        isSpeedBuffed = false;

        drone.OnDestroyed -= HandleDriverKilled;
    }

    private void HandleDriverKilled() {
        open = true;
        ownedBy = Player.Team.Neutral;
        isSpeedBuffed = false;
    }
}
