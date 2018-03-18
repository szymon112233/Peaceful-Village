using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 32f;

    private Rigidbody2D rigidbody;

    public GameObject owner;
    public GameObject boomEffect;

	// Use this for initialization
	void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        Destroy(gameObject);
        Instantiate(boomEffect, transform.position, Quaternion.identity);
        Collider2D otherCollider = collision.collider;
        if (!otherCollider.CompareTag("Unbreakable"))
        {
            if (otherCollider.CompareTag("Wall"))
                GameManager.instance.gamestate.RemoveWall(new Vector2Int((int)otherCollider.transform.position.x, (int)otherCollider.transform.position.y));
            if (otherCollider.gameObject != owner)
                Destroy(otherCollider.gameObject);
        }
            
    }
}