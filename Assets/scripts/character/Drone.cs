using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : AbstractMech {

    public LayerMask mineralLayer;
    public LayerMask mineralHeldLayer;
    public Transform mineralParent;
    
    private GameObject heldMineral = null;
    private Vector3 pickupBox;
    
    private Motherlode motherlode = null;

    private void Awake() {
        base.Awake();
        Initialize();
        pickupBox = new Vector3(1.0f, 1.0f, 0.0f);       
    }

    public override void Move(Player.InputArgs input) {
        if (motherlode != null) {
            input.movement.y = 0.0f;
            if (input.actionPressed) {
                StopRidingMotherlode();
            } else {
                input.movement.x = 0.0f;
            }
        }
        base.Move(input);
    }

    private void FixedUpdate() {
        Collider2D c = Physics2D.OverlapBox(gameObject.transform.position, pickupBox, 0f, mineralLayer);
        if (c != null && !IsHoldingMineral()) {
            PickupMineral(c.gameObject);
        }
    }

    public bool IsHoldingMineral() {
        return heldMineral != null;
    }




    public void StartRidingMotherlode(Motherlode motherlode) {
        this.motherlode = motherlode;
        lockYMovement = true;
    }

    public void StopRidingMotherlode() {
        lockYMovement = false;
        motherlode.DettachDrone(this);
        this.motherlode = null;
    }




    public void StartTransformation() {
        SpendMineral();
        ignoresInput = true;
    }

    public void CompleteTransformation() {
        //DESTROY THIS
        Destroy(gameObject);
    }
    
    public void StartSpeedBuff() {
        SpendMineral();
        ignoresInput = true;
    }

    public void CompleteSpeedBuff() {
        hasSpeedBuff = true;
        ignoresInput = false;
        Initialize();
    }

    public bool HasSpeedBuff() {
        return hasSpeedBuff;
    }

    public void StartDepositMineral() {
        SpendMineral();
        ignoresInput = true;
        // Turn invulnerable
    }

    public void CompleteDepositMineral() {
        ignoresInput = false;
    }

    private void PickupMineral(GameObject mineral) {
        mineral.transform.parent = mineralParent;
        mineral.transform.localPosition = Vector3.zero;
        mineral.layer = LayerMask.NameToLayer("MineralHeld");
        heldMineral = mineral;

        
    }

    private void DropMineral() {
        heldMineral.transform.parent = gameObject.transform.parent;
        heldMineral.layer = LayerMask.NameToLayer("Mineral");
        heldMineral = null;
    }

    private void SpendMineral() {
        Destroy(heldMineral);
        heldMineral = null;
    }
}
