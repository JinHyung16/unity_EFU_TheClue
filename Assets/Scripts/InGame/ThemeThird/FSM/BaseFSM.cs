using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughFSM
{
    public abstract class BaseFSM<T>
    {
        public abstract void EnterState(T state);
        public abstract void UpdateState(T state);
        public abstract void ExitState(T state);
    }
}
