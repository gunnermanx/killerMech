using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralSpawner : MonoBehaviour {
    private const int MAX_MINERAL_SLOTS = 6;

    public GameObject mineralPrefab;
    public List<Transform> mineralSpawnLocations = new List<Transform>(MAX_MINERAL_SLOTS);
    
    public void SpawnMineral(int index) {
        GameObject mineralGO = Instantiate(mineralPrefab);
        mineralGO.transform.parent = mineralSpawnLocations[index];
        mineralGO.transform.localPosition = Vector3.zero;
    }

    public void InitializeMinerals() {
        for (int i = 0; i < MAX_MINERAL_SLOTS; i++) {
            SpawnMineral(i);
        }
    }
}
