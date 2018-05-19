using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

        Init();
	}

    #endregion

    public static UnityAction<int> EventTeamsNumberChanged;
    static System.Random rnd = new System.Random();

    public Dropdown mapDropdown = null;
	public List<string> maps = new List<string>();
	public bool isCustomMap;
	public string selectedMap = "";
    public InputField teamsInputNumber;
    public List<TankEntry> tankEntries = new List<TankEntry>();
    public GameObject scrollContent;

    public GameObject EmptyTankEntryPrefab;

    private int teamsNumber = 2;


    private void Init()
    {
        teamsInputNumber.onEndEdit.AddListener(OnTeamsNumberChanged);
    }

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

    private void OnTeamsNumberChanged(string value)
    {
        int teams = 0;
        int.TryParse(value,out teams);
        if (teams == 0)
        {
            teamsInputNumber.text = teamsNumber.ToString();
            return;
        }
        teamsNumber = teams;
        EventTeamsNumberChanged.Invoke(teamsNumber);
    }
    

    // Tak wiem, bardzo brzydkie funkcji :(
    public void AddTankEntry()
    {
        GameObject go = GameObject.Instantiate(EmptyTankEntryPrefab, scrollContent.transform);
        TankEntry entry = go.GetComponent<TankEntry>();
        tankEntries.Add(entry);
  
        int r = rnd.Next(GameManager.botNames.Count);
        go.GetComponentInChildren<Text>().text = GameManager.botNames[r];

        RectTransform scrollRT = scrollContent.GetComponent<RectTransform>();
        scrollRT.sizeDelta = new Vector2(scrollRT.sizeDelta.x, scrollRT.sizeDelta.y + 30);

        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(go.GetComponent<RectTransform>().anchoredPosition.x, -scrollRT.sizeDelta.y + 15);

        go.GetComponentInChildren<Button>().onClick.AddListener(() => { RemoveTankEntry(tankEntries.IndexOf(entry)); });
    }

    private void RemoveTankEntry(int entryNumber)
    {
        RectTransform scrollRT = scrollContent.GetComponent<RectTransform>();
        scrollRT.sizeDelta = new Vector2(scrollRT.sizeDelta.x, scrollRT.sizeDelta.y - 30);

        for (int i = entryNumber + 1; i < tankEntries.Count; i++)
        {
            tankEntries[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(tankEntries[i].GetComponent<RectTransform>().anchoredPosition.x,
                tankEntries[i].GetComponent<RectTransform>().anchoredPosition.y + 30);
        }

        Destroy(tankEntries[entryNumber].gameObject);
        tankEntries.RemoveAt(entryNumber);
    }

}
