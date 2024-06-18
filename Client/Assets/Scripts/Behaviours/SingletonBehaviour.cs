using System.Threading;
using UnityEngine;

namespace Behaviour
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}