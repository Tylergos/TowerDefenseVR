using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;
    public Transform seeker, target;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.WorldToNode(startPos);
        Node targetNode = grid.WorldToNode(targetPos);
        Node current;

        List<Node> openNodes = new List<Node>();
        HashSet<Node> closedNodes = new HashSet<Node>();

        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            current = openNodes[0];

            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].fCost < current.fCost || (openNodes[i].fCost == current.fCost && openNodes[i].hCost < current.hCost))
                {
                    current = openNodes[i];
                }
            }

            openNodes.Remove(current);
            closedNodes.Add(current);

            if (current == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in current.neighbours)
            {
                if (!neighbour.walkable || closedNodes.Contains(neighbour))
                {
                    continue;
                }

                int moveCostNeighbour = current.gCost + GetDistance(current, neighbour);
                if (moveCostNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                {
                    neighbour.gCost = moveCostNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openNodes.Contains(neighbour))
                        openNodes.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node start, Node target)
    {
        List<Node> path = new List<Node>();
        Node currentNode = target;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node node1, Node node2)
    {
        //only calculates horizontal distance to point
        int dstX = Mathf.Abs(node1.gridPosition.x - node2.gridPosition.x);
        int dstZ = Mathf.Abs(node1.gridPosition.z - node2.gridPosition.z);

        return 14 * Mathf.Min(dstX, dstZ) + 10 * Mathf.Abs(dstX - dstZ);
    }
}
