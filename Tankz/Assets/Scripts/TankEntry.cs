using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankEntry : MonoBehaviour {

    public bool isHuman = false;
    public int teamNumber = 1;

    private Toggle humanTogle;
    private Dropdown teamDropdown;

    private void Awake()
    {
        humanTogle = gameObject.GetComponentInChildren<Toggle>();
        humanTogle.onValueChanged.AddListener(OnHumanToggleChanged);

        teamDropdown = gameObject.GetComponentInChildren<Dropdown>();
        teamDropdown.onValueChanged.AddListener(OnTeamDropdownChanged);

        GameSetupPanel.EventTeamsNumberChanged += OnTeamNumberChanged;
    }

    private void OnDestroy()
    {
        humanTogle.onValueChanged.RemoveAllListeners();
        teamDropdown.onValueChanged.RemoveAllListeners();
        GameSetupPanel.EventTeamsNumberChanged -= OnTeamNumberChanged;
    }

    private void OnHumanToggleChanged(bool value)
    {
        isHuman = value;
    }

    private void OnTeamDropdownChanged(int value)
    {
        teamNumber = (value+1);
    }

    private void OnTeamNumberChanged(int value)
    {
        teamDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 1; i <= value; i++)
        {
            options.Add(i.ToString());
        }
        teamDropdown.AddOptions(options);
    }
}
