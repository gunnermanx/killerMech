using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedShrine : AbstractBaseShrine {

    protected override bool CanEnterShrine(Drone d) {
        return !d.HasSpeedBuff() && d.IsHoldingMineral();
    }

    protected override void BeforeWait(Drone d) {
        Debug.Log("Starting speed buff process of drone: " + d.name);
        d.StartSpeedBuff();
    }

    protected override void AfterWait(Drone d) {
        Debug.Log("Completing speed buff process of drone: " + d.name);
        d.CompleteSpeedBuff();
    }
}
