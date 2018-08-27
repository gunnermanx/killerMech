using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour {

    [SerializeField] private GameObject blueDronePrefab;
    [SerializeField] private GameObject blueBruiserPrefab;
    [SerializeField] private GameObject blueAcePrefab;
    [SerializeField] private GameObject redDronePrefab;
    [SerializeField] private GameObject redBruiserPrefab;
    [SerializeField] private GameObject redAcePrefab;

    public GameObject stagePrefab;

    private Stage stage;

    private int blueAceLives = 3;
    private int redAceLives = 3;
    

    public void Initialize() {
        SpawnStage();
        RegisterDelegates();
    }

    private void SpawnMechForPlayer(Player p) {
        GameObject mechGO;
        AbstractMech mech;
        
        Vector3 spawnLocation = stage.GetSpawnPoint(p.team, p.id);

        if (p.id != Player.Id.Ace) {
            mechGO = (p.team == Player.Team.Blue) ?
                (GameObject)Instantiate(blueDronePrefab, spawnLocation, Quaternion.identity) :
                (GameObject)Instantiate(redDronePrefab, spawnLocation, Quaternion.identity);
            mech = mechGO.GetComponent<Drone>();
        } else {
            mechGO = (p.team == Player.Team.Blue) ?
                (GameObject)Instantiate(blueBruiserPrefab, spawnLocation, Quaternion.identity) :
                (GameObject)Instantiate(redBruiserPrefab, spawnLocation, Quaternion.identity);
            mech = mechGO.GetComponent<Ace>();
        }

        p.AssignMech(mech);
        mech.owner = p;
        mechGO.name = p.team.ToString() + "_" + p.id.ToString();        
    }

    private void SpawnStage() {
        GameObject stageGO = (GameObject)Instantiate(stagePrefab);
        stage = stageGO.GetComponent<Stage>();
        stage.Initialize();
        stage.ListenForEconWinCondition(HandleEconVictoryReached);       
    }

    private void HandleEconVictoryReached(Player.Team t) {
        Debug.Log("YAY team " + t.ToString() + " has reached econmoic victory");
    }

    private void HandleMilitaryVictoryReached(Player.Team t) {

    }

    private void HandleTugOfWarVictoryReached(Player.Team t) {

    }
    
    private void OnPlayerActionPressed(Player.Team t, Player.Id i) {
        //Debug.Log("match => delegate triggered " + t.ToString() + " " + i.ToString());

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
            p.OnActionPressed -= OnPlayerActionPressed;
        }
    }

    private void OnDisable() {
        UnregisterDelegates();
    }
}
