using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSSearcher : MonoBehaviour
{
    public Vector2Int startCoord, destinationCoord;

    Tank myTank;

    public void Start()
    {
        myTank = GameManager.instance.gamestate.tanksList[0];
    }

    // Coord in MapNodes in Gamestate
    public List<MapNode> Search()
    {
        MapNode start = GameManager.instance.gamestate.mapNodes[startCoord.x, startCoord.y];
        MapNode destination = GameManager.instance.gamestate.mapNodes[destinationCoord.x, destinationCoord.y];
        HashSet<MapNode> visited = new HashSet<MapNode>();
        Queue<MapNode> tiles_queue = new Queue<MapNode>();

        Hashtable parents = new Hashtable();

        visited.Add(start);
        tiles_queue.Enqueue(start);

        while (tiles_queue.Count > 0)
        {
            MapNode current = tiles_queue.Dequeue();

            if (current.Equals(destination))
                return GetParentsPath(parents, current);

            List<MapNode> neighbors = new List<MapNode>(FindNeighbors(current));
            //neighbors.ForEach(x => Debug.Log("Neighbor: " + x));

            foreach (var neighbor in neighbors)
            {
                if (visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                parents[neighbor] = current;
                tiles_queue.Enqueue(neighbor);
            }
        }
        
        return new List<MapNode>();
    }

    List<MapNode> FindNeighbors(MapNode mapNode)
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
                if (neighbor.CanMove(myTank))
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    List<MapNode> GetParentsPath(Hashtable parents, MapNode from)
    {
        List<MapNode> path = new List<MapNode>();

        MapNode current = from;
        while (parents.ContainsKey(current))
        {
            path.Add(current);
            current = (MapNode)parents[current];
        }
        path.Reverse();
        return path;
    }
}
