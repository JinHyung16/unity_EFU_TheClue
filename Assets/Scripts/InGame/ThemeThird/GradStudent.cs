using HughFSM;
using HughPathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradStudent : EnemyFSM
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rayMaxDistance;
    [SerializeField] private Transform enemyHead;

    private Animator enemyAnimator;
    private StateMachine<EnemyFSM> enemyState;

    //A star Path Move
    private Vector3[] movePath;
    private int targetPathIndex = 0;
    private IEnumerator pathFindIEnum;

    private Transform targetTransform; //충돌시 target의 위치
    private Vector3 targetLookDir; //target을 바라보는 방향 담을 변수

    private bool onChaseTarget = false;

    //Attack Range
    public override bool IsAttackRange
    {
        get
        {
            return base.IsAttackRange;
        }
    }

    public override bool IsCallEnemy { get => base.IsCallEnemy; set => base.IsCallEnemy = value; }

    private void Start()
    {
        enemyAnimator = GetComponentInChildren<Animator>();

        enemyState = new StateMachine<EnemyFSM>();

        enemyState.InitialSetting(this, EnemyIdleState.GetInstance);

        pathFindIEnum = MoveToPath();
    }

    protected override void Update()
    {
        if (Physics.Raycast(enemyHead.position, transform.forward, out RaycastHit hit, rayMaxDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                onChaseTarget = true;
            }
            else
            {
                onChaseTarget = false;
            }
            Debug.DrawRay(enemyHead.position, transform.forward * rayMaxDistance, Color.red);
        }

        enemyState.UpdateFSM();
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
        enemyState.ChangeState(state);
    }


    /// <summary>
    /// index에 맞춰 animation을 바꾼다.
    /// 0 = idle, 1 = walk, 2 = attack
    /// </summary>
    /// <param name="index"> 현재 state에 따라 바꿀 애니메이션 index </param>
    public override void PlayAnimation(int index)
    {
        switch (index)
        {
            case 0:
                enemyAnimator.SetBool("IsWalk", false);
                enemyAnimator.SetBool("IsAttack", false);
                break;
            case 1:
                enemyAnimator.SetBool("IsWalk", true);
                break;
            case 2:
                enemyAnimator.SetBool("IsAttack", true);
                break;
        }
    }

    public override void AttackEnemy()
    {
        targetLookDir = targetTransform.position - transform.position;
        targetLookDir.y = 0;
        Quaternion look = Quaternion.LookRotation(targetLookDir.normalized);
        transform.rotation = look;

        this.PlayAnimation(2);
    }

    public override void MovementStart()
    {
        PathManager.GetInstance.RequestPath(transform.position, PathFindCallBack, onChaseTarget);
        this.PlayAnimation(1);
    }

    public override void MovementStop()
    {
        if (pathFindIEnum != null)
        {
            Debug.Log("GradStudent Movement Stop");
            StopCoroutine(pathFindIEnum);
            pathFindIEnum = MoveToPath();
        }
        this.PlayAnimation(0);
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
        Vector3 curWayPosition = this.transform.position;
        if (movePath != null)
        {
            curWayPosition = movePath[0];
        }
        Vector3 lastWayPosition = movePath[movePath.Length - 1];
        while (true)
        {
            if (Vector3.Distance(this.transform.position, lastWayPosition) <= 1.0f)
            {
                yield break;
            }

            if (this.transform.position == curWayPosition)
            {
                targetPathIndex++;
                if (movePath.Length <= targetPathIndex)
                {
                    yield break;
                }
                curWayPosition = movePath[targetPathIndex];
            }

            targetLookDir = curWayPosition - this.transform.position;
            targetLookDir.y = 0;
            Quaternion look = Quaternion.LookRotation(targetLookDir.normalized);
            this.transform.rotation = look;
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(curWayPosition.x, transform.position.y, curWayPosition.z), 
                moveSpeed * Time.deltaTime);
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
