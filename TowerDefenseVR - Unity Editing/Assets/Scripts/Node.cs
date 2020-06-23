using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public bool walkable;
    public Node[] neighbours;

    public Node()
    {
        worldPosition = Vector3.zero;
        walkable = true;
    }

    public Node(Vector3 pos, bool walk)
    {
        worldPosition = pos;
        walkable = walk;
    }

    public void addNeighbour(Node neighbour)
    {
        neighbours[neighbours.Length] = neighbour;
    }
}
