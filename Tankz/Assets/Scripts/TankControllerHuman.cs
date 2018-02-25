using System;
using UnityEngine;


public class TankControllerHuman : TankController
{
    public int localPlayerNumber = 1;
    
    
    public override Vector2 GetMoveVector()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal" + localPlayerNumber), Input.GetAxisRaw("Vertical" + localPlayerNumber));
    }

    public override bool GetShoot()
    {
        return Input.GetButtonDown(String.Format("Fire{0}", localPlayerNumber));
    }
}
