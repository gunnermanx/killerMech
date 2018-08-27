using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBaseShrine : MonoBehaviour {
    public BoxCollider2D droneCollider;
    public BoxCollider2D aceCollider;

    public LayerMask droneLayer;
    public LayerMask aceLayer;

    private Player.Team ownedBy = Player.Team.Neutral;

    private IEnumerator shrineTimedCoroutine;
    private bool open;

    private const float SHRINE_TIME_S = 2.0f;

    private void Awake() {
        open = true;
    }

    void FixedUpdate() {
        if (CheckCollisionWithDrone()) return;
        if (CheckCollisionWithAce()) return;
    }

    private bool CheckCollisionWithDrone() {
        if (open) {
            Collider2D c = Physics2D.OverlapBox(droneCollider.transform.position, droneCollider.size, 0f, droneLayer);
            if (c != null) {
                Drone drone = c.gameObject.transform.parent.GetComponent<Drone>();
                if (drone != null && (ownedBy == drone.team || ownedBy == Player.Team.Neutral)) {
                    if (CanEnterShrine(drone)) {
                        StartTimedCoroutine(drone);
                        return true;
                    }
                    
                }
            }
        }
        return false;
    }

    private bool CheckCollisionWithAce() {
        Collider2D c = Physics2D.OverlapBox(aceCollider.transform.position, aceCollider.size, 0f, aceLayer);
        if (c != null) {
            AbstractMech mech = c.gameObject.GetComponent<AbstractMech>();
            if (mech != null) {
                ChangeOwnership(mech.team);
                return true;
            }
        }
        return false;
    }

    private void ChangeOwnership(Player.Team t) {
        ownedBy = t;
        // TODO: do some color/state change

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;        
        Gizmos.DrawWireCube(droneCollider.transform.position, droneCollider.size);
    }

    private void StartTimedCoroutine(Drone d) {        
        shrineTimedCoroutine = ShrineCoroutine(d);
        StartCoroutine(shrineTimedCoroutine);
    }

    private void StopTransformation(Drone d) {
        // replace the drone with a bruiser
    }
    
    private IEnumerator ShrineCoroutine(Drone d) {
        // Close the transformation shrine
        open = false;

        BeforeWait(d);       
        yield return new WaitForSeconds(SHRINE_TIME_S);
        AfterWait(d);

        // Open the transformation shrine
        open = true;
    }

    protected abstract bool CanEnterShrine(Drone d);
    protected abstract void BeforeWait(Drone d);
    protected abstract void AfterWait(Drone d);
}
