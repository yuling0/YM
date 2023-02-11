using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AStar
{
    public class PathRequestManager : SingletonBase<PathRequestManager>
    {
        private struct PathRequestParam
        {
            public Vector3 startPos;
            public Vector3 endPos;
            public UnityAction<Vector3[], bool> OnPathSuccess;

            public PathRequestParam(Vector3 startPos, Vector3 endPos, UnityAction<Vector3[], bool> callback)
            {
                this.startPos = startPos;
                this.endPos = endPos;
                this.OnPathSuccess = callback;
            }
        }
        AStarPathFinding pathFinding;
        Queue<PathRequestParam> requestQueue;
        bool isProcessing;
        PathRequestParam currentReuqest;

        private PathRequestManager()
        {
            requestQueue = new Queue<PathRequestParam>();

            GameObject pathFindingObj = new GameObject() { name = "A*" };

            pathFindingObj.transform.position = Vector3.zero;

            pathFindingObj.AddComponent<Map>();

            pathFinding = pathFindingObj.AddComponent<AStarPathFinding>();
            pathFinding.Init(this);
        }


        public void PathRequest(Vector3 startPos, Vector3 endPos, UnityAction<Vector3[], bool> callback)
        {
            requestQueue.Enqueue(new PathRequestParam(startPos, endPos, callback));
            TryProcessRequest();
        }

        private void TryProcessRequest()
        {
            if (!isProcessing && requestQueue.Count > 0)
            {
                isProcessing = true;
                currentReuqest = requestQueue.Dequeue();
                pathFinding.FindPath(currentReuqest.startPos, currentReuqest.endPos);
            }
        }

        public void ProcessCallback(Vector3[] wayPoints, bool success)
        {
            currentReuqest.OnPathSuccess?.Invoke(wayPoints, success);
            isProcessing = false;
            TryProcessRequest();
        }
    }

}
