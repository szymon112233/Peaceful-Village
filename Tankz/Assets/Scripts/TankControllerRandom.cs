using UnityEngine;

public class TankControllerRandom : TankController
{
    public float decisonTime = 3.0f;
    private float timer;
    
    private Vector2 vector;

    private void Awake()
    {
        timer = decisonTime;
        vector = new Vector2();
    }

    public override Vector2 GetMoveVector()
    {
        if (timer < 0)
        {
            if (Random.value > 0.5f)
            {
                vector = new Vector2(Random.Range(-1.0f, 1.0f) , 0.0f);
            }
            else
            {
                vector = new Vector2(0.0f , Random.Range(-1.0f, 1.0f));
            }
            timer = decisonTime;
        }

        return vector;
    }

    public override bool GetShoot()
    {
        return Random.value > 0.1f;
    }

    private void Update()
    {
        timer -= Time.unscaledDeltaTime;
    }
}
