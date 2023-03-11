using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughGenerics
{
    public class PresenterSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 각 Scene의 있는 Presenter들이 사용할 Template이다.
        /// Scene이 바뀌면 해당 Presenter는 사라져야 하므로, DontDestroyOnLoad 사용하지 않는다.
        /// </summary>
        private static T instance;
        public static T GetInstance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
}
