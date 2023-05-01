using HughFSM;
using HughPathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradStudent : EnemyFSM
{
    [SerializeField] private float moveSpeed;

    private Animator enemyAnimator;
    private StateMachine<EnemyFSM> enemyState;

    //A star Path Move
    private Vector3[] movePath;
    private int targetPathIndex = 0;
    private IEnumerator pathFindIEnum;

    private Transform targetTransform; //충돌시 target의 위치
    private Vector3 targetLookDir; //target을 바라보는 방향 담을 변수

    //Attack Range
    public override bool IsAttackRange
    {
        get
        {
            return base.IsAttackRange;
        }
    }

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();

        enemyState = new StateMachine<EnemyFSM>();

        enemyState.InitialSetting(this, EnemyIdleState.GetInstance);

        pathFindIEnum = MoveToPath();
    }

    protected override void Update()
    {
        enemyState.UpdateFSM();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GradStudent 충돌 진입");
            targetTransform = other.gameObject.transform;
            IsAttackRange = true;
            ChangeState(EnemyAttackState.GetInstance);
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GradStudent 충돌 중");
            targetTransform = other.gameObject.transform;
            IsAttackRange = true;
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GradStudent 충돌 나감");
            IsAttackRange = false;
            ChangeState(EnemyIdleState.GetInstance);
        }
    }

    public override void ChangeState(BaseFSM<EnemyFSM> state)
    {
        enemyState.ChangeState(state);
        Debug.Log("GradStudent ChangeState");
    }
    public override void SetAimation(int index)
    {
    }
    public override void AttackEnemy()
    {
        targetLookDir = targetTransform.position - transform.position;
        targetLookDir.y = 0;
        Quaternion look = Quaternion.LookRotation(targetLookDir.normalized);
        transform.rotation = look;
    }

    public override void MovementStart()
    {
        PathManager.GetInstance.RequestPath(transform.position, PathFindCallBack);
    }

    public override void MovementStop()
    {
        if (pathFindIEnum != null)
        {
            StopCoroutine(pathFindIEnum);
            pathFindIEnum = MoveToPath();
        }
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
        Vector3 curWayPosition = transform.position;
        if (movePath != null)
        {
            curWayPosition = movePath[0];
        }
        Vector3 lastWayPosition = movePath[movePath.Length - 1];
        while (true)
        {
            if (Vector3.Distance(transform.position, lastWayPosition) <= 1.0f)
            {
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

            targetLookDir = curWayPosition - transform.position;
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
}
