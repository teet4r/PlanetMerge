using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Behaviour
{
    public class SceneSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => _instance;
        private static T _instance;

        protected virtual void Awake()
        {
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}