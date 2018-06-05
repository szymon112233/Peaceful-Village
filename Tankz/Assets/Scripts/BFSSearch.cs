using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BFSSearch
{
    public static List<Vector3> Search(Tank tank, MapNode destNode)
    {
        HashSet<MapNode> visited = new HashSet<MapNode>();
        Queue<MapNode> tiles_queue = new Queue<MapNode>();

        Hashtable parents = new Hashtable();

        visited.Add(tank.Node);
        tiles_queue.Enqueue(tank.Node);

        while (tiles_queue.Count > 0)
        {
            MapNode current = tiles_queue.Dequeue();

            if (current.Equals(destNode))
                return GetParentsPath(parents, current);

            List<MapNode> neighbors = new List<MapNode>(FindNeighbors(tank, current));

            foreach (var neighbor in neighbors)
            {
                if (visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                parents[neighbor] = current;
                tiles_queue.Enqueue(neighbor);
            }
        }
        
        return new List<Vector3>();
    }

    static List<MapNode> FindNeighbors(Tank tank, MapNode mapNode)
    {
        MapNode[,] mapNodes = GameManager.instance.gamestate.mapNodes;
        List<MapNode> neighbors = new List<MapNode>();
        MapNode neighbor;
        List<Vector2Int> coordinates = new List<Vector2Int>
        {
            new Vector2Int(mapNode.x - 1, mapNode.y),
            new Vector2Int(mapNode.x + 1, mapNode.y),
            new Vector2Int(mapNode.x, mapNode.y - 1),
            new Vector2Int(mapNode.x, mapNode.y + 1)
        };

        foreach (var coordinate in coordinates)
        {
            if (coordinate.x > 0 && coordinate.x < GameManager.instance.gamestate.mapSize.x &&
                coordinate.y > 0 && coordinate.y < GameManager.instance.gamestate.mapSize.y)
            {
                neighbor = mapNodes[coordinate.x, coordinate.y];
                if (neighbor.CanMove(tank))
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    static List<Vector3> GetParentsPath(Hashtable parents, MapNode from)
    {
        List<MapNode> nodePath = new List<MapNode>();
        List<Vector3> path = new List<Vector3>();

        MapNode current = from;
        while (parents.ContainsKey(current))
        {
            nodePath.Add(current);
            current = (MapNode)parents[current];
        }
        nodePath.Reverse();

        foreach (MapNode node in nodePath)
            path.Add(new Vector3(node.transform.position.x + 4f, node.transform.position.y + 4f, 0f));
        
        return path;
    }
}
