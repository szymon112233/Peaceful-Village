using UnityEngine;

public class TankControllerBTree : TankController
{
    
    private Vector2 moveVector;
    private bool shoot;

    private void Awake()
    {
        moveVector = new Vector2();
        shoot = false;
    }

    public void SetMoveVector(Vector2 vector)
    {
        moveVector = vector;
    }

    public void SetShoot(bool value)
    {
        shoot = value;
    }

    public override Vector2 GetMoveVector()
    {
        return moveVector;
    }

    public override bool GetShoot()
    {
        return shoot;
    }

}
