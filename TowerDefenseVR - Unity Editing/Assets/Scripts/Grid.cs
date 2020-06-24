﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    LayerMask unwalkable;
    public Vector3 gridWorldSize;
    [Range(0.0f, 10.0f)]
    public float nodeSizexz;
    [Range(0.0f, 10.0f)]
    public float nodeSizey;
    private Node[,,] grid;

    private int gridSizeX, gridSizeY, gridSizeZ;
    private float nodeHalfxz, nodeHalfy;
    private Vector3 bottomLeftFront, bottomLeftBack, bottomRightFront, bottomRightBack, topLeftFront, topLeftBack, topRightFront, topRightBack;

    private Node playerNode;
    

    private void Start()
    {
        //+z is forward, +x is right, +y is top
        //Calculate the world position of each vertex of the grid
        bottomLeftFront = new Vector3(transform.position.x - (gridWorldSize.x / 2), transform.position.y - (gridWorldSize.y / 2), transform.position.z + (gridWorldSize.z / 2));
        bottomLeftBack = new Vector3(transform.position.x - (gridWorldSize.x / 2), transform.position.y - (gridWorldSize.y / 2), transform.position.z - (gridWorldSize.z / 2));
        bottomRightFront = new Vector3(transform.position.x + (gridWorldSize.x / 2), transform.position.y - (gridWorldSize.y / 2), transform.position.z + (gridWorldSize.z / 2));
        bottomRightBack = new Vector3(transform.position.x + (gridWorldSize.x / 2), transform.position.y - (gridWorldSize.y / 2), transform.position.z - (gridWorldSize.z / 2));
        topLeftFront = new Vector3(transform.position.x - (gridWorldSize.x / 2), transform.position.y + (gridWorldSize.y / 2), transform.position.z + (gridWorldSize.z / 2));
        topLeftBack = new Vector3(transform.position.x - (gridWorldSize.x / 2), transform.position.y + (gridWorldSize.y / 2), transform.position.z - (gridWorldSize.z / 2));
        topRightFront = new Vector3(transform.position.x + (gridWorldSize.x / 2), transform.position.y + (gridWorldSize.y / 2), transform.position.z + (gridWorldSize.z / 2));
        topRightBack = new Vector3(transform.position.x + (gridWorldSize.x / 2), transform.position.y + (gridWorldSize.y / 2), transform.position.z - (gridWorldSize.z / 2));
        
        //calculate grid size from world size
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeSizexz);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeSizey);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeSizexz);

        //calculate half node sizes
        nodeHalfxz = nodeSizexz / 2;
        nodeHalfy = nodeSizey / 2;
        
        //create unwalkable layermask
        unwalkable = LayerMask.GetMask("Unwalkable");

        CreateGrid();
    }

    private void CreateGrid()
    {
        //generates the grid of nodes
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    grid[x, y, z] = new Node(GridToWorld(x,y,z), IsWalkablePosition(GridToWorld(x,y,z)));
                }
            }
        }
    }

    public Vector3 GridToWorld(int x, int y, int z)
    {
        //calculates and returns grid position to world position
        return new Vector3(this.transform.position.x - gridWorldSize.x / 2 + nodeHalfxz + x * nodeSizexz,
            this.transform.position.y - gridWorldSize.y / 2 + nodeHalfy + y * nodeSizey,
            this.transform.position.z - gridWorldSize.z / 2 + nodeHalfxz + z * nodeSizexz);
    }

    public Vector3 GridToWorld(Vector3 gridCoord)
    {
        //calculates and returns grid position to world position
        return new Vector3(this.transform.position.x - gridWorldSize.x / 2 + nodeHalfxz + gridCoord.x * nodeSizexz,
            this.transform.position.y - gridWorldSize.y / 2 + nodeHalfy + gridCoord.y * nodeSizey,
            this.transform.position.z - gridWorldSize.z / 2 + nodeHalfxz + gridCoord.z * nodeSizexz);
    }

    public Node WorldToNode(float x, float y, float z)
    {
        //calcualates and returns the node containing the world position

        float percentX = Mathf.Clamp01((x - bottomLeftBack.x) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((y - bottomLeftBack.y) / gridWorldSize.y);
        float percentZ = Mathf.Clamp01((z - bottomLeftBack.z) / gridWorldSize.z);

        return grid[Mathf.RoundToInt(percentX * (gridSizeX - 1)), Mathf.RoundToInt(percentY * (gridSizeY - 1)), Mathf.RoundToInt(percentZ * (gridSizeZ - 1))];
    }

    public Node WorldToNode(Vector3 worldCoord)
    {
        //calcualates and returns the node containing the world position
        float percentX = Mathf.Clamp01((worldCoord.x - bottomLeftBack.x) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldCoord.y - bottomLeftBack.y) / gridWorldSize.y);
        float percentZ = Mathf.Clamp01((worldCoord.z - bottomLeftBack.z) / gridWorldSize.z);

        return grid[Mathf.RoundToInt(percentX * (gridSizeX - 1)), Mathf.RoundToInt(percentY * (gridSizeY - 1)), Mathf.RoundToInt(percentZ * (gridSizeZ - 1))];
    }

    private bool IsWalkablePosition(Vector3 pos)
    {
        //checks area for unwalkable layer to determine if node is walkable
        return !Physics.CheckBox(pos, 
            new Vector3(nodeHalfxz, nodeHalfy, nodeHalfxz), Quaternion.identity, unwalkable);
    }

    public void SetPlayerNode(Vector3 pos)
    {
        playerNode = WorldToNode(pos);
    }

    private void OnDrawGizmos()
    {
        //draw main grid gizmo
        Gizmos.DrawWireCube(transform.position, gridWorldSize);
        
        //draw the individual nodes in the grid
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (node == playerNode)
                {
                    Gizmos.color = Color.green;
                }
                else if (node.walkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(node.worldPosition, new Vector3(nodeSizexz - .1f, nodeSizey - .1f, nodeSizexz - .1f));
            }
            //Gizmos.DrawCube(grid[0, 0, 0].worldPosition, new Vector3(nodeSizexz - .1f, nodeSizey - .1f, nodeSizexz - .1f));
        }

    }
}