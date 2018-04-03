using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    [SerializeField]
    public readonly Vector2Int mapSize;
    [SerializeField]
    public readonly List<Vector2Int> hardWallsList;
    [SerializeField]
    public readonly List<Vector2Int> wallsList;
    [SerializeField]
    public readonly List<Vector2Int> bushList;
    [SerializeField]
    public readonly List<Vector2Int> waterList;
    [SerializeField]
    public readonly List<Vector2Int> eagleList;
    [SerializeField]
    public readonly List<Tank> tanksList;

    public GameState()
    {
        mapSize = new Vector2Int(0,0);
        hardWallsList = new List<Vector2Int>();
        wallsList = new List<Vector2Int>();
        bushList = new List<Vector2Int>();
        waterList = new List<Vector2Int>();
        eagleList = new List<Vector2Int>();
        tanksList = new List<Tank>();
    }
    
    public GameState(Vector2Int mapSize, List<Vector2Int> hardWallsList, List<Vector2Int> wallsList, 
        List<Vector2Int> bushList, List<Vector2Int> waterList, List<Vector2Int> eagleList, List<Tank> tanksList)
    {
        this.mapSize = mapSize;
        this.hardWallsList = new List<Vector2Int>(hardWallsList.ToArray());
        this.wallsList = new List<Vector2Int>(wallsList.ToArray());
        this.bushList = new List<Vector2Int>(bushList.ToArray());
        this.waterList = new List<Vector2Int>(waterList.ToArray());
        this.eagleList = new List<Vector2Int>(eagleList.ToArray());
        this.tanksList = new List<Tank>(tanksList.ToArray());
    }

    public override string ToString()
    {
        return String.Format("Game State: Map Size= {0};",
            mapSize);
    }
    
    public bool RemoveWall(Vector2Int wallCords)
    {
        if (wallsList.Contains(wallCords))
        {
            Log("Removing a Wall!");
            wallsList.Remove(wallCords);
            return true;
        }
        else
        {
            Log("Cannot remove a Wall! Wall does not eixist in GameState!");
            return false;
        }
    }

    public bool RemoveEagle(Vector2Int eagleCoords)
    {
        if (eagleList.Contains(eagleCoords))
        {
            Log("Removing a Wall!");
            eagleList.Remove(eagleCoords);
            if (eagleList.Count == 1)
                GameManager.instance.GameOver();
            return true;
        }
        else
        {
            Log("Cannot remove a Wall! Wall does not eixist in GameState!");
            return false;
        }
    }

    public bool RemoveTank(Tank tank)
    {
        if (tanksList.Contains(tank))
        {
            Log("Removing a Tank!");
            tanksList.Remove(tank);
            if (tanksList.Count == 1)
                GameManager.instance.GameOver();
            return true;
        }
        else
        {
            Log("Cannot remove a Tank! Tank does not eixist in GameState!");
            return false;
        }
    }
    
    public bool AddTank(Tank tank)
    {
        if (!tanksList.Contains(tank))
        {
            tanksList.Add(tank);
            Log("Adding a Tank!");
            return true;
        }
        else
        {
            Log("Cannot add a Tank! Tank instance already exists in GameState!");
            return false;
        }
    }

    [System.Diagnostics.Conditional("DEBUG_GAMESTATE")]
    private void Log(string message)
    {
        Debug.LogFormat("GameState: {0}", message);
    }
}

