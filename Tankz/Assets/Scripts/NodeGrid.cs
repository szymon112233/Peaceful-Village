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
    public int numberOfBushesToGenerate = 10;
    public int numberOfWatersToGenerate = 10;
    public int numberOfEnemiesToGenerate = 1;
    public MapNode[,] MapNodes = null;
    public GameObject[] environmentPrefabs = null;

    int wallSpriteIndex = 0;
    int unbreakableWallSpriteIndex = 0;
    int bushSpriteIndex = 0;
    int waterSpriteIndex = 0;
    int playerSpriteIndex = 0;
    int enemySpriteStartIndex = 0;

    List<Vector2Int> hardWalls;
    List<Vector2Int> walls;
    List<Vector2Int> bushes;
    List<Vector2Int> waters;
    List<Tank> tanks;

    private void Awake()
    {
        hardWalls = new List<Vector2Int>();
        walls = new List<Vector2Int>();
        bushes = new List<Vector2Int>();
        waters = new List<Vector2Int>();
        tanks = new List<Tank>();

        wallSpriteIndex = GetSpriteIndex("Wall");
        if (wallSpriteIndex == -1)
            Debug.LogError("Could not find index of wallSpriteIndex!!");
        unbreakableWallSpriteIndex = GetSpriteIndex("UnbreakableWall");
        if (unbreakableWallSpriteIndex == -1)
            Debug.LogError("Could not find index of unbreakableWallSpriteIndex!!");
        bushSpriteIndex = GetSpriteIndex("Bush");
        if (bushSpriteIndex == -1)
            Debug.LogError("Could not find index of bushSpriteIndex!!");
        waterSpriteIndex = GetSpriteIndex("Water");
        if (waterSpriteIndex == -1)
            Debug.LogError("Could not find index of waterSpriteIndex!!");
        playerSpriteIndex = GetSpriteIndex("Player1");
        if (playerSpriteIndex == -1)
            Debug.LogError("Could not find index of playerSpriteIndex!!");
        enemySpriteStartIndex = GetSpriteIndex("Enemy");
        if (enemySpriteStartIndex == -1)
            Debug.LogError("Could not find index of enemySpriteStartIndex!!");
    }

    private int GetSpriteIndex(string name)
    {
        for (int i = 0; i < environmentPrefabs.Length; i++)
        {
            if (environmentPrefabs[i].name == name)
                return i;
        }

        return -1;
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
                            GameObject player = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize +4, i * nodeSize - 4, 1.0f), new Quaternion(), gameObject.transform);
                            player.GetComponent<TankControllerHuman>().localPlayerNumber = playerIndex++;
                            tanks.Add(player.GetComponent<Tank>());
                        }
                        else if (index >= enemySpriteStartIndex)
                        {
                            GameObject enemy = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize + 4, i * nodeSize - 4, 1.0f), new Quaternion(), gameObject.transform);
                            enemy.GetComponent<Tank>().team = index - enemySpriteStartIndex;
                            tanks.Add(enemy.GetComponent<Tank>());
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
                        else if (index == bushSpriteIndex)
                        {
                            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                            bushes.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
                        }
                        else if (index == waterSpriteIndex)
                        {
                            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                            waters.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
                        }
                        else
                            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[index], new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                    }
                }
            }
            GameManager.instance.gamestate = new GameState(gridSize, hardWalls, walls, bushes, waters, tanks);

            Debug.Log(GameManager.instance.gamestate);
            GameManager.instance.CenterCamera();
        }
        else
            Debug.LogError("Can't read from file!");
    }

    void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        foreach (Tank tank in tanks)
            if (tank != null)
                Destroy(tank.gameObject);
        hardWalls = new List<Vector2Int>();
        walls = new List<Vector2Int>();
        bushes = new List<Vector2Int>();
        waters = new List<Vector2Int>();
        
        GameManager.instance.gamestate = new GameState();
    }

    void GenerateRandom()
    {
        Clear();
        GenerateGrid();
        hardWalls.AddRange(GenerateOuterWalls());
        walls.AddRange(GenerateRandomWalls(numberOfWallsToGenerate));
        bushes.AddRange(GenerateRandomBushes(numberOfBushesToGenerate));
        waters.AddRange(GenerateRandomWater(numberOfWatersToGenerate));
        GeneratePlayers();
        GenerateEnemies();

        GameManager.instance.gamestate = new GameState(gridSize, hardWalls, walls, bushes, waters, tanks);
	    
	    Debug.Log(GameManager.instance.gamestate);
        
        GameManager.instance.CenterCamera();
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
            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[unbreakableWallSpriteIndex], MapNodes[j, i].transform);
            walls.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
        }

        i = gridSize.y -1;
        for (int j = 0; j < gridSize.x; j++)
        {
            MapNodes[j, i].objectOnNode = Instantiate(environmentPrefabs[unbreakableWallSpriteIndex], MapNodes[j, i].transform);
            walls.Add(new Vector2Int((int)MapNodes[j, i].transform.position.x, (int)MapNodes[j, i].transform.position.y));
        }

        i = 0;
        for (int j = 0; j < gridSize.y; j++)
        {
            MapNodes[i, j].objectOnNode = Instantiate(environmentPrefabs[unbreakableWallSpriteIndex], MapNodes[i, j].transform);
            walls.Add(new Vector2Int((int)MapNodes[i, j].transform.position.x, (int)MapNodes[i, j].transform.position.y));
        }

        i = gridSize.x - 1;
        for (int j = 0; j < gridSize.y; j++)
        {
            MapNodes[i, j].objectOnNode = Instantiate(environmentPrefabs[unbreakableWallSpriteIndex], MapNodes[i, j].transform);
            walls.Add(new Vector2Int((int)MapNodes[i, j].transform.position.x, (int)MapNodes[i, j].transform.position.y));
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
    
    List<Vector2Int> GenerateRandomBushes(int count)
    {
        List<Vector2Int> bushes = new List<Vector2Int>();
        while (count > 0)
        {
            int randomX = Random.Range(0, gridSize.x);
            int randomY = Random.Range(0, gridSize.y);
            if (MapNodes[randomX, randomY].isFree())
            {
                MapNodes[randomX, randomY].objectOnNode = Instantiate(environmentPrefabs[bushSpriteIndex], MapNodes[randomX, randomY].transform);
                count--;
                bushes.Add(new Vector2Int((int)MapNodes[randomX, randomY].transform.position.x, (int)MapNodes[randomX, randomY].transform.position.y));
            }     
        }
        return bushes;
    }
    
    List<Vector2Int> GenerateRandomWater(int count)
    {
        List<Vector2Int> waters = new List<Vector2Int>();
        while (count > 0)
        {
            int randomX = Random.Range(0, gridSize.x);
            int randomY = Random.Range(0, gridSize.y);
            if (MapNodes[randomX, randomY].isFree())
            {
                MapNodes[randomX, randomY].objectOnNode = Instantiate(environmentPrefabs[waterSpriteIndex], MapNodes[randomX, randomY].transform);
                count--;
                waters.Add(new Vector2Int((int)MapNodes[randomX, randomY].transform.position.x, (int)MapNodes[randomX, randomY].transform.position.y));
            }     
        }
        return waters;
    }

    void GeneratePlayers()
    {
        int counter = 0;
        while (counter <= 1)
        {
            int[] x =  { 2, gridSize.x - 2 };
            int[] y =  { 2, gridSize.y - 2 };
            GameObject player = Instantiate(playerPrefabs[counter], new Vector3(x[counter] * nodeSize, y[counter++] * nodeSize, 0), new Quaternion());
            tanks.Add(player.GetComponent<Tank>());
        }
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToGenerate; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(8,(gridSize.x - 1) * nodeSize ), Random.Range(8,(gridSize.y - 1) * nodeSize ), 0), new Quaternion());
            tanks.Add(enemy.GetComponent<Tank>());
        }
    }
}
