using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public GameObject objectOnNode = null;
    public GameObject obstacle = null;

    public int x, y;

    private void Start()
    {
        x = (int)transform.position.x / 8;
        y = (int)transform.position.y / 8;
    }

    public bool IsFree()
    {
        return objectOnNode == null;
    }

    public bool CanMove(Tank myTank)
    {
        return obstacle == null;/* && !GameManager.instance.gamestate.tanksList.Any(
            tank => {
                if (tank == myTank)
                    return false;
                Vector3 vector = transform.position - tank.transform.position;
                if (Mathf.Abs(vector.x) <= 12 && Mathf.Abs(vector.y) <= 12)
                    return true;
                return false;
            }
        );*/
    }

    public override bool Equals(object other)
    {
        if (other == null || GetType() != other.GetType())
            return false;
        MapNode node = (MapNode)other;
        if (transform.position.x == node.transform.position.x &&
            transform.position.y == node.transform.position.y)
            return true;
        return false;
    }

    public override string ToString()
    {
        return "MapNode: (" + x + ", " + y + ")";
    }
}
