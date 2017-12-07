using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 32f;

    Rigidbody2D rigidbody;

	// Use this for initialization
	void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        rigidbody.velocity = transform.up * speed;
	}
}