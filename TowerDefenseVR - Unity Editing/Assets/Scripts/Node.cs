﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
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

    public Node()
    {
        worldPosition = Vector3.zero;
        walkable = true;
        neighbours = new List<Node>();
    }

    public Node(Vector3 pos, bool walk, Vector3Int gridPos)
    {
        worldPosition = pos;
        walkable = walk;
        gridPosition = gridPos;
        neighbours = new List<Node>();
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
