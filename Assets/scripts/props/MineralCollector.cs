using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralCollector : MonoBehaviour {
    public BoxCollider2D droneCollider;
    public GameObject mineralSprite;
    public LayerMask droneLayer;
    public Player.Team owner;

    public delegate void CollectorFilled(Player.Team team);
    public CollectorFilled OnCollectorFilled;
    private void TriggerOnCollectorFilled() { if (OnCollectorFilled != null) OnCollectorFilled(owner); }

    private const float DEPOSIT_TIME_S = 1.5f;

    private bool filled = false;
    private IEnumerator timedCoroutine;
    
    void FixedUpdate() {
        if (CheckCollisionWithDrone()) return;
    }

    private bool CheckCollisionWithDrone() {
        if (!filled) {
            Collider2D c = Physics2D.OverlapBox(droneCollider.transform.position, droneCollider.size, 0f, droneLayer);
            if (c != null) {
                Drone drone = c.gameObject.transform.parent.GetComponent<Drone>();                
                if (drone != null && owner == drone.team && drone.IsHoldingMineral()) {
                    Debug.Log(gameObject.name + " Accepted deposit");
                    StartMineralDeposit(drone);
                    return true;
                }
            }
        }
        return false;
    }

    private void StartMineralDeposit(Drone d) {
        timedCoroutine = TimedCoroutine(d);
        StartCoroutine(timedCoroutine);
    }

    private IEnumerator TimedCoroutine(Drone d) {
        // make drone invulerable
        d.StartDepositMineral();

        mineralSprite.SetActive(true);
        filled = true;
        droneCollider.enabled = false;

        yield return new WaitForSeconds(DEPOSIT_TIME_S);

        d.CompleteDepositMineral();
        
        TriggerOnCollectorFilled();
    }
}
