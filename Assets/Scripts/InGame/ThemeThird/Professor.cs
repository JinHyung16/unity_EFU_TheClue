using HughFSM;
using HughPathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : EnemyFSM
{
    [SerializeField] private float moveSpeed;

    private Rigidbody rigid;
    private Animator animator;
    private StateMachine<EnemyFSM> myState;

    //A star Path Move
    private Vector3[] movePath;
    private int targetPathIndex = 0;
    private IEnumerator pathFindIEnum;

    private Transform targetTransform; //충돌시 target의 위치
    private Vector3 targetLookDir; //target을 바라보는 방향 담을 변수

    public override bool IsCallEnemy { get => base.IsCallEnemy; set => base.IsCallEnemy = value; }
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
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        myState = new StateMachine<EnemyFSM>();

        /*
        myState.InitialSetting(this, EnemyIdleState.GetInstance);
        pathFindIEnum = MoveToPath();
        */
    }

    protected override void Update()
    {
        myState.UpdateFSM();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = other.gameObject.transform;
            IsAttackRange = true;
            ChangeState(EnemyAttackState.GetInstance);
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = other.gameObject.transform;
            IsAttackRange = true;
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAttackRange = false;
            ChangeState(EnemyIdleState.GetInstance);
        }
    }

    public override void ChangeState(BaseFSM<EnemyFSM> state)
    {
        myState.ChangeState(state);
    }

    /// <summary>
    /// index에 맞춰 animation을 바꾼다.
    /// 0 = idle, 1 = walk, 2 = attack
    /// </summary>
    /// <param name="index"> 현재 state에 따라 바꿀 애니메이션 index </param>
    public override void PlayAnimation(int index)
    {
        /*
        switch (index)
        {
            case 0:
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsAttack", false);
                break;
            case 1:
                animator.SetBool("IsWalk", true);
                break;
            case 2:
                animator.SetBool("IsAttack", true);
                break;
        }
        */
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
        PathManager.GetInstance.RequestPath(transform.position, PathFindCallBack, true);
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

    /*
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
    */
    #endregion
}
