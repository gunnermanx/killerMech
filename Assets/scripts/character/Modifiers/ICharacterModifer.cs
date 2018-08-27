using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterModifier {
    int GetPriority();
    Vector3 MoveModifier(
        Player.InputArgs input, 
        MovementController.CollisionInfo collisions,
        Vector3 v
   );
}
