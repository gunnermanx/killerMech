using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject playerManagerPrefab;
    
    public static GameManager instance = null;

    private PlayerManager playerManager;

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

	private void Start()
	{        
        InitializeManagers();
	}

	private void InitializeManagers()
    {
        GameObject playerManagerGO = Instantiate(playerManagerPrefab);
        playerManager = playerManagerGO.GetComponent<PlayerManager>();
        playerManager.Initialize();
        playerManagerGO.transform.parent = this.transform;
    }

    private void StartMatch()
    {
        Debug.Log("Starting match");
        SceneManager.LoadScene("Match", LoadSceneMode.Single);
    }

	private void Update()
	{
        if (Input.GetButtonDown("Blue_Ace_action"))
        {
            StartMatch();    
        }
	}
}
