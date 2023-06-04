using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughFSM;

public class GradStudentMoveState : BaseFSM<GradStudent>
{
    #region Static
    private static readonly GradStudentMoveState instnace = new GradStudentMoveState();
    public static GradStudentMoveState GetInstance
    {
        get
        {
            return instnace;
        }
    }
    #endregion

    private float curTime = 0.0f;
    private float secTime = 0.0f;

    public override void EnterState(GradStudent state)
    {
        state.MovementStart();

        curTime = Random.Range(8.0f, 15.0f);
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
                        state.ChangeState(GradStudentIdleState.GetInstance);
                    }
                }
            }
        }
    }

    public override void ExitState(GradStudent state)
    {
        state.MovementStop();
    }

}
