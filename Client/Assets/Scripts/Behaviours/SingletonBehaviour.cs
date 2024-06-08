using System.Threading;
using UnityEngine;

namespace Behaviour
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => _instance;
        private static T _instance;

        protected static CancellationToken cancellationToken => _cancellationTokenSource.Token;
        private static CancellationTokenSource _cancellationTokenSource;

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

            _cancellationTokenSource = new();
        }

        protected virtual void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}