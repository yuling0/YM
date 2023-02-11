using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace AStar
{
    public class AStarPathFinding : MonoBehaviour
    {
        Map map;
        Coroutine currentCoroutine;
        PathRequestManager requestManager;
        private void Awake()
        {
            map = GetComponent<Map>();
        }
        public void Init(PathRequestManager requestManager)
        {
            this.requestManager = requestManager;
        }
        public void FindPath(Vector3 startPos, Vector3 endPos)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = StartCoroutine(FindPathAsync(startPos, endPos));
        }
        public IEnumerator FindPathAsync(Vector3 startPos, Vector3 targetPos)
        {
            AStarNode startNode = map.GetNodeFromPosition(startPos);
            AStarNode endNode = map.GetNodeFromPosition(targetPos);
            Vector3[] wayPoints = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool isSuccess = false;
            if (startNode.isWalkable && endNode.isWalkable)
            {

                Heap<AStarNode> openList = new Heap<AStarNode>(map.MaxSize);
                HashSet<AStarNode> closeList = new HashSet<AStarNode>();
                map.closeList = closeList;


                openList.Add(startNode);

                while (!openList.IsEmpty())
                {
                    AStarNode cur = openList.Pop();

                    if (cur == endNode)         //找到目标点
                    {
                        isSuccess = true;
                        stopwatch.Stop();
                        print(stopwatch.ElapsedMilliseconds + "ms");
                        break;
                    }

                    AStarNode[] neighbors = map.GetNeighborFromNode(cur);

                    for (int i = 0; i < neighbors.Length; i++)          //遍历当前节点的邻居
                    {
                        if (!neighbors[i].isWalkable || closeList.Contains(neighbors[i])) continue;

                        int newCost = cur.g + GetDistance(cur, neighbors[i]) + neighbors[i].w;

                        if (newCost < neighbors[i].g || !openList.Contains(neighbors[i]))
                        {
                            neighbors[i].g = newCost;
                            neighbors[i].h = GetDistance(neighbors[i], endNode);
                            neighbors[i].parent = cur;

                            if (!openList.Contains(neighbors[i]))
                            {
                                openList.Add(neighbors[i]);
                            }
                            else
                            {
                                openList.Update(neighbors[i]);
                            }
                        }

                    }
                    closeList.Add(cur);
                }
            }

            yield return null;

            if (isSuccess)
            {
                wayPoints = RetracePath(startNode, endNode);
            }

            requestManager.ProcessCallback(wayPoints, isSuccess);

        }

        private Vector3[] RetracePath(AStarNode start, AStarNode end)
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode temp = end;

            while (temp != start)
            {
                path.Add(temp);
                temp = temp.parent;
            }
            path.Add(start);
            map.path = path;
            Vector3[] wayPoints = OptimizePath(path);

            Array.Reverse(wayPoints);

            return wayPoints;
        }

        private Vector3[] OptimizePath(List<AStarNode> path)
        {
            List<Vector3> wayPoints = new List<Vector3>();
            Vector2 oldDirection = Vector2.zero;
            wayPoints.Add(path[0].pos);
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 newDirection = new Vector2(path[i].x - path[i - 1].x, path[i].y - path[i - 1].y);

                if (newDirection != oldDirection)
                {
                    wayPoints.Add(path[i].pos);
                }
                oldDirection = newDirection;
            }

            return wayPoints.ToArray();
        }
        private int GetDistance(AStarNode n1, AStarNode n2)
        {
            int x = Mathf.Abs(n1.x - n2.x);         //行的差值
            int y = Mathf.Abs(n1.y - n2.y);         //列的差值

            if (x > y)                  //行大于列，所以应该按列斜着走，再竖直方向走
                return 14 * y + 10 * (x - y);
            return 14 * x + 10 * (y - x);     //列大于行，所以应该按行斜着走，再水平方向走
        }
    }

}
