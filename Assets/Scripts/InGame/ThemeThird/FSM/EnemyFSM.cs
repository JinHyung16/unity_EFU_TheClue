using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughFSM;
using HughPathFinding;

public class EnemyFSM : MonoBehaviour
{
    /*
    [SerializeField] private float moveSpeed;

    private Animator enemyAnimator;
    private StateMachine<EnemyFSM> enemyState;

    //A star Path Move
    private Vector3[] movePath;
    private int targetPathIndex = 0;
    private IEnumerator pathFindIEnum;
    */


    //Attack Range
    public virtual bool IsAttackRange { get; protected set; } = false;

    private void Start()
    {
        //enemyAnimator = GetComponent<Animator>();

        //enemyState = new StateMachine<EnemyFSM>();

        //enemyState.InitialSetting(this, EnemyIdleState.GetInstance);

        //pathFindIEnum = MoveToPath();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            Debug.Log("EnemyFSM 충돌 진입");
            IsAttackRange = true;
            ChangeState(EnemyAttackState.GetInstance);
        }
        */
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            Debug.Log("EnemyFSM 충돌 중");
            IsAttackRange = true;
            ChangeState(EnemyAttackState.GetInstance);
        }
        */
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            Debug.Log("EnemyFSM 충돌 나감");
            IsAttackRange = false;
            ChangeState(EnemyIdleState.GetInstance);
        }
        */
    }

    public virtual void ChangeState(BaseFSM<EnemyFSM> state)
    {
        //Debug.Log("EnemyFSM ChangeState");
    }
    public virtual void SetAimation(int index)
    {
        //Debug.Log("EnemyFSM SetAnimation");
    }

    public virtual void AttackEnemy()
    {
    }
    public virtual void MovementStart()
    {
        //Debug.Log("EnemyFSM MovementStart");
        //PathManager.GetInstance.RequestPath(transform.position, PathFindCallBack);
    }

    public virtual void MovementStop()
    {
        //ebug.Log("EnemyFSM MovementStop");
        /*
        if (pathFindIEnum != null)
        {
            StopCoroutine(pathFindIEnum);
            pathFindIEnum = MoveToPath();
        }
        */
    }

    /*
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
        Vector3 curWayPosition = transform.position;
        if (movePath != null)
        {
            curWayPosition = movePath[0];
        }
        Vector3 lastWayPosition = movePath[movePath.Length - 1];
        Debug.Log("TargetLastPos: " + lastWayPosition);
        while (true)
        {
            if (Vector3.Distance(transform.position, lastWayPosition) <= 1.0f)
            {
                Debug.Log("Enemym가 움직일 수 있는 거리가 1칸 아래라 멈췄습니다");
                yield break;
            }

            if (transform.position == curWayPosition)
            {
                targetPathIndex++;
                if (movePath.Length <= targetPathIndex)
                {
                    yield break;
                }
                curWayPosition = movePath[targetPathIndex];
            }

            Vector3 targetLookDir = curWayPosition - transform.position;
            targetLookDir.y = 0;
            Quaternion look = Quaternion.LookRotation(targetLookDir.normalized);
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
    */
}
