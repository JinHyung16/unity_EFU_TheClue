using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughGenerics
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T GetInstance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("Singleton Generic을 상속받은 오브젝트가 없습니다");
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

            //T type이 중복되어 있을 경우 1개만 남기고 삭제시켜준다.
            var T_typeObjects = FindObjectsOfType<T>();
            if (T_typeObjects.Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
