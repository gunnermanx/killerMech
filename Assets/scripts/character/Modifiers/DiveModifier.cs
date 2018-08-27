using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveModifier : MonoBehaviour, ICharacterModifier {
    [SerializeField] public int priority;
    [SerializeField] private float diveVelocity;
    private bool diving = false;    

    public int GetPriority() {
        return priority;
    }

    public Vector3 MoveModifier(
        Player.InputArgs input,
        MovementController.CollisionInfo collisions,
        Vector3 v) {

        Vector3 newV = v;
        if (input.movement.y < 0 && collisions.below == false) {
            newV.y = diveVelocity;
            diving = true;
        } 
        else if (diving && input.movement.y >= -Mathf.Epsilon && input.movement.y <= Mathf.Epsilon) {
            newV.y = diveVelocity * 0.25f;
            diving = false;
        } if (collisions.below == true) {
            diving = false;
        }

        GetComponent<Animator>().SetBool("Dive", diving);

        return newV;
    }
}
