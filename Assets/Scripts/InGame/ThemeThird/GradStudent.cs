using Cysharp.Threading.Tasks;
using HughFSM;
using HughPathFinding;
using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GradStudent : EnemyFSM
{
    [SerializeField] private float moveSpeed;
    //[SerializeField] private float rayMaxDistance;
    //[SerializeField] private Transform ememyRayPos;

    private Animator enemyAnimator;
    private StateMachine<EnemyFSM> myState;

    //A star Path Move
    private Vector3[] movePath;
    private int targetPathIndex = 0;

    private Transform targetTransform; //충돌시 target의 위치
    //private Vector3 targetLookDir; //target을 바라보는 방향 담을 변수
    //private RaycastHit hit;

    //1=player 쫓기, 2=랜덤 이동, 3=region 03 호출, 4=region 04호출
    public int OnChaseTarget { get; set; } = 2;

    private CancellationTokenSource tokenSource;

    public override bool IsMoveDone { get => base.IsMoveDone; protected set => base.IsMoveDone = value; }
    public override bool IsAttackTime { get => base.IsAttackTime; protected set => base.IsAttackTime = value; }
    private void Start()
    {
        enemyAnimator = GetComponentInChildren<Animator>();

        myState = new StateMachine<EnemyFSM>();

        myState.InitialSetting(this, EnemyIdleState.GetInstance);

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    protected override void Update()
    {
        /*
        if (Physics.Raycast(ememyRayPos.position, transform.forward, out hit, rayMaxDistance) || 
            Physics.Raycast(ememyRayPos.position, -transform.forward, out hit, rayMaxDistance))
        {
            if (!ThemeThirdPresenter.GetInstance.IsCallEnemyAnimation && !IsAttackTime && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Player")))
            {
                OnChaseTarget = 1;
                MovementStart();
            }
            Debug.DrawRay(ememyRayPos.position, transform.forward * rayMaxDistance, Color.red);
            Debug.DrawRay(ememyRayPos.position, -transform.forward * rayMaxDistance, Color.red);
        }
        */
        myState.UpdateFSM();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = other.gameObject.transform;
            Vector3 attackTargetLookDir = targetTransform.position - this.transform.position;
            attackTargetLookDir.y = 0;
            Quaternion look = Quaternion.LookRotation(attackTargetLookDir.normalized);
            this.transform.rotation = look;
            IsAttackTime = true;
            ChangeState(EnemyAttackState.GetInstance);
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = other.gameObject.transform;
            IsAttackTime = true;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAttackTime = false;
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
        IsAttackTime = true;
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && 1.0f <= enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            Debug.Log("공격 애니메이션 끝나습니다.");
            IsAttackTime = false;
            ChangeState(EnemyIdleState.GetInstance);
        }
    }

    public override void MovementStart()
    {
        PathManager.GetInstance.RequestPath(this.transform.position, PathFindCallBack, OnChaseTarget);
    }

    public override void MovementStop()
    {
        StopCoroutine("MoveToPath");
        this.PlayAnimation(0);

        IsMoveDone = false;
        OnChaseTarget = 2;
    }

    #region Enemy PathFind Move
    private void PathFindCallBack(Vector3[] newPath, bool success)
    {
        if (success)
        {
            movePath = newPath;
            targetPathIndex = 0;
            StopCoroutine("MoveToPath");
            StartCoroutine("MoveToPath");
        }
    }

    private IEnumerator MoveToPath()
    {
        Vector3 curWayPosition = movePath[0];
        while (true)
        {
            if (this.transform.position == curWayPosition)
            {
                targetPathIndex++;
                if (movePath.Length <= targetPathIndex)
                {
                    IsMoveDone = true;
                    yield break;
                }
                curWayPosition = movePath[targetPathIndex];
            }
            //this.PlayAnimation(1);
            Vector3 targetLookDir = curWayPosition - this.transform.position;
            targetLookDir.y = 0;
            Quaternion look = Quaternion.LookRotation(targetLookDir.normalized);
            this.transform.rotation = look;

            this.transform.position = Vector3.MoveTowards(this.transform.position, curWayPosition, moveSpeed * Time.deltaTime);
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
