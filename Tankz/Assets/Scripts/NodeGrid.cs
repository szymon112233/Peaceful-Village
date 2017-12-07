using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {

    public Vector2Int gridSize = new Vector2Int();
    public float nodeSize = 0.0f;
    public GameObject nodePrefab = null;
    public GameObject wallPrefab = null;
    public GameObject[] playerPrefabs = null;
    public int numberOfWallsToGenerate = 10;
    public Node[,] nodes = null;

	
	void Start () {
        GenerateGrid();
        GenerateOuterWalls();
        GenerateRandomWalls(numberOfWallsToGenerate);
        GeneratePlayers();
    }
	
	void GenerateGrid()
    {
        nodes = new Node[gridSize.x, gridSize.y];
        for (int i = (gridSize.y - 1) ; i>=0 ; i--)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                GameObject go = Instantiate(nodePrefab, new Vector3(j * nodeSize, i * nodeSize, 1.0f), new Quaternion(), gameObject.transform);
                nodes[j, i] = go.GetComponent<Node>();
            }
        }
    }

    void GenerateOuterWalls()
    {
        int i = 0;
        for (int j = 0; j< gridSize.x; j++)
        {
            (nodes[j, i].objectOnNode = Instantiate(wallPrefab, nodes[j, i].transform)).tag = "Unbreakable";
        }

        i = gridSize.y -1;
        for (int j = 0; j < gridSize.x; j++)
        {
            (nodes[j, i].objectOnNode = Instantiate(wallPrefab, nodes[j, i].transform)).tag = "Unbreakable";
        }

        i = 0;
        for (int j = 0; j < gridSize.y; j++)
        {
            if (nodes[i, j].isFree())
                (nodes[i, j].objectOnNode = Instantiate(wallPrefab, nodes[i, j].transform)).tag = "Unbreakable";
        }

        i = gridSize.x - 1;
        for (int j = 0; j < gridSize.y; j++)
        {
            if (nodes[i, j].isFree())
                (nodes[i, j].objectOnNode = Instantiate(wallPrefab, nodes[i, j].transform)).tag = "Unbreakable";
        }
    }

    void GenerateRandomWalls(int count)
    {
        while (count > 0)
        {
            int randomX = Random.Range(0, gridSize.x);
            int randomY = Random.Range(0, gridSize.y);
            if (nodes[randomX, randomY].isFree())
            {
                nodes[randomX, randomY].objectOnNode = Instantiate(wallPrefab, nodes[randomX, randomY].transform);
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
            /*int randomX = Random.Range(0, gridSize.x);
            int randomY = Random.Range(0, gridSize.y);
            if (nodes[randomX, randomY].isFree())
            {*/
            int[] x = new int[] { 2, gridSize.x - 2 };
            int[] y = new int[] { 2, gridSize.y - 2 };
            GameObject player = Instantiate(playerPrefabs[counter], new Vector3(x[counter] * nodeSize, y[counter++] * nodeSize, 0), new Quaternion());
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.grid = this;
            //}
        }
    }
}
