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
    //Attack Range
    public override bool CanMove
    {
        get
        {
            return base.CanMove;
        }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        //myState = new StateMachine<EnemyFSM>();

        /*
        myState.InitialSetting(this, EnemyIdleState.GetInstance);
        pathFindIEnum = MoveToPath();
        */
    }

    protected override void Update()
    {
        //myState.UpdateFSM();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            targetTransform = other.gameObject.transform;
            ChangeState(EnemyAttackState.GetInstance);
        }
        */
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        switch (index)
        {
            case 0:
                //animator.SetBool("IsRun", false);
                //animator.SetBool("IsSurprised", false);
                //animator.SetBool("IsLock", false);
                break;
            case 1:
                //animator.SetBool("IsRun", true);
                break;
            case 2:
                //animator.SetBool("IsSurprised", true);
                break;
            case 3:
                //animator.SetBool("IsLock", true);
                break;
        }
    }

    public override void AttackEnemy()
    {
        /*
        targetLookDir = targetTransform.position - transform.position;
        targetLookDir.y = 0;
        Quaternion look = Quaternion.LookRotation(targetLookDir.normalized);
        transform.rotation = look;
        */
    }

    public override void MovementStart()
    {
        //PathManager.GetInstance.RequestPath(transform.position, PathFindCallBack, 0);
    }

    public override void MovementStop()
    {
        /*
        if (pathFindIEnum != null)
        {
            StopCoroutine(pathFindIEnum);
            pathFindIEnum = MoveToPath();
        }
        */
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
                    CanMove = false;
                    yield break;
                }
                else
                {
                    CanMove = true;
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
    #endregion
}
