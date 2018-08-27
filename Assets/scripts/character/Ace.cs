using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ace : AbstractMech {
    public HitBox hitBox;

    private void Awake() {
        base.Awake();
        Initialize();
        //InitializeHitBox();        
    }
}
