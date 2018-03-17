using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameState
{
    [SerializeField]
    private Vector2Int mapSize;
    [SerializeField]
    private List<Vector2Int> hardWallsList;
    [SerializeField]
    private List<Vector2Int> wallsList;
    [SerializeField]
    private List<GameObject> tanksList;
    
    public GameState(Vector2Int mapSize, List<Vector2Int> hardWallsList, List<Vector2Int> wallsList, List<GameObject> tanksList)
    {
        this.mapSize = mapSize;
        this.hardWallsList = new List<Vector2Int>(hardWallsList.ToArray());
        this.wallsList = new List<Vector2Int>(wallsList.ToArray());
        this.tanksList = new List<GameObject>(tanksList.ToArray());
    }

    public override string ToString()
    {
        return String.Format("Game State: Map Size= {0}; hardWallsList = {1}; wallsList = {2}; tanksList = {3}",
            mapSize, hardWallsList, wallsList, tanksList);
    }

    public bool RemoveWall(Vector2Int wallCords)
    {
        if (wallsList.Contains(wallCords))
        {
            wallsList.Remove(wallCords);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool RemoveTank(GameObject tank)
    {
        if (tanksList.Contains(tank))
        {
            tanksList.Remove(tank);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool AddTank(GameObject tank)
    {
        if (!tanksList.Contains(tank))
        {
            tanksList.Add(tank);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    
}

[Serializable]
public struct GameTank
{
    public int team;
    public Vector2 position;

    public override string ToString()
    {
        return String.Format("GameTank: Team = {0}, Position ={1}", team, position);
    }
}

