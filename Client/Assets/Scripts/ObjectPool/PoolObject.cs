using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;

public abstract class PoolObject : MonoBehaviour
{
    public Transform Tr => tr;
    protected Transform tr;

    protected CancellationToken DisableCancellationToken => _cancellation.Token;
    private CancellationTokenSource _cancellation;

    public virtual void Initialize()
    {
        tr = GetComponent<Transform>();
    }

    protected virtual void OnEnable()
    {
        _cancellation = new CancellationTokenSource();
    }

    protected virtual void OnDisable()
    {
        _cancellation.Cancel();
        _cancellation.Dispose();
    }

    public void Activate() => gameObject.SetActive(true);

    // 풀에 다시 집어넣는 작업
    public abstract void Pool();
}