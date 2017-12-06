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

    Rigidbody2D rigidBody = null;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(moveVector);
	}

    void Move(Direction d)
    {
        Vector2Int move = Vector2Int.zero;
        Quaternion rotate = new Quaternion();
        if (d == Direction.Down)
        {
            move.y = -1;
            rotate = Quaternion.AngleAxis(180, Vector3.forward);
        }
        else if (d == Direction.Left)
        {
            move.x = -1;
            rotate = Quaternion.AngleAxis(90, Vector3.forward);
        }
        else if (d == Direction.Up)
        {
            move.y = 1;
            rotate = Quaternion.AngleAxis(0, Vector3.forward);
        }
        else if (d == Direction.Right)
        {
            move.x = 1;
            rotate = Quaternion.AngleAxis(-90, Vector3.forward);
        }
        Vector3 t = node.transform.position;
        Debug.Log(((int)t.x / grid.nodeSize + move.x) + "," + ((int)t.y / grid.nodeSize + move.y));
        Node destinationNode = grid.nodes[(int)t.x  / (int)grid.nodeSize + move.x, (int)t.y / (int)grid.nodeSize + move.y];
        transform.rotation = rotate;
        if (destinationNode.isFree())
        {
            node.objectOnNode = null;
            node = destinationNode;
            node.objectOnNode = gameObject;
            gameObject.transform.SetParent(node.transform);
            transform.localPosition = new Vector3();
        }
    }

    void Move(Vector2 moveVector)
    {
        
        float speed = grid.nodeSize * Time.deltaTime;
        float angle = 0;

        rigidBody.MovePosition(rigidBody.position + moveVector * speed);
        rigidBody.MoveRotation(angle);

    }
}
