using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public bool walkable;
    public bool tower;
    public Node[] neighbours;
    public Vector3Int gridPosition;

    public Node()
    {
        worldPosition = Vector3.zero;
        walkable = true;
    }

    public Node(Vector3 pos, bool walk, Vector3Int gridPos)
    {
        worldPosition = pos;
        walkable = walk;
        gridPosition = gridPos;
    }
}
