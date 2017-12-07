using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {

    public Vector2Int gridSize = new Vector2Int();
    public float nodeSize = 0.0f;
    public GameObject nodePrefab = null;
    public GameObject wallPrefab = null;
    public GameObject playerPrefab1 = null;
    public int numberOfWallsToGenerate = 10;
    public Node[,] nodes = null;

	
	void Start () {
        GenerateGrid();
        GenerateOuterWalls();
        GenerateRandomWalls(numberOfWallsToGenerate);
        GeneratePlayer(playerPrefab1);
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

    void GeneratePlayer(GameObject prefab)
    {
        GameObject player = Instantiate(prefab, new Vector3(gridSize.x * nodeSize /2, gridSize.y * nodeSize / 2, 0), new Quaternion());
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.grid = this;
        /*bool isGenerated = false;
        while (!isGenerated)
        {
            int randomX = Random.Range(0, gridSize.x);
            int randomY = Random.Range(0, gridSize.y);
            if (nodes[randomX, randomY].isFree())
            {
                GameObject player = Instantiate(prefab, nodes[randomX, randomY].transform);
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerController.grid = this;
                nodes[randomX, randomY].objectOnNode = player;
                playerController.node = nodes[randomX, randomY];
                isGenerated = true;
            }
        }*/
    }
}
