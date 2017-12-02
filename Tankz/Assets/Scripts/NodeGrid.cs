using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {

    public Vector2Int gridSize = new Vector2Int();
    public float nodeSize = 0.0f;
    public GameObject nodePrefab = null;
    public Node[,] nodes = null;

	
	void Start () {
        GenerateGrid();
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
        Debug.Log("Dupa");
    }
}
