using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using HughFSM;

public class EnemyMoveState : BaseFSM<EnemyFSM>
{
    #region Static
    private static readonly EnemyMoveState instnace = new EnemyMoveState();
    public static EnemyMoveState GetInstance
    {
        get
        {
            return instnace;
        }
    }
    #endregion

    private float curTime = 0.0f;
    private float secTime = 0.0f;

    public override void EnterState(EnemyFSM state)
    {
        curTime = 7.0f;
        secTime = 0.0f;

        state.MovementStart();
    }

    public override void UpdateState(EnemyFSM state)
    {
        if (state.IsAttackRange)
        {
            state.ChangeState(EnemyAttackState.GetInstance);
            Debug.Log("EnemyMove에서 공격임을 받고있다");
        }
        else if(!state.IsCallEnemy)
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
            if (secTime == 0.0f)
            {
                state.ChangeState(EnemyIdleState.GetInstance);
            }
        }
    }

    public override void ExitState(EnemyFSM state)
    {
        state.PlayAnimation(0);
    }

}
