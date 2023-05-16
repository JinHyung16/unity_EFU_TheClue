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

    public override void EnterState(EnemyFSM state)
    {
        state.MovementStart();
        state.PlayAnimation(1);

        Debug.Log("EnemyMove 진입");
    }

    public override void UpdateState(EnemyFSM state)
    {
        if (!state.CanMove)
        {
            state.ChangeState(EnemyIdleState.GetInstance);
            Debug.Log("Enemy Move 그만");
        }
    }

    public override void ExitState(EnemyFSM state)
    {
    }

}
