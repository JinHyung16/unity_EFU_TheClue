using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughFSM;

public class EnemyAttackState : BaseFSM<EnemyFSM>
{
    #region Static
    private static readonly EnemyAttackState instance = new EnemyAttackState();
    public static EnemyAttackState GetInstance
    {
        get
        {
            return instance;
        }
    }
    #endregion
    public override void EnterState(EnemyFSM _state)
    {
        _state.MovementStop();
        Debug.Log("Enemy Attack State 진입");
    }

    public override void UpdateState(EnemyFSM _state)
    {
        //만약 Enemy 공격 범위에 적이 있다면
        //아니라면 -> _state.ChangeState(EnemyMoveState.GetInstance);

        if (_state.IsAttackRange)
        {
            _state.AttackEnemy();
        }
    }

    public override void ExitState(EnemyFSM _state)
    {
    }
}