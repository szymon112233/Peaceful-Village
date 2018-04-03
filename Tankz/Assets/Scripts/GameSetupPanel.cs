using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameSetupPanel : MonoBehaviour
{
	
	#region Singleton

	public static GameSetupPanel instance = null;
	
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

	public Dropdown mapDropdown = null;
	public List<string> maps = new List<string>();
	public bool isCustomMap;
	public string selectedMap = "";

	public void Show(bool value)
	{
		gameObject.SetActive(value);
	}
	private void ShowMapChoiceDropdown(bool value)
	{
		mapDropdown.gameObject.SetActive(value);
		if (value)
		{
			LoadAvailableMaps();
			selectedMap = maps[mapDropdown.value];
		}
			
	}

	private void LoadAvailableMaps()
	{
		maps = new List<string>();
		var files = Directory.GetFiles(Application.dataPath + "/Maps");
		foreach (string file in files)
		{
			Debug.LogFormat("Found File: {0}", file);
			if(file.Contains(".csv") && !file.Contains(".meta"))
				maps.Add(file);
		}
		var mapNames = new List<string>();
		foreach (string map in maps)
		{
			mapNames.Add(map.Replace(Application.dataPath + "/Maps\\", "").Replace(".csv", ""));
		}
		
		mapDropdown.ClearOptions();
		
		mapDropdown.AddOptions(mapNames);
	}

	public void OnMapDropdownChanged(int choice)
	{
		selectedMap = maps[choice];
	}
	
	public void OnCustomMapSelected(bool choice)
	{
		isCustomMap = choice;
		ShowMapChoiceDropdown(choice);
	}

	public void OnStartButtonClicked()
	{
		GameManager.instance.StartGame();
	}
}
