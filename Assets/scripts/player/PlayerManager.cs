using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
    public Transform blueTeamParent;
    public Transform redTeamParent;

    private Dictionary<Player.Id, Player> blueTeam;
    private Dictionary<Player.Id, Player> redTeam;

    public static PlayerManager instance = null;

    public void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        blueTeam = new Dictionary<Player.Id, Player>();
        redTeam = new Dictionary<Player.Id, Player>();
    }

    void Update() {

    }

    public void Initialize() {
        foreach (Player.Id i in Enum.GetValues(typeof(Player.Id))) {
            CreatePlayer(i, Player.Team.Blue);
        }
        foreach (Player.Id i in Enum.GetValues(typeof(Player.Id))) {
            CreatePlayer(i, Player.Team.Red);
        }
    }

    public List<Player> GetPlayers(Player.Team t) {
        return (t == Player.Team.Blue) ? 
            new List<Player>(blueTeam.Values) :
            new List<Player>(redTeam.Values);
    }

    public Player GetPlayer(Player.Team t, Player.Id i) {
        return (t == Player.Team.Blue) ? blueTeam[i] : redTeam[i];
    }

    private void CreatePlayer(Player.Id id, Player.Team team) {
        GameObject playerGO = new GameObject("Player" + team.ToString() + id.ToString());
        Player player = playerGO.AddComponent<Player>();
        player.Initialize(id, team);

        if (team == Player.Team.Blue) {
            blueTeam.Add(id, player);
            playerGO.transform.SetParent(blueTeamParent);
        } else {
            redTeam.Add(id, player);
            playerGO.transform.SetParent(redTeamParent);
        }       
    }
}
