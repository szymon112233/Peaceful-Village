﻿using System;
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
    public readonly List<GameObject> tanksList;
    
    public GameState()
    {
        mapSize = new Vector2Int(0,0);
        hardWallsList = new List<Vector2Int>();
        wallsList = new List<Vector2Int>();
        bushList = new List<Vector2Int>();
        waterList = new List<Vector2Int>();
        tanksList = new List<GameObject>();
    }
    
    public GameState(Vector2Int mapSize, List<Vector2Int> hardWallsList, List<Vector2Int> wallsList, 
        List<Vector2Int> bushList, List<Vector2Int> waterList, List<GameObject> tanksList)
    {
        this.mapSize = mapSize;
        this.hardWallsList = new List<Vector2Int>(hardWallsList.ToArray());
        this.wallsList = new List<Vector2Int>(wallsList.ToArray());
        this.bushList = new List<Vector2Int>(bushList.ToArray());
        this.waterList = new List<Vector2Int>(waterList.ToArray());
        this.tanksList = new List<GameObject>(tanksList.ToArray());
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
    
    public bool RemoveTank(GameObject tank)
    {
        if (tanksList.Contains(tank))
        {
            Log("Removing a Tank!");
            tanksList.Remove(tank);
            return true;
        }
        else
        {
            Log("Cannot remove a Tank! Tank does not eixist in GameState!");
            return false;
        }
    }
    
    public bool AddTank(GameObject tank)
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

    [System.Diagnostics.Conditional("DEBUG")]
    private void Log(string message)
    {
        Debug.LogFormat("GameState: {0}", message);
    }
}

