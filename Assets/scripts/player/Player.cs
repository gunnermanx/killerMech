using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Id
    {
        Ace,
        Drone1,
        Drone2,
        Drone3,
        Drone4,
    }

    public enum Team
    {
        Blue,
        Red
    }

    public Team team { get; private set; }
    public Id id { get; private set; }

    public void Initialize(Id id, Team team)
    {
        this.id = id;
        this.team = team;
    }

    private void InitializeInput(AbstractMech mech)
    {
        PlayerInput input = gameObject.AddComponent<PlayerInput>();
        input.Initialize(mech, team.ToString() + "_" + id.ToString() + "_");
    }
}