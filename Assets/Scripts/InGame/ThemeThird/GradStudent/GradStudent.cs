using Cysharp.Threading.Tasks;
using HughFSM;
using HughPathFinding;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class GradStudent : MonoBehaviour
{
    [SerializeField] private Transform gradStudentTransform;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    private Rigidbody enemyRigid;
    private Animator enemyAnimator;

    //FSM state machine
    private StateMachine<GradStudent> myState;

    //Path Find 코루틴
    private float pathFindTime = 0.0f;
    private float startTime = 0.0f;
    private Coroutine pathFindCoroutine = null;
    private Vector3 directionToTarget;

    //A star Path Move
    private Vector3[] movePath;
    private int targetPathIndex = 0;
    private Coroutine moveToPathCoroutine = null;

    //3=region 03 호출 지점으로, 4=region 04 호출 지점으로, other=target Chase
    public int OnChaseTarget { get; set; } = 2;

    private CancellationTokenSource tokenSource;

    public Animator EnemyAnimator { get { return this.enemyAnimator; } }

    public bool IsAttackTime { get; set; }
    public bool IsAttackDone { get; set; } = false;
    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody>();
        enemyAnimator = GetComponentInChildren<Animator>();

        myState = new StateMachine<GradStudent>();

        myState.InitialSetting(this, GradStudentIdleState.GetInstance);

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    private void Update()
    {
        myState.UpdateFSM();

        /*
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleAndWalkLayer.Blend Tree"))
        {
            Debug.Log("참");
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("장애물 위에 있는듯");
        }
    }
    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
    }

    public void ChangeState(BaseFSM<GradStudent> state)
    {
        myState.ChangeState(state);
    }

    public void EnemyIdle()
    {
        enemyAnimator.SetFloat("IsWalk", 0.0f);
        IsAttackDone = false;
    }

    public void EnemyAttack()
    {
        AttackDelay().Forget();
    }

    private async UniTaskVoid AttackDelay()
    {
        IsAttackDone = true;
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: tokenSource.Token);
        if (IsAttackTime)
        {
            ChangeState(GradStudentAttackState.GetInstance);
        }
        else
        {
            ChangeState(GradStudentIdleState.GetInstance);
        }
    }

    public void MovementStart()
    {
        IsAttackDone = false;
        OnChaseTarget = 0;

        if (pathFindCoroutine != null)
        {
            StopCoroutine(pathFindCoroutine);
        }
        pathFindCoroutine = StartCoroutine(PathFind());
    }

    public void MovementStop()
    {
        IsAttackDone = false;
        OnChaseTarget = 0;
        enemyAnimator.SetFloat("IsWalk", 0.0f);

        if (pathFindCoroutine != null)
        {
            StopCoroutine(pathFindCoroutine);
        }

        if (moveToPathCoroutine != null)
        {
            StopCoroutine(moveToPathCoroutine);
        }
    }

    private void ResetPathFindTime()
    {
        pathFindTime = 0.0f;
        startTime = Time.time;
    }

    private void PathFindCallBack(Vector3[] newPath, bool success)
    {
        if (success)
        {
            movePath = newPath;
            targetPathIndex = 0;
            if (moveToPathCoroutine != null)
            {
                StopCoroutine(moveToPathCoroutine);
            }
            moveToPathCoroutine = StartCoroutine(MoveToPath());
        }
    }

    private IEnumerator PathFind()
    {
        while (true)
        {
            pathFindTime = Time.time - startTime;
            if (1.0f <= pathFindTime)
            {
                PathManager.GetInstance.RequestPath(gradStudentTransform.position, PathFindCallBack, OnChaseTarget);
                ResetPathFindTime();
            }
            yield return null;
        }
    }

    private IEnumerator MoveToPath()
    {
        Vector3 curWayPosition = movePath[0];

        while (true)
        {
            if (transform.position == curWayPosition)
            {
                targetPathIndex ++;
                if (movePath.Length <= targetPathIndex)
                {
                    yield break;
                }

                //exception handling
                try
                {
                    curWayPosition = movePath[targetPathIndex];
                    throw new IndexOutOfRangeException();
                }
                catch (System.IndexOutOfRangeException e)
                {
                    Debug.Log("IndexOutOfRangeException : " + e);
                }
                finally
                {
                    curWayPosition = movePath[targetPathIndex];
                }
            }

            //움직여야 하는 target 방향 계산
           directionToTarget = curWayPosition - gradStudentTransform.position;

            //움직이는 방향으로 쳐다볼 수 있게 회전 1
            directionToTarget.y = 0;
            Quaternion look = Quaternion.LookRotation(directionToTarget);
            enemyRigid.transform.rotation = Quaternion.Lerp(gradStudentTransform.rotation, look, turnSpeed * Time.deltaTime);

            //움직이는 방향으로 쳐다볼 수 있게 회전
            //Quaternion look = Quaternion.LookRotation(directionToTarget);
            //enemyRigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, look, turnSpeed));

            //타겟 방향으로 움직이기 1
            //enemyRigid.transform.position = Vector3.MoveTowards(transform.position, curWayPosition, moveSpeed * Time.deltaTime);

            //타겟 방향으로 움직이기 2
            float directionMagnitude = Mathf.Clamp01(directionToTarget.magnitude);
            enemyAnimator.SetFloat("IsWalk", directionMagnitude);
            directionToTarget.Normalize();
            enemyRigid.MovePosition(gradStudentTransform.position + directionToTarget * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
