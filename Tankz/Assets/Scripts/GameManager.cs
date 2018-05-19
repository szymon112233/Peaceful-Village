using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Singleton

	public static GameManager instance = null;
	
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
                
			//if not, set instance to this
			instance = this;
            
		//If instance already exists and it's not this:
		else if (instance != this)
                
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
            
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	#endregion

    [HideInInspector]
	public GameState gamestate;
    public GameMode gameMode = GameMode.FreeForAll;
	public NodeGrid nodeGrid = null;

	private CameraCenter cameraCenter;


    public static List<string> botNames = new List<string>()
    {
        "markiek1",
        "piorkows",
        "Crash",
        "Ranger",
        "Phobos",
        "Mynx",
        "Orbb",
        "Sarge",
        "Bitterman",
        "Grunt",
        "Hossman",
        "Daemia",
        "Bones",
    };

    private void Start()
	{
		cameraCenter = GetComponent<CameraCenter>();
		nodeGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGrid>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            UnityEngine.SceneManagement.SceneManager.LoadScene("test");
	}

	public void CenterCamera()
	{
		cameraCenter.CenterCamera();
    }

    public void GameOver()
    {
        if (gameMode == GameMode.FreeForAll)
            UnityEngine.SceneManagement.SceneManager.LoadScene("gameover");
    }

	public void StartGame()
	{
		if (nodeGrid == null)
			nodeGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGrid>();
		
		if (GameSetupPanel.instance.isCustomMap)
			nodeGrid.LoadMap(GameSetupPanel.instance.selectedMap);
		else
			nodeGrid.GenerateRandom();
		GameSetupPanel.instance.Show(false);
	}
}
