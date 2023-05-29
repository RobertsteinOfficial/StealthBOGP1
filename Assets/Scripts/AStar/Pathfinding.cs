using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private MapGrid grid;

    public Transform agent, target;

    private void Awake()
    {
        grid = GetComponent<MapGrid>();
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                //Setto currentnode il nodo con il costo minore
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }


            //se non sono arrivato, devo loopare tra tutti i nodi vicini al currentnode

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);

                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }


    int GetDistance(Node nodeA, Node nodeB)
    {
        int deltaX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int deltaY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (deltaX > deltaY)
        {
            return 14 * deltaY + 10 * (deltaX - deltaY);
        }

        return 14 * deltaX + 10 * (deltaY - deltaX);
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    private void Update()
    {
        FindPath(agent.position, target.position);
    }
}
