using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Vector3 worldPosition;
    public bool walkable;
    public bool tower;
    public bool teleporter;
    public List<Node> neighbours;
    public Vector3Int gridPosition;
    public Node parent;

    public int gCost;
    public int hCost;
    int heapIndex;

    public Node()
    {
        worldPosition = Vector3.zero;
        walkable = true;
        neighbours = new List<Node>();
    }

    public Node(Vector3 _pos, bool _walk, Vector3Int _gridPos)
    {
        worldPosition = _pos;
        walkable = _walk;
        gridPosition = _gridPos;
        neighbours = new List<Node>();
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node _other)
    {
        int compare = fCost.CompareTo(_other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(_other.hCost);
        }
        return -compare;
    }
}
