using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode shoot;

    public float fireRate = 0.3f;

    public NodeGrid grid;
    public Node node;

    public GameObject bulletPrefab = null;

    Rigidbody2D rigidBody = null;
    Tank tank = null;
    bool canShoot = true;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        tank = GetComponent<Tank>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(shoot) && canShoot)
            StartCoroutine(Shoot());
    }

    private void FixedUpdate()
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
        
        float speed = tank.speed * Time.fixedDeltaTime;

        if (moveVector.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            moveVector = new Vector3(1, 0, 0);
        }
        else if (moveVector.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            moveVector = new Vector3(-1, 0, 0);
        }
        else if (moveVector.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            moveVector = new Vector3(0, 1, 0);
        }
        else if (moveVector.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            moveVector = new Vector3(0, -1, 0);
        }
            
        rigidBody.MovePosition(rigidBody.position + moveVector * speed);

    }

    IEnumerator Shoot()
    {
        canShoot = false;
        Instantiate(bulletPrefab, tank.bulletPoint.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
