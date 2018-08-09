using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour {

    public GameObject dronePrefab;
    public GameObject bruiserPrefab;
    public GameObject acePrefab;

    public GameObject stagePrefab;

    private GameObject stage;

    private int blueAceLives = 3;
    private int redAceLives = 3;



    private void Update() {

    }

    public void Initialize() {
        SpawnStage();
        SpawnMinerals();
        SpawnUpgradeStations();

        RegisterDelegates();
    }

    private void SpawnMechForPlayer(Player p) {
        GameObject mechGO;
        if (p.id != Player.Id.Ace) {
            mechGO = (GameObject)Instantiate(bruiserPrefab);
            AbstractMech mech = mechGO.GetComponent<Bruiser>();
            p.AssignMech(mech);
        } else {
            mechGO = (GameObject)Instantiate(acePrefab);
            AbstractMech mech = mechGO.GetComponent<Ace>();
            p.AssignMech(mech);
        }
        mechGO.name = p.team.ToString() + "_" + p.id.ToString();
    }

    private void SpawnStage() {
        stage = (GameObject)Instantiate(stagePrefab);
    }

    private void SpawnMinerals() {

    }

    private void SpawnUpgradeStations() {

    }
    
    private void OnPlayerActionPressed(Player.Team t, Player.Id i) {
        Debug.Log("match => delegate triggered " + t.ToString() + " " + i.ToString());

        Player p = PlayerManager.instance.GetPlayer(t, i);
        if (!p.HasMech()) {
            if (i == Player.Id.Ace) {
                if ((t == Player.Team.Blue && blueAceLives > 0) ||
                    (t == Player.Team.Red && redAceLives > 0)) {
                    SpawnMechForPlayer(p);
                }
            } else {
                SpawnMechForPlayer(p);
            }
            
        }
    }

    private void RegisterDelegates() {
        List<Player> bluePlayers = PlayerManager.instance.GetPlayers(Player.Team.Blue);
        foreach (Player p in bluePlayers) {
            p.OnActionPressed += OnPlayerActionPressed;
        }

        List<Player> redPlayers = PlayerManager.instance.GetPlayers(Player.Team.Red);
        foreach (Player p in redPlayers) {
            p.OnActionPressed += OnPlayerActionPressed;
        }
    }

    private void UnregisterDelegates() {
        List<Player> bluePlayers = PlayerManager.instance.GetPlayers(Player.Team.Blue);
        foreach (Player p in bluePlayers) {
            p.OnActionPressed -= OnPlayerActionPressed;
        }

        List<Player> redPlayers = PlayerManager.instance.GetPlayers(Player.Team.Red);
        foreach (Player p in redPlayers) {
            p.OnActionPressed += OnPlayerActionPressed;
        }
    }

    private void OnDisable() {
        UnregisterDelegates();
    }
}
