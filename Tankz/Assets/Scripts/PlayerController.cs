using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("Which player is it? e.g. 1 == Player1")]
    public int player;

    public TankController controller;

    public float fireRate = 0.3f;

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
        if (controller.GetShoot() && canShoot)
            StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = controller.GetMoveVector();
        if (moveVector.sqrMagnitude != 0.0f) 
            Move(moveVector);
    }

    void Move(Vector2 moveVector)
    {
        
        float speed = tank.speed * Time.fixedDeltaTime;
        if (moveVector.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            RoundYPos();
            moveVector = new Vector3(1, 0, 0);
        }
        else if (moveVector.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            RoundYPos();
            moveVector = new Vector3(-1, 0, 0);
        }
        else if (moveVector.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            RoundXPos();
            moveVector = new Vector3(0, 1, 0);
        }
        else if (moveVector.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            RoundXPos();
            moveVector = new Vector3(0, -1, 0);
        }
        rigidBody.MovePosition(rigidBody.position + moveVector * speed);

    }

    private void RoundXPos()
    {
        Vector2 pos = rigidBody.position;
        int currGridPos = Mathf.FloorToInt(pos.x / 8) * 8 -4;
        int nextGridPos = currGridPos + 8;
        
        if (pos.x != currGridPos)
        {
            if (pos.x >= nextGridPos - 4)
            {
                rigidBody.position = new Vector2(nextGridPos, transform.position.y);
            }
            else if (pos.x <= currGridPos + 4)
            {
                rigidBody.position = new Vector2(currGridPos, transform.position.y);
            }
        }
    }

    private void RoundYPos()
    {
        Vector2 pos = rigidBody.position;
        int currGridPos = Mathf.FloorToInt(pos.y / 8) * 8 - 4;
        int nextGridPos = currGridPos + 8;
        
        if (pos.y!= currGridPos)
        {
            if (pos.y >= nextGridPos - 4)
            {
                rigidBody.position = new Vector2(pos.x, nextGridPos);
            }
            else if (pos.y <= currGridPos + 4)
            {
                rigidBody.position = new Vector2(pos.x, currGridPos);
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        Instantiate(bulletPrefab, tank.bulletPoint.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
