using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughFSM;

public class GradStudentAttackState : BaseFSM<GradStudent>
{
    #region Static
    private static readonly GradStudentAttackState instance = new GradStudentAttackState();
    public static GradStudentAttackState GetInstance
    {
        get
        {
            return instance;
        }
    }
    #endregion
    public override void EnterState(GradStudent state)
    {
        state.EnemyAnimator.SetTrigger("onAttack");
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.ThemeThird_EnemyAttack);
    }

    public override void UpdateState(GradStudent state)
    {
        state.EnemyAttack();
    }

    public override void ExitState(GradStudent state)
    {
        state.IsAttackDone = false;
    }
}
