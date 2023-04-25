using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughFSM;
using HughPathFinding;
using System.IO.Ports;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Animator enemyAnimator;
    private StateMachine<EnemyFSM> enemyState;

    private Vector3[] movePath;
    private int targetPathIndex = 0;

    private IEnumerator pathFindIEnum;
    public bool IsAttackRange { get; private set; } = false;
    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();

        enemyState = new StateMachine<EnemyFSM>();

        enemyState.InitialSetting(this, EnemyIdleState.GetInstance);

        pathFindIEnum = MoveToPath();
    }

    private void Update()
    {
        enemyState.UpdateFSM();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player 와 충돌 진입");
            IsAttackRange = true;
            ChangeState(EnemyAttackState.GetInstance);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player 와 충돌 나감");
            IsAttackRange = false;
            ChangeState(EnemyIdleState.GetInstance);
        }
    }

    public void ChangeState(BaseFSM<EnemyFSM> state)
    {
        enemyState.ChangeState(state);
    }
    public void SetAimation(int index)
    {
        //enemyAnimator.SetInteger("EnemyState", index);
    }

    public void EnemyMovement()
    {
        PathManager.GetInstance.RequestPath(transform.position, PathFindCallBack);
    }

    public void EnemyStopMovement()
    {
        StopCoroutine(pathFindIEnum);
        Debug.Log("Enemym 코루틴 멈춰!");
    }
    #region Enemy PathFind Move
    private void PathFindCallBack(Vector3[] newPath, bool success)
    {
        if (success)
        {
            movePath = newPath;
            targetPathIndex = 0;
            StopCoroutine(pathFindIEnum);
            pathFindIEnum = MoveToPath();
            StartCoroutine(pathFindIEnum);
        }
    }
    private IEnumerator MoveToPath()
    {
        if (movePath == null)
        {
            yield break;
        }

        if (movePath.Length <= 0)
        {
            Debug.Log("Enemym가 움직일 수 있는 거리가 1칸 아래인거같아요");
        }

        Vector3 curWayPosition = movePath[0];
        while (true)
        {
            if (transform.position == curWayPosition)
            {
                targetPathIndex++;
                if (movePath.Length <= targetPathIndex)
                {
                    yield break;
                }
                curWayPosition = movePath[targetPathIndex];
            }

            Vector3 dir = curWayPosition - transform.position;
            dir.y = 0;
            Quaternion look = Quaternion.LookRotation(dir.normalized);
            transform.rotation = look;

            transform.position = Vector3.MoveTowards(transform.position, curWayPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (movePath != null)
        {
            for (int i = targetPathIndex; i < movePath.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(movePath[i], Vector3.one);

                if (i == targetPathIndex)
                {
                    Gizmos.DrawLine(transform.position, movePath[i]);
                }
                else
                {
                    Gizmos.DrawLine(movePath[i - 1], movePath[i]);
                }
            }
        }
    }
    #endregion
}
