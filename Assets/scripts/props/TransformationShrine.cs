using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationShrine : AbstractBaseShrine {
    [SerializeField] private GameObject blueBruiserPrefab;
    [SerializeField] private GameObject redBruiserPrefab;

    protected override bool CanEnterShrine(Drone d) {
        return d.IsHoldingMineral();
    }

    protected override void BeforeWait(Drone d) {
        Debug.Log("Starting transformation process of drone: " + d.name);        
        d.StartTransformation();
    }

    protected override void AfterWait(Drone d) {
        Debug.Log("Completing the transformation process of drone: " + d.name);
        CreateBruiser(d);
        d.CompleteTransformation();
    }

    private void CreateBruiser(Drone d) {
        // Create new bruiser
        GameObject bruiserGO = (d.team == Player.Team.Blue) ?
            Instantiate(blueBruiserPrefab) :
            Instantiate(redBruiserPrefab);
        
        Bruiser bruiser = bruiserGO.GetComponent<Bruiser>();
        
        bruiser.owner = d.owner;
        bruiser.owner.AssignMech(bruiser);

        bruiser.transform.parent = d.transform.parent;
        bruiser.transform.position = d.transform.position;

        if (d.HasSpeedBuff()) {
            bruiser.SetSpeedBuff();
        }
    }
}
