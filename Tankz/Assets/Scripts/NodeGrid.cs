using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	
	void Start () {
        GenerateGrid();
        GenerateOuterWalls();
        GenerateRandomWalls(numberOfWallsToGenerate);
        GeneratePlayers();
	    GenerateEnemies();
	}
	
	void GenerateGrid()
    {
        MapNodes = new MapNode[gridSize.x, gridSize.y];
        for (int i = (gridSize.y - 1) ; i>=0 ; i--)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                GameObject go = Instantiate(nodePrefab, new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                MapNodes[j, i] = go.GetComponent<MapNode>();
            }
        }
    }

    void GenerateOuterWalls()
    {
        int i = 0;
        for (int j = 0; j< gridSize.x; j++)
        {
            (MapNodes[j, i].objectOnNode = Instantiate(wallPrefab, MapNodes[j, i].transform)).tag = "Unbreakable";
        }

        i = gridSize.y -1;
        for (int j = 0; j < gridSize.x; j++)
        {
            (MapNodes[j, i].objectOnNode = Instantiate(wallPrefab, MapNodes[j, i].transform)).tag = "Unbreakable";
        }

        i = 0;
        for (int j = 0; j < gridSize.y; j++)
        {
            if (MapNodes[i, j].isFree())
                (MapNodes[i, j].objectOnNode = Instantiate(wallPrefab, MapNodes[i, j].transform)).tag = "Unbreakable";
        }

        i = gridSize.x - 1;
        for (int j = 0; j < gridSize.y; j++)
        {
            if (MapNodes[i, j].isFree())
                (MapNodes[i, j].objectOnNode = Instantiate(wallPrefab, MapNodes[i, j].transform)).tag = "Unbreakable";
        }
    }

    void GenerateRandomWalls(int count)
    {
        while (count > 0)
        {
            int randomX = Random.Range(0, gridSize.x);
            int randomY = Random.Range(0, gridSize.y);
            if (MapNodes[randomX, randomY].isFree())
            {
                MapNodes[randomX, randomY].objectOnNode = Instantiate(wallPrefab, MapNodes[randomX, randomY].transform);
                count--;
            }
                
        }
    }

    void GeneratePlayers()
    {
        bool isGenerated = false;
        int counter = 0;
        while (counter <= 1)
        {
            int[] x =  { 2, gridSize.x - 2 };
            int[] y =  { 2, gridSize.y - 2 };
            GameObject player = Instantiate(playerPrefabs[counter], new Vector3(x[counter] * nodeSize, y[counter++] * nodeSize, 0), new Quaternion());
        }
    }
    
    void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToGenerate; i++)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(8,(gridSize.x - 1) * nodeSize ), Random.Range(8,(gridSize.y - 1) * nodeSize ), 0), new Quaternion());
        }
    }
}
