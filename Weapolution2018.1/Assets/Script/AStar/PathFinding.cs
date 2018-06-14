using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour {
    public bool drawGizmos;
    Grid grid;
    PathRequestManager pathRequestManager;
	// Use this for initialization
	void Awake () {
        grid = GetComponent<Grid>();
        pathRequestManager = GetComponent<PathRequestManager>();
	}


    // Update is called once per frame
    void Update () {

	}

    int GetDistance(Node nodeA, Node nodeB) {
        int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (disX > disY) return 14 * disY + 10 * (disX - disY);
        else return 14 * disX + 10 * (disY - disX);
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.GetNodeFromWorld(startPos);
        Node targetNode = grid.GetNodeFromWorld(targetPos);
        //Debug.Log(startNode.walkable);
        if (true) {  //startNode.walkable  targetNode.walkable
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                //for (int i = 1; i < openSet.Count; i++) {
                //    if (currentNode.fCost > openSet[i].fCost || (currentNode.fCost == openSet[i].fCost && 
                //                                                    currentNode.hCost > openSet[i].hCost))
                //    {
                //        currentNode = openSet[i];
                //    }
                //}
                //openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                if (currentNode == targetNode)
                {
                    //Debug.Log("find");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;
                    int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + neighbor.movementPenalty;
                    if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;
                        if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                        else openSet.UpdateItem(neighbor);
                    }
                }

            }
        }
        yield return null;
        if (pathSuccess) {
            wayPoints = RetracePath(startNode, targetNode);
        }
        pathRequestManager.FinishProcessingPath(wayPoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //path.Add(startNode);
        Vector3[] wayPoints = SimplifyPath(path);
        Array.Reverse(wayPoints);
        return wayPoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                wayPoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return wayPoints.ToArray();
    }

}
