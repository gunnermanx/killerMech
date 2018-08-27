using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

    public List<MineralCollector> mineralCollectors;
    public Player.Team team;

    public delegate void AllCollectorsFilled(Player.Team t);
    public AllCollectorsFilled OnAllCollectorsFilled;
    private void TriggerAllCollectorsFilled() { if (OnAllCollectorsFilled != null) OnAllCollectorsFilled(team); }

    private int mineralsCollected = 0;

    public void Initalize() {
        InitializeMineralCollectors();
    }

    private void InitializeMineralCollectors() {
        foreach (MineralCollector collector in mineralCollectors) {
            collector.OnCollectorFilled += OnCollectorFilled;
        }
    }

    private void OnCollectorFilled(Player.Team t) {
        if (++mineralsCollected == mineralCollectors.Count) {
            TriggerAllCollectorsFilled();
        } 
    }
}
