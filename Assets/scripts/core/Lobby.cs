using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour {

    private Dictionary<Player.Id, bool> blueTeamReadyFlags;
    private Dictionary<Player.Id, bool> redTeamReadyFlags;

    public void Initialize() {
        RegisterDelegates();
    }

    private void OnPlayerActionPressed(Player.Team t, Player.Id i) {
        //Debug.Log("delegate triggered " + t.ToString() + " " + i.ToString());

        // TODO: update
        if (t == Player.Team.Blue && i == Player.Id.Drone1) {
            Debug.Log("==> delegate triggered " + t.ToString() + " " + i.ToString());

            UnregisterDelegates();

            GameManager.instance.StartMatch();
        }
    }

    private void RegisterDelegates() {
        List<Player> bluePlayers = PlayerManager.instance.GetPlayers(Player.Team.Blue);
        foreach(Player p in bluePlayers) {
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
