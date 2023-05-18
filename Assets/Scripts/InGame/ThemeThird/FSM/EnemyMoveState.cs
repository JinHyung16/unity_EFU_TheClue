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
        curTime = 6.0f;
        secTime = 0.0f;

        state.MovementStart();
        state.PlayAnimation(1);
    }

    public override void UpdateState(EnemyFSM state)
    {
        if (!ThemeThirdPresenter.GetInstance.IsCallEnemyAnimation)
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

            if (secTime == 0.0f || state.IsMoveDone)
            {
                state.ChangeState(EnemyIdleState.GetInstance);
            }
        }
    }

    public override void ExitState(EnemyFSM state)
    {
        Debug.Log("EnemyMove 나감");
    }

}
