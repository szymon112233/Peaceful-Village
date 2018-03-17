using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    MapNode _mapNode;

    public Transform bulletPoint = null;
    public int team;

    [Header("Speed in pixels per second")]
    public float speed = 16;

    private void OnDestroy()
    {
        GameManager.instance.gamestate.RemoveTank(this);
    }
}
