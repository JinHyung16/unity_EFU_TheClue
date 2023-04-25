using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughFSM
{
    public abstract class BaseFSM<T>
    {
        public abstract void EnterState(T _state);
        public abstract void UpdateState(T _state);
        public abstract void ExitState(T _state);
    }
}
