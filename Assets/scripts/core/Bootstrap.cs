using UnityEngine;
using System.Collections;

public class Bootstrap : MonoBehaviour {
    public GameObject gameManager;

    void Awake() {
        if (GameManager.instance == null) {
            Instantiate(gameManager);
        }
    }
}
