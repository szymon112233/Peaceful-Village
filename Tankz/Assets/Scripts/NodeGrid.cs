using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class NodeGrid : MonoBehaviour {
    
    public Vector2Int gridSize = new Vector2Int();
    public float nodeSize = 0.0f;
    public GameObject nodePrefab = null;
    public GameObject wallPrefab = null;
    public GameObject enemyPrefab = null;
    public GameObject[] playerPrefabs = null;
    public int numberOfWallsToGenerate = 10;
    public int numberOfEnemiesToGenerate = 1;
    public MapNode[,] MapNodes = null;
    public GameObject[] environmentPrefabs = null;

    public bool spawnPlayers, spawnEnemies;

    List<GameObject> players = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();

    /*
    public void GenerateRandom()
    {
        Clear();
        GenerateGrid();
        GenerateOuterWalls();
        GenerateRandomWalls(numberOfWallsToGenerate);
        GeneratePlayers();
        GenerateEnemies();
    }*/

    public void LoadMap()
    {
        Clear();
        string path = EditorUtility.OpenFilePanel("Select map file", "", "csv");
        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            string[] lines = content.Split('\n');
            gridSize.x = lines[0].Split(',').Length;
            gridSize.y = lines.Length - 1;
            GenerateGrid();
            GenerateOuterWalls();
            for (int i = gridSize.y - 1; i >= 0; i--)
            {
                string[] indices = lines[gridSize.y - 1 - i].Split(',');
                int index;
                for (int j = 0; j < gridSize.x; j++)
                {
                    if (MapNodes[j, i].objectOnNode == null && (index = int.Parse(indices[j])) != -1)
                        MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                }
            }
            if (spawnPlayers)
            GeneratePlayers();
            if (spawnEnemies)
            GenerateEnemies();
        }
        else
            Debug.LogError("Can't read from file!");
    }

    void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        foreach (GameObject player in players)
            Destroy(player);
        foreach (GameObject enemy in enemies)
            Destroy(enemy);
    }

    void GenerateRandom()
    {
        Clear();
        GenerateGrid();
	    List<Vector2Int> hardWalls = GenerateOuterWalls();
	    List<Vector2Int> walls = GenerateRandomWalls(numberOfWallsToGenerate);
	    List<GameTank> tanks = new List<GameTank>();
	    tanks.AddRange(GeneratePlayers());
	    tanks.AddRange(GenerateEnemies());
	    
	    GameManager.instance.gamestate = new GameState(gridSize, hardWalls, walls, tanks);
	    
	    Debug.Log(GameManager.instance.gamestate);
	}
	
	void GenerateGrid()

    {
        MapNodes = new MapNode[gridSize.x, gridSize.y];
        for (int i = gridSize.y - 1; i >= 0; i--)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                GameObject go = Instantiate(nodePrefab, new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                MapNodes[j, i] = go.GetComponent<MapNode>();
            }
        }
    }

    List<Vector2Int> GenerateOuterWalls()
    {
        List<Vector2Int> walls = new List<Vector2Int>();
        int i = 0;
        for (int j = 0; j< gridSize.x; j++)
        {
            (MapNodes[j, i].objectOnNode = Instantiate(wallPrefab, MapNodes[j, i].transform)).tag = "Unbreakable";
            walls.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
        }

        i = gridSize.y -1;
        for (int j = 0; j < gridSize.x; j++)
        {
            (MapNodes[j, i].objectOnNode = Instantiate(wallPrefab, MapNodes[j, i].transform)).tag = "Unbreakable";
            walls.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
        }

        i = 0;
        for (int j = 0; j < gridSize.y; j++)
        {
            if (MapNodes[i, j].isFree())
            {
                (MapNodes[i, j].objectOnNode = Instantiate(wallPrefab, MapNodes[i, j].transform)).tag = "Unbreakable";
                walls.Add(new Vector2Int((int)MapNodes[i, j].transform.position.x, (int)MapNodes[i, j].transform.position.y));
            }   
        }

        i = gridSize.x - 1;
        for (int j = 0; j < gridSize.y; j++)
        {
            if (MapNodes[i, j].isFree())
            {
                (MapNodes[i, j].objectOnNode = Instantiate(wallPrefab, MapNodes[i, j].transform)).tag = "Unbreakable";
                walls.Add(new Vector2Int((int)MapNodes[i, j].transform.position.x, (int)MapNodes[i, j].transform.position.y));
            }  
        }

        return walls;
    }

    List<Vector2Int> GenerateRandomWalls(int count)
    {
        List<Vector2Int> walls = new List<Vector2Int>();
        while (count > 0)
        {
            int randomX = Random.Range(0, gridSize.x);
            int randomY = Random.Range(0, gridSize.y);
            if (MapNodes[randomX, randomY].isFree())
            {
                MapNodes[randomX, randomY].objectOnNode = Instantiate(wallPrefab, MapNodes[randomX, randomY].transform);
                count--;
                walls.Add(new Vector2Int((int)MapNodes[randomX, randomY].transform.position.x, (int)MapNodes[randomX, randomY].transform.position.y));
            }     
        }
        return walls;
    }

    List<GameTank> GeneratePlayers()
    {
        List<GameTank> tanks = new List<GameTank>();
        bool isGenerated = false;
        int counter = 0;
        while (counter <= 1)
        {
            int[] x =  { 2, gridSize.x - 2 };
            int[] y =  { 2, gridSize.y - 2 };
            GameObject player = Instantiate(playerPrefabs[counter], new Vector3(x[counter] * nodeSize, y[counter++] * nodeSize, 0), new Quaternion());

            players.Add(player);

            GameTank tank = new GameTank();
            tank.team = 0;
            tank.position = new Vector2(player.transform.position.x, player.transform.position.y);
            tanks.Add(tank);

        }
        return tanks;
    }
    
    List<GameTank> GenerateEnemies()
    {
        List<GameTank> tanks = new List<GameTank>();
        for (int i = 0; i < numberOfEnemiesToGenerate; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(8,(gridSize.x - 1) * nodeSize ), Random.Range(8,(gridSize.y - 1) * nodeSize ), 0), new Quaternion());

            enemies.Add(enemy);

            GameTank tank = new GameTank();
            tank.team = 1;
            tank.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
            tanks.Add(tank);

        }
        return tanks;
    }
}
