using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughFSM;

public class EnemyIdleState : BaseFSM<EnemyFSM>
{
    #region Static
    private static readonly EnemyIdleState instance = new EnemyIdleState();
    public static EnemyIdleState GetInstance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private float curTime = 0.0f;
    private float secTime = 0.0f;
    public override void EnterState(EnemyFSM _state)
    {
        curTime = 5.0f;
        secTime = 0.0f;

        _state.SetAimation(0);

        Debug.Log("Enemy Idle State 진입");
    }

    public override void UpdateState(EnemyFSM _state)
    {
        if (_state.IsAttackRange)
        {
            _state.ChangeState(EnemyAttackState.GetInstance);
        }
        else {

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
                _state.ChangeState(EnemyMoveState.GetInstance);
            }
        }
    }

    public override void ExitState(EnemyFSM _state)
    {
    }
}
