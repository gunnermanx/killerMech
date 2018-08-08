using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public Transform blueTeamParent;
    public Transform redTeamParent;

    private List<Player> blueTeam;
    private List<Player> redTeam;

	public void Awake()
	{
        blueTeam = new List<Player>();
        redTeam = new List<Player>();
	}

	void Update()
	{
			
	}

    public void Initialize()
    {
        foreach(Player.Team t in Enum.GetValues(typeof(Player.Team)))
        {
            foreach(Player.Id i in Enum.GetValues(typeof(Player.Id)))
            {
                CreatePlayer(i, t);
            }
        }
    }

    private void CreatePlayer(Player.Id id, Player.Team team)
    {
        GameObject playerGO = new GameObject("Player" + id.ToString());
        Player player = playerGO.AddComponent<Player>();

        if (team == Player.Team.Blue)
        {
            blueTeam.Add(player);
            playerGO.transform.SetParent(blueTeamParent);
        } else {
            redTeam.Add(player);
            playerGO.transform.SetParent(redTeamParent);
        }
    }
}
