using HughPathFinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    #region Static
    private static PathRequestManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion
    struct PathRequest
    {
        public Vector3 start;
        public Vector3 end;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            this.start = _start;
            this.end = _end;
            this.callback = _callback;
        }
    }

    [SerializeField] private PathFinding pathFinding;

    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest curPathRequest;

    private bool isProcessingPath = false;

    public static void RequestPath(Vector3 start, Vector3 end, Action<Vector3[], bool> callback) 
    {
        PathRequest newPathRequest = new PathRequest(start, end, callback);
        GetInstance.pathRequestQueue.Enqueue(newPathRequest);
        GetInstance.TryNextProcess();
    }

    private void TryNextProcess()
    {
        if (!isProcessingPath && 0 < pathRequestQueue.Count)
        {
            curPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(curPathRequest.start, curPathRequest.end);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        curPathRequest.callback(path, success);
        isProcessingPath = false;
        TryNextProcess();
    }
}
