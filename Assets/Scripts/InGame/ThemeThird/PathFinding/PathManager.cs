using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughPathFinding
{
    public class PathManager : MonoBehaviour
    {
        #region Static
        public static PathManager GetInstance;
        private void Awake()
        {
            GetInstance = this;
        }
        #endregion

        struct Path //경로를 탐색하여 찾을때마다 새로 만들어 Queue에 넣는다.
        {
            public Vector3 start;
            public Vector3 end;
            public Action<Vector3[], bool> callback;
            public Path(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
            {
                this.start = _start;
                this.end = _end;
                this.callback = _callback;
            }
        }

        [SerializeField] private PathFinding pathFinding; //최단 경로를 찾아주는 script

        [SerializeField] private Transform playerTransform; //target position

        private Queue<Path> pathQueue = new Queue<Path>(); //찾은 path data를 넣는 queue
        private Path curPath; //현재 경로

        private bool isProcessingPathDone; //현재 찾은 경로에 대해 처리가 다 끝났는지 확인
        public Vector3[] FinalPath { get; private set; }

        private void TryNextPathFind()
        {
            if (!isProcessingPathDone && 0 < pathQueue.Count)
            {
                curPath = pathQueue.Dequeue();
                isProcessingPathDone = true;
                pathFinding.StartFindPath(curPath.start, curPath.end);
            }
        }

        public void RequestPath(Vector3 start, Action<Vector3[], bool> callback)
        {
            Path newPath = new Path(start, playerTransform.position, callback);
            pathQueue.Enqueue(newPath);
            TryNextPathFind();
        }

        public void DonePathFinding(Vector3[] finalPath, bool success)
        {
            curPath.callback(finalPath, success);
            isProcessingPathDone = false;
            TryNextPathFind();
        }

        public void SetTargetPosition(Transform target)
        {
            this.playerTransform = target;
        }
    }
}
