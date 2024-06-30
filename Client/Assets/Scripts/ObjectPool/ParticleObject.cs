using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleObject : PoolObject
{
    [SerializeField] protected ParticleSystem[] _particleSystems;
    private UniTask[] _tasks;

    public override void Initialize()
    {
        base.Initialize();

        _tasks = new UniTask[_particleSystems.Length];

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        for (int i = 0; i < _particleSystems.Length; ++i)
            _particleSystems[i].Clear();
    }

    public void SetLocalScale(Vector3 localScale)
    {
        for (int i = 0; i < _particleSystems.Length; ++i)
            _particleSystems[i].transform.localScale = localScale;
    }

    public async UniTask Play()
    {
        for (int i = 0; i < _particleSystems.Length; ++i)
        {
            var i_ = i;
            _particleSystems[i_].Play();
            _tasks[i_] = UniTask.WaitUntil(() => _particleSystems[i_].isStopped);
        }

        await UniTask.WhenAll(_tasks).AttachExternalCancellation(DisableCancellationToken);

        Pool();
    }
}
