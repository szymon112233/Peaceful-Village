using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankController : MonoBehaviour
{
    public abstract Vector2 GetMoveVector();

    public abstract bool GetShoot();
}
