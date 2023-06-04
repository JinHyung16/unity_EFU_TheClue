using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughFSM;

public class GradStudentIdleState : BaseFSM<GradStudent>
{
    #region Static
    private static readonly GradStudentIdleState instance = new GradStudentIdleState();
    public static GradStudentIdleState GetInstance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private float curTime = 0.0f;
    private float secTime = 0.0f;
    public override void EnterState(GradStudent state)
    {
        Debug.Log("Idle 진입");

        state.EnemyIdle();

        curTime = Random.Range(5.0f, 10.0f);
        secTime = 0.0f;
    }

    public override void UpdateState(GradStudent state)
    {
        if (!ThemeThirdPresenter.GetInstance.IsCallEnemyAnimation)
        {
            if (state.IsAttackTime)
            {
                state.ChangeState(GradStudentAttackState.GetInstance);
            }
            else
            {
                if (curTime > 0.0f)
                {
                    curTime -= Time.deltaTime;
                    secTime = Mathf.FloorToInt(curTime % 60);
                }
                else
                {
                    if (curTime != 0.0f)
                    {
                        curTime = 0.0f;
                        secTime = Mathf.FloorToInt(curTime % 60);
                    }
                }
                if (secTime <= 0.0f)
                {
                    if (!state.IsAttackTime)
                    {
                        state.ChangeState(GradStudentMoveState.GetInstance);
                    }
                }
            }
        }
    }

    public override void ExitState(GradStudent state)
    {
    }
}
