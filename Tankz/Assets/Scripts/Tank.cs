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

    public MapNode Node
    {
        get
        {
            int x = (int)transform.position.x / 8;
            int y = (int)transform.position.y / 8;
            return GameManager.instance.gamestate.mapNodes[x, y];
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.gamestate.RemoveTank(this);
    }
}
