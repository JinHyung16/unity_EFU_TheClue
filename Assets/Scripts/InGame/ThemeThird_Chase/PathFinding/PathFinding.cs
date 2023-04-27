using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace HughPathFinding
{
    public class PathFinding : MonoBehaviour
    {
        //[SerializeField] private PathRequestManager pathRequestManager;
        [SerializeField] private AstarGrid astarGrid;
        [SerializeField] private PathManager pathManager;
        public void StartFindPath(Vector3 start, Vector3 target)
        {
            StartCoroutine(FindPath(start, target));
        }

        private IEnumerator FindPath(Vector3 start, Vector3 target)
        {
            //Stopwatch stopWatch = Stopwatch.StartNew();
            //stopWatch.Start();

            Vector3[] wayPoints = new Vector3[0];
            bool findPathDone = false;

            Node startNode = astarGrid.NodeFrowmWorldPosition(start);
            Node targetNode = astarGrid.NodeFrowmWorldPosition(target);

            if (startNode.isWalkable && targetNode.isWalkable)
            {
                //List<Node> openSet = new List<Node>(); //거리를 계사한해야할 노드 집합
                Heap<Node> openSet = new Heap<Node>(astarGrid.MaxSize);
                HashSet <Node> closedSet = new HashSet<Node>(); //이미 계산이 완료된 노드 집합
                openSet.Add(startNode);
                
                while (0 < openSet.Count)
                {
                    Node curNode = openSet.RemoveFirst();
                    /*
                    Node curNode = openSet[0];
                    for (int i = 1; i < openSet.Count; i++)
                    {
                        if (openSet[i].FCost < curNode.FCost || openSet[i].FCost == curNode.FCost)
                        {
                            if (openSet[i].hCost < curNode.hCost)
                            {
                                curNode = openSet[i];
                            }
                        }
                    }
                    openSet.Remove(curNode);
                    */

                    closedSet.Add(curNode);
                    if (curNode == targetNode)
                    {
                        findPathDone = true;
                        //stopWatch.Stop();
                        //print("Path Found Time: " + stopWatch.ElapsedMilliseconds + " ms");
                        break;
                    }

                    //최단경로의 상하좌우 1칸씩을 보면서 최단 경로를 계속 찾아간다
                    foreach (Node neighbor in astarGrid.GetNeighborhoodNodes(curNode))
                    {
                        if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                        {
                            continue;
                        }
                        int moveCost = curNode.gCost + GetManhattenDistance(curNode, neighbor);
                        if (moveCost < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = moveCost;
                            neighbor.hCost = GetManhattenDistance(neighbor, targetNode);
                            neighbor.Parent = curNode;
                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);
                            }
                        }
                    }
                }
            }
            yield return null;
            if (findPathDone)
            {
                wayPoints = GetFinalPath(startNode, targetNode);
            }
            pathManager.DonePathFinding(wayPoints, findPathDone);
        }


        //찾은 최단경로 패스를 다시 그래프로 생각하면 맨 아래부터 최상위 부모까지 거꾸로 올라가며 그린다.
        private Vector3[] GetFinalPath(Node startNode, Node endNode)
        {
            List<Node> finalPathList = new List<Node>();
            Node curNode = endNode;
            while (curNode != startNode)
            {
                finalPathList.Add(curNode);
                curNode = curNode.Parent;
            }
            Vector3[] wayPoints = ConvertFindPathToVector(finalPathList);
            Array.Reverse(wayPoints);
            return wayPoints;
        }

        Vector3[] ConvertFindPathToVector(List<Node> path)
        {
            List<Vector3> wayPointList = new List<Vector3>();
            Vector2 dirOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 dirNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (dirOld != dirNew)
                {
                    wayPointList.Add(path[i].nodePosition);
                }
                dirOld = dirNew;
            }
            return wayPointList.ToArray();
        }

        private int GetManhattenDistance(Node aNode, Node bNode)
        {
            int distX = Mathf.Abs(aNode.gridX - bNode.gridX);
            int distY = Mathf.Abs(aNode.gridY - bNode.gridY);
            return distX + distY;
        }
    }
}