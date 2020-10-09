using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    LayerMask unwalkable;
    LayerMask placedTower;
    LayerMask teleporterMask;
    public Vector3 gridWorldSize;
    [Range(0.1f, 10.0f)]
    public float nodeSizexz;
    [Range(0.1f, 10.0f)]
    public float nodeSizey;
    private Node[,,] grid;

    private int gridSizeX, gridSizeY, gridSizeZ;
    private float nodeHalfxz, nodeHalfy;
    private Vector3 bottomLeftFront, bottomLeftBack, bottomRightFront, bottomRightBack, topLeftFront, topLeftBack, topRightFront, topRightBack;

    private Node playerNode;
    private Node spawnNode;
    private Node endNode;

    private Dictionary<int, Node> towerIDToNode;

    private Dictionary<Node, Teleporter> nodeToTeleporter;
    List<GameObject> teleporters = new List<GameObject>();

    public List<Node> path;

    private void Awake()
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
        placedTower = LayerMask.GetMask("PlacedTower");
        teleporterMask = LayerMask.GetMask("Teleporter");

        CreateGrid();
        foreach (Node node in grid)
        {
            CreateNeighbours(node);
        }
        towerIDToNode = new Dictionary<int, Node>();
        nodeToTeleporter = new Dictionary<Node, Teleporter>();
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
                    grid[x, y, z] = new Node(GridToWorld(x, y, z), IsWalkablePosition(GridToWorld(x, y, z)), new Vector3Int(x, y, z));
                }
            }
        }
    }

    public void CreateNeighbours(Node _node)
    {
        for (int x = _node.gridPosition.x - 1; x <= _node.gridPosition.x + 1; x++)
        {
            for (int z = _node.gridPosition.z - 1; z <= _node.gridPosition.z + 1; z++)
            {
                if (x < 0 || x >= gridSizeX || z < 0 || z >= gridSizeZ)
                    continue;
                if (grid[x, _node.gridPosition.y, z] != _node)
                {
                    _node.neighbours.Add(grid[x, _node.gridPosition.y, z]);
                }
            }
        }
    }

    public Vector3 GridToWorld(int _x, int _y, int _z)
    {
        //calculates and returns grid position to world position
        return new Vector3(this.transform.position.x - gridWorldSize.x / 2 + nodeHalfxz + _x * nodeSizexz,
            this.transform.position.y - gridWorldSize.y / 2 + nodeHalfy + _y * nodeSizey,
            this.transform.position.z - gridWorldSize.z / 2 + nodeHalfxz + _z * nodeSizexz);
    }

    public Vector3 GridToWorld(Vector3 _gridCoord)
    {
        //calculates and returns grid position to world position
        return new Vector3(this.transform.position.x - gridWorldSize.x / 2 + nodeHalfxz + _gridCoord.x * nodeSizexz,
            this.transform.position.y - gridWorldSize.y / 2 + nodeHalfy + _gridCoord.y * nodeSizey,
            this.transform.position.z - gridWorldSize.z / 2 + nodeHalfxz + _gridCoord.z * nodeSizexz);
    }

    public Node WorldToNode(float _x, float _y, float _z)
    {
        //calcualates and returns the node containing the world position

        float percentX = Mathf.Clamp01((_x - bottomLeftBack.x) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((_y - bottomLeftBack.y) / gridWorldSize.y);
        float percentZ = Mathf.Clamp01((_z - bottomLeftBack.z) / gridWorldSize.z);

        return grid[Mathf.RoundToInt(percentX * (gridSizeX - 1)), Mathf.RoundToInt(percentY * (gridSizeY - 1)), Mathf.RoundToInt(percentZ * (gridSizeZ - 1))];
    }

    public Node WorldToNode(Vector3 _worldCoord)
    {
        //calcualates and returns the node containing the world position
        float percentX = Mathf.Clamp01((_worldCoord.x - bottomLeftBack.x) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((_worldCoord.y - bottomLeftBack.y) / gridWorldSize.y);
        float percentZ = Mathf.Clamp01((_worldCoord.z - bottomLeftBack.z) / gridWorldSize.z);

        return grid[Mathf.RoundToInt(percentX * (gridSizeX - 1)), Mathf.RoundToInt(percentY * (gridSizeY - 1)), Mathf.RoundToInt(percentZ * (gridSizeZ - 1))];
    }

    private bool IsWalkablePosition(Vector3 _pos)
    {
        //checks area for unwalkable layer to determine if node is walkable
        return !Physics.CheckBox(_pos,
            new Vector3(nodeHalfxz, nodeHalfy, nodeHalfxz), Quaternion.identity, unwalkable);
    }

    public void SetPlayerNode(Vector3 _pos)
    {
        //sets player node on grid
        playerNode = WorldToNode(_pos);
    }

    public void SetEndNode(Vector3 _pos)
    {
        //sets end node on grid
        endNode = WorldToNode(_pos);
    }

    public void SetSpawnerNode(Vector3 _pos)
    {
        spawnNode = WorldToNode(_pos);
    }

    public void AddTeleporterNodes(GameObject _teleporter, int _radius = 2)
    {
        teleporters.Add(_teleporter);
        Vector3Int baseGrid = WorldToNode(_teleporter.transform.position).gridPosition;
        for (int x = 1 - _radius; x < _radius; x++)
        {
            for (int z = 1 - _radius; z < _radius; z++)
            {
                try
                {
                    if (Physics.CheckBox(grid[baseGrid.x + x, baseGrid.y, baseGrid.z + z].worldPosition,
                        new Vector3(nodeHalfxz - .1f, nodeHalfy - .1f, nodeHalfxz - .1f), Quaternion.identity, teleporterMask, QueryTriggerInteraction.Collide))
                    {
                        grid[baseGrid.x + x, baseGrid.y, baseGrid.z + z].teleporter = true;
                        try
                        {
                            nodeToTeleporter.Add(grid[baseGrid.x + x, baseGrid.y, baseGrid.z + z], _teleporter.GetComponent<Teleporter>());
                        }
                        catch
                        {
                            //Debug.Log("Node already contains teleporter");
                        };
                    }
                }
                catch { };
            }
        }
    }

    public void AddTowerNode(GameObject _tower, int _radius = 2)
    {
        towerIDToNode.Add(_tower.GetInstanceID(), WorldToNode(_tower.transform.position));
        Vector3Int baseGrid = towerIDToNode[_tower.GetInstanceID()].gridPosition;

        for (int x = 1 - _radius; x < _radius; x++)
        {
            for (int z = 1 - _radius; z < _radius; z++)
            {
                try
                {
                    if (Physics.CheckBox(grid[baseGrid.x + x, baseGrid.y, baseGrid.z + z].worldPosition,
                        new Vector3(nodeHalfxz - .1f, nodeHalfy - .1f, nodeHalfxz - .1f), Quaternion.identity, placedTower))
                    {
                        grid[baseGrid.x + x, baseGrid.y, baseGrid.z + z].tower = true;
                    }
                }
                catch { };
            }
        }
    }

    public void RemoveTowerNode(GameObject _tower, int _radius = 2)
    {
        Vector3Int baseGrid = towerIDToNode[_tower.GetInstanceID()].gridPosition;

        for (int x = 1 - _radius; x < _radius; x++)
        {
            for (int z = 1 - _radius; z < _radius; z++)
            {
                try
                {
                    if (!Physics.CheckBox(grid[baseGrid.x + x, baseGrid.y, baseGrid.z + z].worldPosition,
                        new Vector3(nodeHalfxz - .1f, nodeHalfy - .1f, nodeHalfxz - .1f), Quaternion.identity, placedTower))
                    {
                        grid[baseGrid.x + x, baseGrid.y, baseGrid.z + z].tower = false;
                    }
                }
                catch { };
            }
        }

        //removes tower from dictionary
        towerIDToNode.Remove(_tower.GetInstanceID());
    }

    public int MaxHeapSize
    {
        get { return gridSizeX* gridSizeY * gridSizeZ; }
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
                else if (node.teleporter)
                {
                    Gizmos.color = Color.yellow;
                }
                else if (node.tower)
                {
                    Gizmos.color = Color.black;
                }
                else if (node == spawnNode)
                {
                    Gizmos.color = Color.blue;
                }
                else if (node == endNode)
                {
                    Gizmos.color = Color.magenta;
                }
                else if (node.walkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }
                Gizmos.DrawCube(node.worldPosition, new Vector3(nodeSizexz - .1f, nodeSizey - .1f, nodeSizexz - .1f));
            }
            //Gizmos.DrawCube(grid[0, 0, 0].worldPosition, new Vector3(nodeSizexz - .1f, nodeSizey - .1f, nodeSizexz - .1f));
        }

    }
}
