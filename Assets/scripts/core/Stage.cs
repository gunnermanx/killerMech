using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {
    public List<MineralSpawner> mineralSpawners;
    public Base blueBase;
    public Base redBase;

    public BoxCollider2D leftWrapEntrance;
    public BoxCollider2D leftWrapExit;
    public BoxCollider2D rightWrapEntrance;
    public BoxCollider2D rightWrapExit;

    public Transform blueDrone1SpawnPoint;
    public Transform blueDrone2SpawnPoint;
    public Transform blueDrone3SpawnPoint;
    public Transform blueDrone4SpawnPoint;
    public Transform blueAceSpawnPoint;

    public Transform redDrone1SpawnPoint;
    public Transform redDrone2SpawnPoint;
    public Transform redDrone3SpawnPoint;
    public Transform redDrone4SpawnPoint;
    public Transform redAceSpawnPoint;


    private LayerMask mechLayer;

    public void Initialize() {
        blueBase.Initalize();
        redBase.Initalize();
        InitializeMineralSpawners();
        
        mechLayer = LayerMask.GetMask("Mech");

        if (blueDrone1SpawnPoint == null ) {
            Debug.Log("DAFUQ");
        }
    }

    private void InitializeMineralSpawners() {
        foreach(MineralSpawner spawner in mineralSpawners) {
            spawner.InitializeMinerals();
        }
    }

    public void ListenForEconWinCondition(Base.AllCollectorsFilled onAllCollectorsFilled) {
        blueBase.OnAllCollectorsFilled += onAllCollectorsFilled;
        redBase.OnAllCollectorsFilled += onAllCollectorsFilled;
    }

    void Update() {
        CheckLeftWrapEntrance();
        CheckRightWrapEntrance();

        
    }

    // w/e ghetto as shit
    public Vector3 GetSpawnPoint(Player.Team t, Player.Id i) {        
        switch (t) {
            case Player.Team.Blue:
                switch (i) {
                    case Player.Id.Ace:
                        return blueAceSpawnPoint.position;                        
                    case Player.Id.Drone1:
                        return blueDrone1SpawnPoint.position;
                    case Player.Id.Drone2:
                        return blueDrone2SpawnPoint.position;
                    case Player.Id.Drone3:
                        return blueDrone3SpawnPoint.position;
                    case Player.Id.Drone4:
                        return blueDrone4SpawnPoint.position;
                    default:
                        return Vector3.zero;
                }             
            case Player.Team.Red:
                switch (i) {
                    case Player.Id.Ace:
                        return redAceSpawnPoint.position;
                    case Player.Id.Drone1:
                        return redDrone1SpawnPoint.position;
                    case Player.Id.Drone2:
                        return redDrone2SpawnPoint.position;
                    case Player.Id.Drone3:
                        return redDrone3SpawnPoint.position;
                    case Player.Id.Drone4:
                        return redDrone4SpawnPoint.position;
                    default:
                        return Vector3.zero;
                }               
            default:
                return Vector3.zero;
        }
        
    }

    private void CheckLeftWrapEntrance() {
        Collider2D[] results = new Collider2D[10];
        ContactFilter2D f = new ContactFilter2D();
        f.SetLayerMask(mechLayer);

        Physics2D.OverlapBox(leftWrapEntrance.bounds.center, leftWrapEntrance.bounds.size, 0f, f, results);
        foreach( Collider2D c in results ) {
            if (c == null) continue;
            AbstractMech mech = c.gameObject.GetComponent<AbstractMech>();
            if (mech != null) {
                mech.transform.position = new Vector3(rightWrapExit.transform.position.x, mech.transform.position.y);
            }
        }        
    }

    private void CheckRightWrapEntrance() {
        Collider2D[] results = new Collider2D[10];
        ContactFilter2D f = new ContactFilter2D();
        f.SetLayerMask(mechLayer);

        Physics2D.OverlapBox(rightWrapEntrance.bounds.center, rightWrapEntrance.bounds.size, 0f, f, results);
        foreach (Collider2D c in results) {
            if (c == null) continue;
            AbstractMech mech = c.gameObject.GetComponent<AbstractMech>();
            if (mech != null) {
                mech.transform.position = new Vector3(leftWrapExit.transform.position.x, mech.transform.position.y);
            }
        }

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(leftWrapEntrance.bounds.center, new Vector3(leftWrapEntrance.bounds.size.x, leftWrapEntrance.bounds.size.y, 0.0f));
        Gizmos.DrawCube(rightWrapEntrance.bounds.center, new Vector3(rightWrapEntrance.bounds.size.x, rightWrapEntrance.bounds.size.y, 0.0f));

        Gizmos.color = Color.green;
        Gizmos.DrawCube(leftWrapExit.bounds.center, new Vector3(leftWrapExit.bounds.size.x, leftWrapExit.bounds.size.y, 0.0f));
        Gizmos.DrawCube(rightWrapExit.bounds.center, new Vector3(rightWrapExit.bounds.size.x, rightWrapExit.bounds.size.y, 0.0f));
    }
}
