using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HughFSM
{
    public class StateMachine<T>
    {
        private T stateOwner;

        private BaseFSM<T> curState;
        private BaseFSM<T> previousState;

        public void AwakeFSM()
        {
            curState = null;
            previousState = null;
        }
        public void UpdateFSM()
        {
            if (curState != null)
            {
                curState.UpdateState(stateOwner);
            }
        }

        public void InitialSetting(T _owner, BaseFSM<T> _state)
        {
            this.stateOwner = _owner;
            ChangeState(_state);
        }

        public void ChangeState(BaseFSM<T> state)
        {
            if (state == curState)
            {
                return;
            }

            previousState = curState;

            //현재 상태가 있다면 종료 먼저 하기
            if (curState != null)
            {
                curState.ExitState(stateOwner);
            }
            curState = state; //새로운 상태 적용

            //새로운 상태 적용한게 null이 아니면 실행
            if (curState != null)
            {
                curState.EnterState(stateOwner);
            }
        }

        public void StateRevert()
        {
            if (previousState != null)
            {
                ChangeState(previousState);
            }
        }
    }
}