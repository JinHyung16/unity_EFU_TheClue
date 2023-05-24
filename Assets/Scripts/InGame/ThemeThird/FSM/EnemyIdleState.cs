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
    public override void EnterState(EnemyFSM state)
    {
        curTime = 5.0f;
        secTime = 0.0f;

        state.PlayAnimation(0);
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

            if (secTime == 0.0f)
            {
                if (!state.IsAttackTime)
                {
                    state.ChangeState(EnemyMoveState.GetInstance);
                }
            }
        }
    }

    public override void ExitState(EnemyFSM state)
    {
        Debug.Log("EnemyIdle 나감");
    }
}
