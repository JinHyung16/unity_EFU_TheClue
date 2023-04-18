using HughPathFinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed;

    private Vector3[] path;
    private int targetPathIndex = 0;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        PathRequestManager.RequestPath(transform.position, targetTransform.position, OnPathFind);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PathRequestManager.RequestPath(transform.position, targetTransform.position, OnPathFind);
        }
    }
    public void OnPathFind(Vector3[] newPath, bool success)
    {
        if (success)
        {
            path = newPath;
            targetPathIndex = 0;
            StopCoroutine("MoveToPath");
            StartCoroutine("MoveToPath");
        }
    }
    private IEnumerator MoveToPath()
    {
        Vector3 curWayPosition = path[0];
        while (true)
        {
            if (transform.position == curWayPosition)
            {
                targetPathIndex++;
                if (path.Length <= targetPathIndex)
                {
                    yield break;
                }
                curWayPosition = path[targetPathIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, curWayPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetPathIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetPathIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
