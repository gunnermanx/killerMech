using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBootstrap : MonoBehaviour {

    public Player player1;
    public Player player2;

    private void Start() {
        player1.Initialize(Player.Id.Drone1, Player.Team.Blue);
        player2.Initialize(Player.Id.Drone1, Player.Team.Red);
    }
}
