using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 32f;

    private Rigidbody2D rigidbody;

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
        if (!collision.collider.CompareTag("Unbreakable"))
            Destroy(collision.collider.gameObject);
    }
}