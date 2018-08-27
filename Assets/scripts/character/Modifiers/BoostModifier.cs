using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostModifier : MonoBehaviour, ICharacterModifier {
    [SerializeField] public int priority;
    [SerializeField] private float boostVelocity;
    [SerializeField] private float maxBoostVelocity;
    
    public int GetPriority() {
        return priority;
    }

    public Vector3 MoveModifier(
        Player.InputArgs input, 
        MovementController.CollisionInfo collisions,
        Vector3 v) {

        Vector3 newV = v;
        if (input.actionPressed) {                       
            newV.y += boostVelocity;
            if (newV.y > maxBoostVelocity) newV.y = maxBoostVelocity;
           
        }
        return newV;
    }


}
