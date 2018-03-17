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

    public int wallSpriteIndex = 0;
    public int unbreakableWallSpriteIndex = 1;
    public int playerSpriteIndex = 5;
    public int enemySpriteStartIndex = 6;

    List<Vector2Int> hardWalls;
    List<Vector2Int> walls;
    List<GameObject> tanks;

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

    private void Awake()
    {
        hardWalls = new List<Vector2Int>();
        walls = new List<Vector2Int>();
        tanks = new List<GameObject>();
    }

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
            hardWalls.AddRange(GenerateOuterWalls());
            int playerIndex = 1;
            for (int i = gridSize.y - 1; i >= 0; i--)
            {
                string[] indices = lines[gridSize.y - 1 - i].Split(',');
                int index;
                for (int j = 0; j < gridSize.x; j++)
                {
                    if (MapNodes[j, i].objectOnNode == null && (index = int.Parse(indices[j])) != -1)
                    {
                        if (index == playerSpriteIndex)
                        {
                            GameObject player = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                            player.GetComponent<TankControllerHuman>().localPlayerNumber = playerIndex++;
                            tanks.Add(player);
                        }
                        else if (index >= enemySpriteStartIndex)
                        {
                            GameObject enemy = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                            enemy.GetComponent<Tank>().team = index - enemySpriteStartIndex;
                            tanks.Add(enemy);
                        }
                        else if (index == unbreakableWallSpriteIndex)
                        {
                            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                            hardWalls.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
                        }
                        else if (index == wallSpriteIndex)
                        {
                            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                            walls.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
                        }
                        else
                            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                    }
                }
            }
            GameManager.instance.gamestate = new GameState(gridSize, hardWalls, walls, tanks);

            Debug.Log(GameManager.instance.gamestate);
        }
        else
            Debug.LogError("Can't read from file!");
    }

    void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        foreach (GameObject tank in tanks)
            Destroy(tank);
        hardWalls = new List<Vector2Int>();
        walls = new List<Vector2Int>();
    }

    void GenerateRandom()
    {
        Clear();
        GenerateGrid();
	    GeneratePlayers();
	    GenerateEnemies();
        walls.AddRange(GenerateRandomWalls(numberOfWallsToGenerate));
        hardWalls.AddRange(GenerateOuterWalls());
	    
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

    void GeneratePlayers()
    {
        int counter = 0;
        while (counter <= 1)
        {
            int[] x =  { 2, gridSize.x - 2 };
            int[] y =  { 2, gridSize.y - 2 };
            GameObject player = Instantiate(playerPrefabs[counter], new Vector3(x[counter] * nodeSize, y[counter++] * nodeSize, 0), new Quaternion());
            tanks.Add(player);
        }
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToGenerate; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(8,(gridSize.x - 1) * nodeSize ), Random.Range(8,(gridSize.y - 1) * nodeSize ), 0), new Quaternion());
            tanks.Add(enemy);
        }
    }
}
