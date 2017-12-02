using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode shoot;

    public NodeGrid grid;
    public Node node;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if (Input.GetKeyDown(moveLeft))
        {
            Move(Direction.Left);
        }
        else if (Input.GetKeyDown(moveRight))
        {
            Move(Direction.Right);
        }
        else if (Input.GetKeyDown(moveUp))
        {
            Move(Direction.Up);
        }
        else if (Input.GetKeyDown(moveDown))
        {
            Move(Direction.Down);
        }
	}

    void Move(Direction d)
    {
        Vector2Int move = Vector2Int.zero;
        if (d == Direction.Down)
            move.y = -1;
        else if (d == Direction.Left)
            move.x = -1;
        else if (d == Direction.Up)
            move.y = 1;
        else if (d == Direction.Right)
            move.x = 1;
        Vector3 t = node.transform.position;
        Debug.Log(((int)t.x / 32 + move.x) + "," + ((int)t.y / 32 + move.y));
        Node destinationNode = grid.nodes[(int)t.x  / 32 + move.x, (int)t.y / 32 + move.y];
        if (destinationNode.isFree())
        {
            node.objectOnNode = null;
            node = destinationNode;
            node.objectOnNode = gameObject;
            gameObject.transform.SetParent(node.transform);
            transform.localPosition = new Vector3();
        }
    }
}
