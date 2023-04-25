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

    public override void EnterState(EnemyFSM _state)
    {
        curTime = 7.0f;
        secTime = 0.0f;

        _state.EnemyMovement();
    }

    public override void UpdateState(EnemyFSM _state)
    {
        if (_state.IsAttackRange)
        {
            Debug.Log("움직이던 중 Player와 마주침");
            _state.ChangeState(EnemyAttackState.GetInstance);
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
            if (secTime == 0.0f)
            {
                _state.ChangeState(EnemyIdleState.GetInstance);
            }
        }
    }

    public override void ExitState(EnemyFSM _state)
    {
    }

}
