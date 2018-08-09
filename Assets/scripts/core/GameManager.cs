using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private const string LOBBY_SCENE = "LobbyScene";
    private const string MATCH_SCENE = "MatchScene";

    public GameObject playerManagerPrefab;

    public GameObject matchPrefab;
    public GameObject lobbyPrefab;

    public static GameManager instance = null;

    private PlayerManager playerManager;
    private Match match;
    private Lobby lobby;

    public void StartMatch() {
        SceneManager.LoadScene(MATCH_SCENE, LoadSceneMode.Single);
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start() {
        InitializeManagers();
        CreateLobby();
    }

    private void InitializeManagers() {
        GameObject playerManagerGO = Instantiate(playerManagerPrefab);
        playerManager = playerManagerGO.GetComponent<PlayerManager>();
        playerManager.Initialize();
        playerManagerGO.transform.parent = this.transform;
    }

    private void CreateLobby() {
        GameObject lobbyGO = Instantiate(lobbyPrefab);
        lobby = lobbyGO.GetComponent<Lobby>();
        lobby.Initialize();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoaded: " + scene.name);
        if (scene.name == MATCH_SCENE) {
            GameObject matchMangerGO = Instantiate(matchPrefab);
            match = matchMangerGO.GetComponent<Match>();
            match.Initialize();
        }
    }
}
